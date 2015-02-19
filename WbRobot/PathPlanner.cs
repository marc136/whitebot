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
        RobotController controller;

        Queue<ICommand> commands = new Queue<ICommand>();

        public PathPlanner(RobotController controller)
        {
            this.controller = controller;
            controller.Finished += controller_Finished;
        }

        void controller_Finished(object sender, FinishedEventArgs e)
        {
            if(commands.Any())
            {
                var command = commands.Dequeue();
                command.apply(controller);
                Console.WriteLine(command.ToString());
            }
        }

        public void enqueue(ICommand command)
        {
            commands.Enqueue(command);
            if (controller.Idle)
            {
                controller.NextState();
            }
        }

        public void drawLine(Vector2 start, Vector2 end)
        {
            enqueue(new LookAtCommand(start));
            enqueue(new MoveToCommand(start));

            enqueue(new LookAtCommand(end));
            enqueue(new PenDownCommand());
            enqueue(new MoveToCommand(end));
            //enqueue(new StopCommand());
            enqueue(new PenUpCommand());
        }

        public void drawLineSegments(params Vector2[] points)
        {
            for (int i = 0; i < points.Length; i += 2 )
            {
                var begin = points[i];
                var end = points[i + 1];
                enqueue(new LookAtCommand(begin));
                enqueue(new MoveToCommand(begin));
                enqueue(new StopCommand());

                enqueue(new LookAtCommand(end));
                enqueue(new PenDownCommand());
                enqueue(new MoveToCommand(end));
                //enqueue(new StopCommand());
                enqueue(new PenUpCommand());
            }
        }

        public void drawCurvedLineStrip(Vector2 start, params Vector2[] points)
        {
            enqueue(new LookAtCommand(start));
            enqueue(new MoveToCommand(start));

            enqueue(new LookAtCommand(points.First()));
            enqueue(new PenDownCommand());

            foreach(var point in points)
            {
                enqueue(new MoveToCommand(point));
            }
            //enqueue(new StopCommand());
            enqueue(new PenUpCommand());
        }

        public void drawStraightLineStrip(Vector2 start, params Vector2[] points)
        {
            enqueue(new LookAtCommand(start));
            enqueue(new MoveToCommand(start));

            enqueue(new LookAtCommand(points.First()));
            enqueue(new PenDownCommand());

            foreach (var point in points)
            {
                enqueue(new LookAtCommand(point));
                enqueue(new MoveToCommand(point));
            }
            //enqueue(new StopCommand());
            enqueue(new PenUpCommand());
        }

        internal void reset()
        {
            commands.Clear();
            controller.stop();
            
        }
    }
}
