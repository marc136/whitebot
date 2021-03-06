﻿using GamePad = Microsoft.Xna.Framework.Input.GamePad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Foundation;
using Timer = System.Timers.Timer;
using ElapsedEventArgs = System.Timers.ElapsedEventArgs;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Logging;

namespace WhiteBot
{
    public partial class Form1 : Form
    {
        RemoteSensors sensors = new RemoteSensors();

        Dictionary<string, ILogger> Loggers = new Dictionary<string, ILogger>();
        
        private ILogger CreateLogger(string title) {
            var logger = new FileLogger(@"logs\" + title + ".log");
            Loggers.Add(title, logger);
            //logger.Log("startPoint");
            return logger;
        }

        Robot[] robots = new Robot[2];
        IRobotController[] robotControllers = new IRobotController[2];
        PathPlanner[] planners = new PathPlanner[2];
        public GamePadController gamePadController;

        private Alignment[] alignment = new Alignment[2];

        bool reversePositions = false;

        public Form1()
        {
            alignment[0] = Alignment.Unknown;
            alignment[1] = Alignment.Unknown;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //moved this stuff to InitializeRobot()
        }

        public void InitializeRobot(RobotController.MLRobotController.Learner learnedData)
        {
            Connect(RobotControllerType.MLController);

            ((RobotControllerML)robotControllers[0]).ImportLearnedData(learnedData);
            AttachLoggers();

            sensors.UpdatePositions += sensorUpdatePosition;
            sensors.UpdateOrientation += sensorUpdateOrientation;
            sensors.UpdateGravity += sensorUpdateGravity;
            sensors.PositionTransform = positionTransform;

            sensors.Start();
        }

        public void InitializeRobot(RobotControllerType type)
        {
            Connect(type);
            AttachLoggers();

            sensors.UpdatePositions += sensorUpdatePosition;
            sensors.UpdateOrientation += sensorUpdateOrientation;
            sensors.UpdateGravity += sensorUpdateGravity;
            sensors.PositionTransform = positionTransform;

            sensors.Start();
        }

        private Vector2 positionTransform(Vector2 p)
        {
            return new Vector2(320, 240) - p;
        }

        private void sensorUpdateGravity(object sender, GravityEventArgs e)
        {
            if (e.Id == 0)
            {
                robot1GravityVis1.Direction(e.Gravity.vec.X, e.Gravity.vec.Y);
                robot1GravityVis2.Direction(e.Gravity.vec.X, e.Gravity.vec.Z);
                robot1GravityVis3.Direction(e.Gravity.vec.Y, e.Gravity.vec.Z);
            }
            else if (e.Id == 1)
            {
                robot2GravityVis1.Direction(e.Gravity.vec.X, e.Gravity.vec.Y);
                robot2GravityVis2.Direction(e.Gravity.vec.X, e.Gravity.vec.Z);
                robot2GravityVis3.Direction(e.Gravity.vec.Y, e.Gravity.vec.Z);
            }
        }

        private void onChangedAlignment(int robotId, Alignment alignment)
        {
            this.alignment[robotId] = alignment;
            pictureBox1.Invalidate();
        }

        private void sensorUpdateOrientation(object sender, OrientationEventArgs e)
        {
            var dir = e.Orientation.getRotationMatrix();
            if(e.Id == 0)
            {
                robot1OrientationVis1.Direction(dir.M11, dir.M12);
                robot1OrientationVis2.Direction(dir.M12, dir.M13);
                robot1OrientationVis3.Direction(dir.M31, dir.M32);
            }
            else if(e.Id == 1)
            {
                robot2OrientationVis1.Direction(dir.M11, dir.M12);
                robot2OrientationVis2.Direction(dir.M12, dir.M13);
                robot2OrientationVis3.Direction(dir.M31, dir.M32);
            }
        }

        private void sensorUpdatePosition(object sender, PositionsEventArgs e)
        {
            int[] ids;

            if(reversePositions)
            {
                positions = e.Positions.Values.ToArray();
                ids = e.Positions.Keys.Reverse().ToArray();
            }
            else
            {
                positions = e.Positions.Values.Reverse().ToArray();
                ids = e.Positions.Keys.ToArray();
            }

            int j = 0;
            foreach(var robot in robots)
            {
                if (j < ids.Length)
                {
                    robot.TrackingId = ids[j];
                    ++j;
                }
                else
                {
                    robot.TrackingId = null;
                }
            }

            pictureBox1.Invalidate();
        }
        
        protected override bool IsInputKey(System.Windows.Forms.Keys keyData)
        {
            if (keyData == System.Windows.Forms.Keys.Right ||keyData == System.Windows.Forms.Keys.Up || keyData == System.Windows.Forms.Keys.Down ||keyData == System.Windows.Forms.Keys.Left)
            {
                return true;
            }
            else
            {
                return base.IsInputKey(keyData);
            }
        }

        public void Connect(RobotControllerType type)
        {
            robots[0] = new Robot("Roboter", 0, sensors);
            robots[0].ChangedAlignment += Robot_ChangedAlignment;
            robots[0].ChangedPosition += Robot_ChangedPosition;
            
            robots[1] = new Robot("Roboter2", 1, sensors);
            robots[1].ChangedAlignment += Robot_ChangedAlignment;
            robots[1].ChangedPosition += Robot_ChangedPosition;


            switch (type)
            {
                case RobotControllerType.MLController:
                    robotControllers[0] = new RobotControllerML(robots[0]);
                    break;
                case RobotControllerType.PidController:
                default:
                    robotControllers[0] = createOldControllerForRobot(0);
                    break;
            }
            planners[0] = new PathPlanner(robotControllers[0]);
            //robotControllers[0].Start();

            robotControllers[1] = createOldControllerForRobot(1);
            planners[1] = new PathPlanner(robotControllers[1]);
            //robotControllers[1].Start();

            gamePadController = new GamePadController(robots);
            gamePadController.ButtonPressed += gamePadController_ButtonPressed;
            gamePadController.Start();
        }

        private IRobotController createOldControllerForRobot(int robotIndex)
        {
            var result = new RobotControllerPID(robots[robotIndex]);
            if (robotIndex == 0)
            {
                kp.Text = result.Kp.ToString();
                ki.Text = result.Ki.ToString();
                kd.Text = result.Kd.ToString();
            }

            return result;
        }

        private void AttachLoggers()
        {
            AttachLoggerToRobot(0);
            AttachLoggerToRobot(1);
        }

        private void AttachLoggerToRobot(int index)
        {
            string robotname = "robot" + index;
            var log1 = CreateLogger(robotname + "_controller");
            robotControllers[index].OnLogController += (string msg) => log1.Log(msg);
            var log2 = CreateLogger(robotname);
            var useEventedLogger = false;
            if (useEventedLogger)
            {
                robotControllers[index].OnLogRobot += (string msg) => log2.Log(msg);
            }
            else
            {
                robotControllers[index].UseTickLogger(log2);
            }
            var log3 = CreateLogger(robotname + "_pathplanner");
            planners[index].UseLogger(log3);
        }

        void gamePadController_ButtonPressed(object sender, ButtonEventArgs e)
        {
            if((e.Button | Buttons.A) != 0)
            {
                robotControllers[0].ReceiveCommand(new LookInDirectionCommand(new Vector2(1, 0)));
            }

            if ((e.Button | Buttons.B) != 0)
            {
                robotControllers[1].ReceiveCommand(new LookInDirectionCommand(new Vector2(-1, 0)));
            }
        }

        void Robot_ChangedPosition(object sender, PositionEventArgs e)
        {
            var robot = (Robot)sender;
            if(robot.Id == 0)
            {
                if(e.KnowsPosition)
                {
                    this.SetTextThreadSafe(robot1PositionX, e.Position.X.ToString());
                    this.SetTextThreadSafe(robot1PositionY, e.Position.Y.ToString());
                }/*
                else
                {
                  this.SetTextThreadSafe(robot1PositionX, "Unknown");
                  this.SetTextThreadSafe(robot1PositionY, "Unknown");
                }/**/
            }
            else if (robot.Id == 1)
            {
                if (e.KnowsPosition)
                {
                    this.SetTextThreadSafe(robot2PositionX, e.Position.X.ToString());
                    this.SetTextThreadSafe(robot2PositionY, e.Position.Y.ToString());
                }/*
                else
                {
                  this.SetTextThreadSafe(robot2PositionX, "Unknown");
                  this.SetTextThreadSafe(robot2PositionY, "Unknown");
                }/**/
            }
        }

        // This delegate enables asynchronous calls for setting 
        // the text property on a Label control. 
        delegate void SetTextCallback(Label destination, string text);
        
        /// <summary>
        /// thread safe changing of text in a label
        /// </summary>
        private void SetTextThreadSafe(Label destination, string value)
        {
            if (destination.IsDisposed) return;

            if (destination.InvokeRequired)
            {
                var s = new SetTextCallback(SetTextThreadSafe);
                try { 
                    if (!destination.IsDisposed)
                    {
                        this.Invoke(s, new object[] { destination, value });
                    }
                }
                catch (ObjectDisposedException)
                {
                }
            }
            else
            {
                destination.Text = value;
            }
        }
        

        void Robot_ChangedAlignment(object sender, AlignmentEventArgs e)
        {
            pictureBox1.Invalidate();
        }

        Vector2[] positions = new Vector2[0];

        Vector2[] targets = new Vector2[2];

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            foreach(var p in positions)
            {
                e.Graphics.FillEllipse(System.Drawing.Brushes.Gray, p.X - 3, p.Y - 3, 7, 7);
            }

            foreach (var robot in robots)
            {
                if(!robot.KnowsPosition)
                {
                    continue;
                }

                e.Graphics.FillEllipse(System.Drawing.Brushes.Blue, robot.Position.X - 3, robot.Position.Y - 3, 7, 7);
                
                if(robot.Alignment != Alignment.Unknown)
                {
                    e.Graphics.DrawLine(Pens.Green, robot.Position.X, robot.Position.Y, robot.Position.X + robot.LookDirection.X * 20, robot.Position.Y + robot.LookDirection.Y * 20);
                    e.Graphics.DrawString((robot.Id + 1).ToString(), new System.Drawing.Font(FontFamily.GenericSerif, 10), Brushes.White, robot.Position.X - robot.LookDirection.X * 15 - 5, robot.Position.Y - robot.LookDirection.Y * 15 - 5);
                }
                else
                {
                    e.Graphics.DrawString((robot.Id + 1).ToString(), new System.Drawing.Font(FontFamily.GenericSerif, 10), Brushes.White, robot.Position.X - 5, robot.Position.Y - 1 * 15 - 5);
                }
            }

            e.Graphics.FillEllipse(System.Drawing.Brushes.Red, targets[0].X - 3, targets[0].Y - 3, 7, 7);
            e.Graphics.FillEllipse(System.Drawing.Brushes.Yellow, targets[1].X - 3, targets[1].Y - 3, 7, 7);

            e.Graphics.DrawString(robots[0].Alignment.ToString(), new System.Drawing.Font(FontFamily.GenericSerif, 10), robots[0].Alignment == Alignment.Unknown ? Brushes.Red : Brushes.Green, 0, 0);
            e.Graphics.DrawString(robots[1].Alignment.ToString(), new System.Drawing.Font(FontFamily.GenericSerif, 10), robots[1].Alignment == Alignment.Unknown ? Brushes.Red : Brushes.Green, 250, 0);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                targets[0] = new Vector2(e.X, e.Y);
                robotControllers[0].ReceiveCommand(new LookAtCommand(targets[0]));
            }
            else if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                targets[1] = new Vector2(e.X, e.Y);
                robotControllers[0].ReceiveCommand(new MoveToCommand(targets[1]));
            }
            
