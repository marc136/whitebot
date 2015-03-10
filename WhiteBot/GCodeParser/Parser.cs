using WhiteBot;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GCodeParser
{
    enum GcodeMovement
    {
        Linear, Fast, Circular
    }

    public class Parser
    {
        private static Regex matcher = null;
        private string input;
        
        MatchCollection matches = null;
        bool absolutePositioning = true; //gCode supports absolute or relative positioning
        
        GcodeMovement movement = GcodeMovement.Linear;
        Vector2 targetVector = new Vector2(0, 0);
        
        private bool moveCommandWasRead = false;
        Vector2 lastVector = new Vector2(0, 0);
        private float currentZPosition = 0;

        public Queue<ACommand> GetCommandQueue { get { return commands; } }
        Queue<ACommand> commands = new Queue<ACommand>();

        public Vector2 CenterPoint = Vector2.Zero;

        public Parser()
        {
            if (matcher == null)
            {
                //add F for feed size?
                const string matchCodes = @"[GXYZ][+-]?[0-9]*\.?[0-9]­*"; //matches Codes like G01 and Z1.1
                const string matchNewlines = @"[\n\r]+\s*";
                const string matchString = matchCodes + "|" + matchNewlines;

                matcher = new Regex(matchString, RegexOptions.IgnoreCase & RegexOptions.Compiled);
            }
        }

        public Parser(string gCode) : this()
        {
            input = gCode;
        }
        
        public void Parse(string gCode = "")
        {
            if (gCode != "") input = gCode;
            if (String.IsNullOrWhiteSpace(input)) throw new ArgumentNullException("The parser needs an input string to work!");
            
            input = gCode;
            SplitCommands();

            ParseCommands();
        }

        private void SplitCommands()
        {
            matches = matcher.Matches(input);
        }

        private void ParseCommands()
        {
            foreach (Match match in matches)
            {
                if (MatchWasHandledAsNewLine(match)) continue;

                HandleParsedCodes(match.Value.ToUpper());
            }
        }

        private bool MatchWasHandledAsNewLine(Match match)
        {
            var result = String.IsNullOrWhiteSpace(match.Value);

            if (result && moveCommandWasRead) {
                switch (movement)
                {
                    case GcodeMovement.Linear:
                    case GcodeMovement.Fast:
                        AddMovementToCommandQueue();
                        break;
                    case GcodeMovement.Circular:
                        throw new NotImplementedException("Cannot handle circular movements");
                    default:
                        throw new NotImplementedException();
                }
            }

            return result;
        }

        private void AddMovementToCommandQueue()
        {
            TranslateTargetVectorIfNeeded();

            commands.Enqueue(new LookAtCommand(targetVector));
            commands.Enqueue(new MoveToCommand(targetVector));

            moveCommandWasRead = false;
            lastVector = targetVector;
            targetVector = new Vector2(0, 0);
        }

        private void TranslateTargetVectorIfNeeded()
        {
            if (absolutePositioning)
            {//use given CenterPoint
                targetVector += CenterPoint;
            }
            else
            { //relative positioning -> change targetPoint vector
                targetVector += lastVector;
            }
        }

        private void HandleParsedCodes(string match)
        {
            Console.WriteLine(match);

            int firstLetter = (char)match[0];
            float number = GetNumber(match);
            switch (firstLetter)
            {
                case (int)'G':
                    if (number == 90)
                    {
                        absolutePositioning = true;
                    }
                    else if (number == 91)
                    {
                        absolutePositioning = false;
                    }
                    else if (number >= 0 && number < 2)
                    { // G0 and G01 
                        //rapid movement or linear movement
                        movement = GcodeMovement.Linear;
                    }
                    else
                    {
                        Console.WriteLine("Unknown G-Command: '" + match + "'");
                    }
                    break;

                case (int)'X':
                    moveCommandWasRead = true;
                    targetVector += new Vector2(GetNumber(match), 0);
                    break;

                case (int)'Y':
                    moveCommandWasRead = true;
                    targetVector += new Vector2(0, GetNumber(match));
                    break;

                case (int)'Z':
                    AddMovePenCommand(GetNumber(match));                   
                    break;

                default:
                    Console.WriteLine("Unknown command: '" + match + "'");
                    break;
            }
        }

        private float GetNumber(string number, int numberOfCharacters = 1)
        {
            string n = number.Substring(numberOfCharacters, number.Length - numberOfCharacters);
            return ParseNumberOrDefault(n, 0);
        }

        private float ParseNumberOrDefault(string number, float defaultValue)
        {
            try
            {
                return Single.Parse(number, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        private void AddMovePenCommand(float newZPosition)
        {
            ACommand penCommand;
            if (PenMovesUpward(newZPosition)) penCommand = new PenUpCommand();
            else penCommand = new PenDownCommand();

            currentZPosition = newZPosition;

            commands.Enqueue(penCommand);
        }

        private bool PenMovesUpward(float newZPosition)
        {
            if (absolutePositioning) 
            {
                return currentZPosition < newZPosition;
            }
            else
            {
                return newZPosition > 0;
            }
        }

    }
}
