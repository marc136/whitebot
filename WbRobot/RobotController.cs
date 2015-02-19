using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BLETest
{
    public class FinishedEventArgs : EventArgs
    {
        public FinishedEventArgs()
        {

        }
    }

    public class RobotController
    {
        Timer timer;
        Robot robot;
        Vector2 target;
        Vector2 start;

        enum State
        {
            RotateTo,
            MoveTo,
            Finished,
            Stop,
            PenUp,
            PenDown,
            EraserUp,
            EraserDown,
            Idle
        }

        State state = State.Idle;

        public RobotController(Robot robot)
        {
            this.robot = robot;
            timer = new System.Timers.Timer(50);
            timer.Elapsed += timer_Elapsed;
            timer.AutoReset = true;

            pid.Kp = 0.1;
            pid.Kd = 0.3;
            pid.Ki = 0.01;
        }

        PidController pid = new PidController();

        int waitCount = 0;

        public double Kp
        {
            set
            {
                pid.Kp = value;
            }
            get
            {
                return pid.Kp;
            }
        }
        public double Ki
        {
            set
            {
                pid.Ki = value;
            }
            get
            {
                return pid.Ki;
            }
        }
        public double Kd
        {
            set
            {
                pid.Kd = value;
            }
            get
            {
                return pid.Kd;
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(!robot.KnowsPosition && state != State.Idle)
            {
                state = State.Stop;
                return;
            }
            
            if (robot.Alignment == Alignment.Unknown && state != State.Idle)
            {
                state = State.Stop;
                return;
            }

            double angularSpeed;
            double linearSpeed;
            double angle;
            switch(state)
            {
                case State.Stop:
                    speed(0, 0);
                    NextState();
                    break;
                case State.Finished:
                    speed(0, 0);
                    state = State.Idle;
                    break;
                case State.Idle:
                    break;
                case State.PenUp:
                    if(waitCount == 0)
                    {
                        robot.penUp();
                    }
                    ++waitCount;
                    if(waitCount == 4)
                    {
                        waitCount = 0;
                        NextState();
                    }
                    break;
                case State.PenDown:
                    if(waitCount == 0)
                    {
                        robot.penDown();
                    }
                    ++waitCount;
                    if(waitCount == 4)
                    {
                        waitCount = 0;
                        NextState();
                    }
                    break;
                case State.EraserUp:
                    if(waitCount == 0)
                    {
                        robot.eraserUp();
                    }
                    ++waitCount;
                    if(waitCount == 4)
                    {
                        waitCount = 0;
                        NextState();
                    }
                    break;
                case State.EraserDown:
                    if(waitCount == 0)
                    {
                        robot.eraserDown();
                    }
                    ++waitCount;
                    if(waitCount == 4)
                    {
                        waitCount = 0;
                        NextState();
                    }
                    break;
                case State.RotateTo:
                    angle = directionalDeviation();
                    angularSpeed = rotationSpeedFromAngle(-angle); // we have to correct the deviation, so negative angle
                    
                    //Console.WriteLine("angle: " + angle + "\tangular: " + angularSpeed);
                    speed(0, angularSpeed);

                    if (angularSpeed == 0)
                    {
                        NextState();
                    }
                    
                    break;
                case State.MoveTo:
                    angle = directionalDeviation();
                    
                    //angularSpeed = 0;
                    var distance = distanceToTarget();
                    //Console.WriteLine("Distance: " + distance);
                    /*
                     * non PID controller
                    angularSpeed = rotationSpeedFromAngle(-angle); // we have to correct the deviation, so negative angle
                    linearSpeed = linearSpeedFromDistance(distance);
                    //linearSpeed = 0;
                    angularSpeed = angularSpeed - compensatePositionError();*/

                    linearSpeed = linearSpeedFromDistance(distance);
                    double pathDistance = getDistanceToPath();
                    var pidResult = pid.update(-angle - pathDistance / 20);
                    //Console.WriteLine("Angle" + angle + "\tPathDistance" + pathDistance + "\tPID: " + pidResult);
                    angularSpeed = rotationSpeedFromAngle(pidResult);

                    if(linearSpeed != 0)
                    {
                        //Console.WriteLine("Linear: " + linearSpeed + "\tangular: " + angularSpeed);
                        speed(linearSpeed, angularSpeed);
                    }
                    else
                    {
                        //state = State.RotateTo;
                        speed(0, 0);
                        NextState();
                    }
                    break;
            }
        }

        public void NextState()
        {
            state = State.Finished;
            OnFinished(new FinishedEventArgs());
        }

        void OnFinished(FinishedEventArgs e)
        {
            if(Finished != null)
            {
                Finished(this, e);
            }
        }

        public event EventHandler<FinishedEventArgs> Finished;

        const double p = 0.1;

        private double compensatePositionError()
        {
            double distanceToPath = getDistanceToPath();
            return rotationSpeedFromAngle(distanceToPath * p);
        }

        private double getDistanceToPath()
        {
            var d = target - start;

            return signedDistance(start, target, robot.Position);
        }

        // signed distance
        // + -> right off path (y smaller)
        // - -> left off path (y greater)
        private double signedDistance(Vector2 lineBegin, Vector2 lineEnd, Vector2 point)
        {
            var d = lineEnd - lineBegin;

            var distance = (d.Y * point.X - d.X * point.Y - lineBegin.X * lineEnd.Y + lineEnd.X * lineBegin.Y) / d.Length();

            return distance;
        }

        private double linearSpeedFromDistance(double distance)
        {
            double min = 100;
            double max = 200;

            if(distance > 100)
            {
                return max;
            }

            if (distance < 2)
            {
                return 0;
            }

            return (max - min) * (distance - 2) / (100 - 2) + min;
        }

        double distanceToTarget()
        {
            var dir = target - start;

            var orthogonal = new Vector2(dir.Y, -dir.X);

            return signedDistance(target, target + orthogonal, robot.Position);
            //return (target - robot.Position).Length();
        }

        double directionalDeviation()
        {
            //var targetDir = target - robot.Position;
            var targetDir = target - start;
            targetDir.Normalize();

            double angle = Math.Atan2(targetDir.Y, targetDir.X) - Math.Atan2(robot.LookDirection.Y, robot.LookDirection.X); ;
            if (angle < -Math.PI)
            {
                angle += 2 * Math.PI;
            }
            else if (angle > Math.PI)
            {
                angle -= 2 * Math.PI;
            }
            return angle;
        }

        double rotationSpeedFromAngle(double angle)
        {
            double min = 500; // 160
            double max = 1600; // 1000

            double speed = 0;
            double absAngle = Math.Abs(angle);

            if (absAngle > 2)
            {
                speed = max;
            }
            else if(absAngle < 0.02)
            {
                speed = 0;
            }
            else
            {
                speed = (max - min) * (absAngle - 0.02) / (2 - 0.02) + min;
            }
             

            return Math.Sign(angle) * speed;
        }

        public void Start()
        {
            timer.Start();
        }

        public void Halt()
        {
            timer.Stop();
        }

        public void stop()
        {
            this.state = State.Stop;
        }

        public void lookAt(Vector2 target)
        {
            this.target = target;
            //Console.WriteLine("angle: " + directionalDeviation());
            this.start = robot.Position;
            state = State.RotateTo;
        }

        public void lookInDirection(Vector2 direction)
        {
            this.start = Vector2.Zero;
            this.target = direction;
            state = State.RotateTo;
        }

        public void moveTo(Vector2 target)
        {
            this.target = target;
            this.start = robot.Position;
            state = State.MoveTo;
        }

        public void speed(double linear, double angular)
        {
            robot.speed(linear, angular);
        }

        public void penUp()
        {
            state = State.PenUp;
        }

        public void penDown()
        {
            state = State.PenDown;
        }

        public void eraserUp()
        {
            state = State.EraserUp;
        }

        public void eraserDown()
        {
            state = State.EraserDown;
        }

        public bool Idle { get { return state == State.Idle; } }
    }
}