            pictureBox1.Invalidate();
        }

        protected override void OnClosed(EventArgs e)
        {
            planners.ToList().ForEach(planner => planner.Reset());
            robots.ToList().ForEach(robot => robot.Speed(0, 0));
            sensors.Close();
            base.OnClosed(e);
        }

        private void changePositions_Click(object sender, EventArgs e)
        {
            reversePositions = reversePositions ? false : true;
            positions = positions.Reverse().ToArray();
            pictureBox1.Invalidate();
        }

        private void penUp_Click(object sender, EventArgs e)
        {
            robots[0].penUp();
        }

        private void penDown_Click(object sender, EventArgs e)
        {
            robots[0].penDown();
        }

        private void eraserDown_Click(object sender, EventArgs e)
        {
            robots[0].eraserDown();
        }

        private void eraserUp_Click(object sender, EventArgs e)
        {
            robots[0].eraserUp();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            robots[0].eraserRelease();
            robots[0].penRelease();
        }

        private void button2_Click(object sender, EventArgs e)
        {// DrawRectangle
            var p = planners[0];
            p.DrawStraightLineStrip(new Vector2(100, 50), new Vector2(100, 150), new Vector2(200, 150), new Vector2(200, 50), new Vector2(100, 50));
        }

        private void drawSineFunction_Click(object sender, EventArgs e)
        {
            var p = planners[0];
            PathPlannerSamples.DrawSineFunction(p);
        }

        private void stop_Click(object sender, EventArgs e)
        {
            planners.ToList().ForEach(planner => planner.Reset());
            robots.ToList().ForEach(robot => robot.Speed(0, 0));
            robotControllers.ToList().ForEach(controller => controller.Stop());
        }

        #region robotControllerPID
        private void kp_TextChanged(object sender, EventArgs e)
        {
            double value;
            if(double.TryParse(kp.Text, out value))
            {
                foreach (RobotControllerPID controller in robotControllers)
                {
                    if(controller != null)
                    {
                        controller.Kp = value;
                    }
                }
            }
        }

        private void ki_TextChanged(object sender, EventArgs e)
        {
            double value;
            if (double.TryParse(ki.Text, out value))
            {
                foreach (RobotControllerPID controller in robotControllers)
                {
                    if (controller != null)
                    {
                        controller.Ki = value;
                    }
                }
            }
        }

        private void kd_TextChanged(object sender, EventArgs e)
        {
            double value;
            if (double.TryParse(kd.Text, out value))
            {
                foreach (RobotControllerPID controller in robotControllers)
                {
                    if (controller != null)
                    {
                        controller.Kd = value;
                    }
                }
            }
        }
        #endregion

        private void drawAxes_Click(object sender, EventArgs e)
        {
            var p = planners[0];
            p.DrawLineSegments(new Vector2(100, 50), new Vector2(100, 160), new Vector2(80, 130), new Vector2(270, 130));
        }

        private void drawNiceFunction_Click(object sender, EventArgs e)
        {
            var p = planners[0];

            Vector2[] points = new Vector2[15];
            Vector2 scale = new Vector2(160.0f, 80);
            Vector2 position = new Vector2(75, 150);

            //calcFunction(p, position, scale, 15, Math.Sin, Math.Cos, 0, Math.PI);
            p.DrawMathematicalFunctionWithDerivate(position, scale, 15, Math.Sin, Math.Cos, 0, Math.PI);
        }
        //info: moved to PathPlanner.DrawMathematicalFunction()
        private void calcFunction(PathPlanner p, Vector2 position, Vector2 scale, int pointCount, Func<double, double> func, double minX, double maxX)
        {
            Vector2[] points = new Vector2[pointCount];
            for (int i = 0; i < points.Length; ++i)
            {
                var x = minX + (maxX - minX) * i / (double)(points.Length - 1);
                points[i] = position + new Vector2(scale.X / points.Length * i, (float)(-scale.Y * func(x)));
            }
            p.DrawCurvedLineStrip(points.First(), points.Skip(1).ToArray());
        }

        //info: moved to PathPlanner.DrawMathematicalFunctionWithDerivate()
        private void calcFunction(PathPlanner p, Vector2 position, Vector2 scale, int pointCount, Func<double, double> func, Func<double, double> derivative, double minX, double maxX)
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
            p.DrawCurvedLineStrip(points.First(), points.Skip(1).ToArray());
        }

        private void moveForward_Click(object sender, EventArgs e)
        {
            var robot = robots[0];

            robot.Speed(200, 50);
            //robot.LinearSpeed(100);
            Thread.Sleep(600);
            robot.Speed(0, 0);
        }

        private void btnStraightSamples_Click(object sender, EventArgs e)
        {
            PathPlannerSamples.StraightSamples(planners[0]);
        }

        private void btnStartControllers_Click(object sender, EventArgs e)
        {
            robotControllers.ToList().ForEach(controller => controller.Start());
        }

        private void btnParseGcode_Click(object sender, EventArgs e)
        {
            var code = textGcode.Text;
            var centerPoint = new Vector2((float)inputCenterPointX.Value, (float)inputCenterPointY.Value);

            var parser = new GCodeParser.Parser();
            parser.CenterPoint = centerPoint;

            parser.Parse(code);

            foreach (ACommand command in parser.GetCommandQueue)
            {
                planners[0].Enqueue(command);
            }
        }

        /// <summary>
        /// Only works for the Machine Learning Controller
        /// </summary>
        private void btnLearn2Generations_Click(object sender, EventArgs e)
        {
            var controller = robotControllers[0] as RobotControllerML;
            if (controller == null) return;
            controller.LearnGenerations(5);
        }

        private void btnStartBenchmarkPath_Click(object sender, EventArgs e)
        {
            const int index = 0;
            PathPlannerSamples.DrawBenchmark(planners[index], robots[index].Position);
        }


    }
}
