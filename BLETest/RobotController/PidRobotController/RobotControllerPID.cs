using Logging;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace BLETest
{
    public class RobotControllerPID : AbstractRobotController, IRobotController
    {
        new protected string name = "PID";

        public RobotControllerPID(Robot robot): base(robot)
        {
            robot.Log += LogRobot;
            
            pid.Kp = 0.1;
            pid.Kd = 0.3;
            pid.Ki = 0.01;
        }

        #region PidController
        PidController pid = new PidController();

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
        #endregion

        protected override void OnTick()
        {
            //handle the States Finished, PenUp, PenDown, EraserUp, EraserDown, Idle
            base.OnTick();

            double angularSpeed, linearSpeed, angle;

            switch(State)
            {
                case RobotState.RotateTo:
                case RobotState.LookAt:
                case RobotState.LookInDirection:
                    angle = directionalDeviation();
                    angularSpeed = rotationSpeedFromAngle(-angle); // we have to correct the deviation, so negative angle
                    
                    //Console.WriteLine("angle: " + angle + "\tangular: " + angularSpeed);
                    LogController(String.Format("Rotate(angle: {0} | angular: {1})", angle, angularSpeed));
                    Speed(0, angularSpeed);

                    if (angularSpeed == 0)
                    {
                        NextState();
                    }
                    
                    break;
                case RobotState.MoveTo:
                    angle = directionalDeviation();
                    
                    //angularSpeed = 0;
                    var distance = distanceToTarget();
                    //Console.WriteLine("Distance: " + distance);
                    /*
                     * non PID controller
                    angularSpeed = rotationSpeedFromAngle(-angle); // we have to correct the deviation, so negative angle
                    LinearSpeed = linearSpeedFromDistance(distance);
                    //LinearSpeed = 0;
                    angularSpeed = angularSpeed - compensatePositionError();*/

                    linearSpeed = linearSpeedFromDistance(distance);
                    double pathDistance = getDistanceToPath();
                    var pidResult = pid.update(-angle - pathDistance / 20);
                    //Console.WriteLine("Angle" + angle + "\tPathDistance" + pathDistance + "\tPID: " + pidResult);
                    angularSpeed = rotationSpeedFromAngle(pidResult);

                    LogController(String.Format("MoveTo(Linear: {0} | angular: {1})", linearSpeed, angularSpeed));
                    if(linearSpeed != 0)
                    {
                        //Console.WriteLine("Linear: " + LinearSpeed + "\tangular: " + angularSpeed);
                        Speed(linearSpeed, angularSpeed);
                    }
                    else
                    {
                        //state = RobotState.RotateTo;
                        Speed(0, 0);
                        NextState();
                    }
                    break;
            }
        }

        const double p = 0.1;

        private double compensatePositionError()
        {
            double distanceToPath = getDistanceToPath();
            return rotationSpeedFromAngle(distanceToPath * p);
        }

        private double getDistanceToPath()
        {
            var d = targetPoint - startPoint;

            return RobotController.HelperFunctions.CalculateSignedDistance(startPoint, targetPoint, robot.Position);
        }

        

        private double linearSpeedFromDistance(double distance)
        {
            double min = 100;
            double max = 200;

            if (distance > 100)
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
            var dir = targetPoint - startPoint;

            var orthogonal = new Vector2(dir.Y, -dir.X);

            return RobotController.HelperFunctions.CalculateSignedDistance(targetPoint, targetPoint + orthogonal, robot.Position);
            //return (targetPoint - robot.Position).Length();
        }

        double directionalDeviation()
        {
            //var targetDir = targetPoint - robot.Position;
            var targetDir = targetPoint - startPoint;
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

    }
}
