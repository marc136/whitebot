using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WhiteBotLogTool.LogParsing
{
    public class PathTimeInformation
    {
        public const string TimestampFormat = "H:mm:ss:fff";

        public DateTime Start;
        public DateTime Stop;

        public PathTimeInformation(DateTime start) : this (start, start) {}
        public PathTimeInformation(DateTime start, DateTime stop)
        {
            this.Start = start; this.Stop = stop;
        }
        
        public override bool Equals(object obj)
        {
            var otherPathTimeInformation = obj as PathTimeInformation;
            if (otherPathTimeInformation == null)
            {
                return false;
            }
            else
            {
                return this.Start == otherPathTimeInformation.Start && this.Stop == otherPathTimeInformation.Stop;
            }
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() + Stop.GetHashCode();
        }

        public override string ToString()
        {
            return Start.ToString(TimestampFormat) + " - " + Stop.ToString(TimestampFormat);
        }
    }

    class LogParser
    {
        private string logfilePlanner;
        private string logfileController;
        private string logfileRobot;

        public const string TimestampFormat = "H:mm:ss:fff";

        Regex findPathsRegex, findMoveToCommandRegex;

        public ObservableCollection<PathTimeInformation> KnownPaths { get; private set; }
        public PathTimeInformation CurrentPathTimeInformation { get; private set; }
        public ObservableCollection<Point> CurrentPathPoints { get; private set; }
        public ObservableCollection<Point> CurrentDesiredPathPoints { get; private set; }

        public LogParser()
        {
            const string findPathsString = @"^(?<timestamp>\d\d:\d\d:\d\d:\d\d\d)\tPathPlanner\t(?<startstop>Start|Stop) (?<sessionpath>session|path)$";
            findPathsRegex = new Regex(findPathsString, RegexOptions.IgnoreCase & RegexOptions.Compiled & RegexOptions.Multiline);
            const string findMoveToCommandString = @"MoveTo\(x:(?<x>\d+)\Sy:(?<y>\d+)\)$";
            findMoveToCommandRegex = new Regex(findMoveToCommandString, RegexOptions.IgnoreCase & RegexOptions.Compiled & RegexOptions.Multiline);

            KnownPaths = new ObservableCollection<PathTimeInformation>();
            CurrentPathPoints = new ObservableCollection<Point>();
            CurrentDesiredPathPoints = new ObservableCollection<Point>();
        }

        public LogParser(string folderpath, int robotIndex) : this()
        {
            var filenamePrefix = "robot"+ robotIndex;
            var filenameSuffix = ".log";
            var filename = Path.Combine(folderpath, filenamePrefix);

            logfilePlanner = filename + "_pathplanner" + filenameSuffix;
            logfileController = filename + "_controller" + filenameSuffix;
            logfileRobot = filename + filenameSuffix;
        }

        /// <summary>
        /// Parse through the pathplanner logfile line by line and add new Paths to the list of known paths
        /// </summary>
        public void UpdateListOfKnownPaths()
        {
            PathTimeInformation currentPathInformation = null;
            string lastTimestampString = "";

            using (StreamReader r = new StreamReader(logfilePlanner))
            {
                string line;
                
                while ((line = r.ReadLine()) != null)
                {
                    if (line.Length > 12) {
                        lastTimestampString = line.Substring(0,12);
                    }
                    //try to match line to regex
                    Match m = findPathsRegex.Match(line);
                    if (m.Success)
                    {
                        var timestamp = DateTime.ParseExact(m.Groups["timestamp"].Value, TimestampFormat, CultureInfo.InvariantCulture);

                        if (currentPathInformation != null)
                        {//add stop to existing PathInformation
                            currentPathInformation.Stop = timestamp;
                            if (!KnownPaths.Contains(currentPathInformation)) KnownPaths.Add(currentPathInformation);
                            currentPathInformation = null;
                        }

                        if (m.Groups["startstop"].Value.ToLower() == "start")
                        {
                            currentPathInformation = new PathTimeInformation(timestamp);
                        }
                        
                        //group2: session|path
                    }
                }

                //file has ended, add last pathInformation
                if (currentPathInformation != null)
                {
                    var timestamp = DateTime.ParseExact(lastTimestampString, TimestampFormat, CultureInfo.InvariantCulture);
                    currentPathInformation.Stop = timestamp;
                    //Maybe data was appended to the log file, so append the last Path in any case
                    if (KnownPaths.Contains(currentPathInformation)) KnownPaths.Remove(currentPathInformation);
                    
                    KnownPaths.Add(currentPathInformation);
                }
            }
            
        }


        public void ParsePath(PathTimeInformation selectedPathTimeInformation)
        {
            CurrentPathTimeInformation = selectedPathTimeInformation;
            CurrentPathPoints.Clear();
            
            //iterate through logfile and stop on line with >= selected.Start
            using (StreamReader r = new StreamReader(logfileRobot))
            {
                string line;
                double pX, pY; //coordinates for point
                var beginningWasFound = false;
                var endWasFound = false;
            

                while ((line = r.ReadLine()) != null)
                {
                    if (line.Length < 12) continue;

                    var timestamp = DateTime.ParseExact(line.Substring(0,12), TimestampFormat, CultureInfo.InvariantCulture);

                    if (!beginningWasFound)
                    {
                        if (timestamp >= selectedPathTimeInformation.Start)
                        {//start saving points
                            beginningWasFound = true;
                        }
                        else {//still before start time, skip this line
                            continue;
                        }
                    }
                    else
                    {
                        if (endWasFound) break;// last line was the last in path, stop parsing the file
                        endWasFound = (timestamp >= selectedPathTimeInformation.Stop);
                    }

                    var informations = line.Split('\t');
                    //Information contained in this array:
                    //[Timestamp, Position.X, Position.Y, LookDirection.X, LookDirection.Y, Gravity.X, Gravity.Y, Gravity.Z, DirectionVector.X, DirectionVector.Y]
                    
                    if (Double.TryParse(informations[1], out pX) && Double.TryParse(informations[2], out pY))
                    {//if numbers were successfully parsed, add point to list
                        CurrentPathPoints.Add(new Point(pX, pY)); 
                    }
                }               
            }
        }

        public void ParseDesiredPath(PathTimeInformation selectedPathTimeInformation)
        {
            //retreive start point from list of points if possible
            if (CurrentPathPoints.Count > 0) CurrentDesiredPathPoints.Add(CurrentPathPoints[0]);

            //parse through controller logfile

            //iterate through logfile and stop on line with >= selected.Start
            using (StreamReader r = new StreamReader(logfilePlanner))
            {
                string line;
                int pX, pY; //coordinates for point
                var beginningWasFound = false;
                var endWasFound = false;
            

                while ((line = r.ReadLine()) != null)
                {
                    if (line.Length < 12) continue;

                    var timestamp = DateTime.ParseExact(line.Substring(0,12), TimestampFormat, CultureInfo.InvariantCulture);

                    if (!beginningWasFound)
                    {
                        if (timestamp >= selectedPathTimeInformation.Start)
                        {//start saving points
                            beginningWasFound = true;
                        }
                        else {//still before start time, skip this line
                            continue;
                        }
                    }
                    else
                    {
                        if (endWasFound) break;// last line was the last in path, stop parsing the file
                        endWasFound = (timestamp >= selectedPathTimeInformation.Stop);
                    }

                    //try to match MoveTo Command
                    Match m = findMoveToCommandRegex.Match(line);
                    if (m.Success)
                    {
                        if (Int32.TryParse(m.Groups["x"].Value, out pX) && Int32.TryParse(m.Groups["y"].Value, out pY))
                        {//if numbers were successfully parsed, add point to list
                            CurrentDesiredPathPoints.Add(new Point(pX, pY));
                        }
                    }
                }
            }
        }
    }
}
