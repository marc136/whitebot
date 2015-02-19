using Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLETest
{
    public class PathPlanner
    {
        IRobotController controller;
        ILogger logger = null;

        Queue<ACommand> commands = new Queue<ACommand>();

        public PathPlanner(IRobotController controller)
        {
            this.controller = controller;
            controller.Finished += controller_Finished;
        }

        public void UseLogger(ILogger logger) {
            this.logger = logger;
            Log("Start session");
        }

        private void Log(string message)
        {
            if (logger != null)
            {
                logger.Log("PathPlanner\t" + message);
            }
        }

        //this is called when a robot controller finishes a task
        void controller_Finished(object sender, FinishedEventArgs e)
        {
            if (logger != null) { Log("Controller: " + e.ToString()); }
            if(commands.Any())
            {
                var command = commands.Dequeue();

                Log("controller startPoint command\t" + command.ToString());
                controller.ReceiveCommand(command);
                Console.WriteLine(command.ToString());
            }
        }

        public void Enqueue(ACommand command)
        {
            commands.Enqueue(command);
            if (controller.Idle)
            {
                controller.NextState();
            }
        }

        public void DrawLine(Vector2 start, Vector2 end)
        {
            Enqueue(new LookAtCommand(start));
            Enqueue(new MoveToCommand(start));

            Enqueue(new LookAtCommand(end));
            Enqueue(new PenDownCommand());
            Enqueue(new MoveToCommand(end));
            //Enqueue(new StopCommand());
            Enqueue(new PenUpCommand());
        }

        public void drawLineSegments(params Vector2[] points)
        {
            for (int i = 0; i < points.Length; i += 2 )
            {
                var begin = points[i];
                var end = points[i + 1];
                Enqueue(new LookAtCommand(begin));
                Enqueue(new MoveToCommand(begin));
                Enqueue(new StopCommand());

                Enqueue(new LookAtCommand(end));
                Enqueue(new PenDownCommand());
                Enqueue(new MoveToCommand(end));
                //Enqueue(new StopCommand());
                Enqueue(new PenUpCommand());
            }
        }

        public void drawCurvedLineStrip(Vector2 start, params Vector2[] points)
        {
            Enqueue(new LookAtCommand(start));
            Enqueue(new MoveToCommand(start));

            Enqueue(new LookAtCommand(points.First()));
            Enqueue(new PenDownCommand());

            foreach(var point in points)
            {
                Enqueue(new MoveToCommand(point));
            }
            //Enqueue(new StopCommand());
            Enqueue(new PenUpCommand());
        }

        public void drawStraightLineStrip(Vector2 start, params Vector2[] points)
        {
            Enqueue(new LookAtCommand(start));
            Enqueue(new MoveToCommand(start));

            Enqueue(new LookAtCommand(points.First()));
            Enqueue(new PenDownCommand());

            foreach (var point in points)
            {
                Enqueue(new LookAtCommand(point));
                Enqueue(new MoveToCommand(point));
            }
            //Enqueue(new StopCommand());
            Enqueue(new PenUpCommand());
        }

        public void DrawMathematicalFunction(Vector2 position, Vector2 scale, int pointCount, Func<double, double> func, double minX, double maxX)
        {
            Vector2[] points = new Vector2[pointCount];
            for (int i = 0; i < points.Length; ++i)
            {
                var x = minX + (maxX - minX) * i / (double)(points.Length - 1);
                points[i] = position + new Vector2(scale.X / points.Length * i, (float)(-scale.Y * func(x)));
            }
            drawCurvedLineStrip(points.First(), points.Skip(1).ToArray());
        }

        public void DrawMathematicalFunctionWithDerivate(Vector2 position, Vector2 scale, int pointCount, Func<double, double> func, Func<double, double> derivative, double minX, double maxX)
        {
            Vector2[] points = new Vector2[pointCount];
            for (int i = 0; i < points.Length; ++i)
            {
                var x = minX + (maxX - minX) * i / (double)(points.Length - 1);

                var angle = (float)derivative(x);

                Vector2 n = new Vector2(-angle, -1.0f);
                n.Normalize();
                n *= new Vector2(scale.Y, (float)(scale.X / (maxX - minX)));
                n.Normalize();
                n *= 20;

                points[i] = position + new Vector2(scale.X / points.Length * i, (float)(-scale.Y * func(x))) + n;
            }
            drawCurvedLineStrip(points.First(), points.Skip(1).ToArray());
        }


        internal void reset()
        {
            commands.Clear();
            controller.Stop();   
        }

        public IRobotController GetController()
        {
            return controller;
        }
    }
}
