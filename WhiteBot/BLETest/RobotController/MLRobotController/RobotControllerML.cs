using WhiteBot.RobotController.MLRobotController;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBot
{
    public class RobotControllerML : AbstractRobotController, IRobotController
    {
        new protected string name = "ML";

        private UInt16 generationsToLearn;
        public int Generation { get { return movementLearner.Generation; } }
        public bool LearnMovement { get; set; }
        private Learner movementLearner;

        private float targetPathLengthSquared = 0;

        public RobotControllerML(Robot robot) : base(robot)
        {
            LearnMovement = false;
            movementLearner = new Learner(robot, new Vector2(80, 60));
        }

        public override void InitializeController()
        {
            base.InitializeController();
        }

        public void LearnGenerations(UInt16 count) 
        {
            generationsToLearn = count;
            LearnMovement = true;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        protected override void OnTick()
        {
            if (LearnMovement)
            {
                LearnHowtoMove();
                return;
            }

            //handle the States Finished, PenUp, PenDown, EraserUp, EraserDown, Idle
            base.OnTick();

            switch (State)
            {
                case RobotState.LookAt:
                case RobotState.LookInDirection:
                case RobotState.RotateTo:
                    HandleRotationCommands();
                    break;
                case RobotState.MoveTo:
                    HandleMoveCommand();
                    break;
            }
        }

        
        [MethodImpl(MethodImplOptions.Synchronized)]
        private void LearnHowtoMove()
        {
            var lastGenerationCount = this.Generation;
            movementLearner.OnTick();
            
            if (this.Generation > lastGenerationCount)
            {
                generationsToLearn--;
                movementLearner.SaveCurrentLearningProcess();
                LearnMovement = generationsToLearn > 0;
            }
        }


        /// <summary>
        /// this is used for debugging, when it reaches 0, a breakpoint may be triggered
        /// </summary>
        int ticker = 10;
        #region Rotation Commands (LookAt, LookInDirection, RotateTo)
        private void HandleRotationCommands()
        {
            var angle = directionalDeviation();

            if (ticker == 0)
            {
                robot.Speed(0, 0);
                ticker = 11;
            }
            ticker--;

            if (Double.IsNaN(angle))
            {//robot orientation is unknown -> stop robot
                robot.Speed(0, 0);
                return;
            }
            var angularSpeed = rotationSpeedFromAngle(-angle); // we have to correct the deviation, so negative angle

            //Console.WriteLine("angle: " + angle + "\tangular: " + angularSpeed);
            robot.Speed(0, angularSpeed);

            if (angularSpeed == 0)
            {
                NextState();
            }
        }

        /// <summary>
        /// What angle needs the robot to turn to reach the target direction
        /// </summary>
        private double directionalDeviation()
        {
            if (targetPoint == startPoint) return 0;

            //var targetDir = targetPoint - robot.Position;
            var targetDir = targetPoint - startPoint;
            targetDir.Normalize();

            //info: http://en.wikipedia.org/wiki/Atan2
            // and: http://msdn.microsoft.com/de-de/library/system.math.atan2(v=vs.110).aspx
            double angle = Math.Atan2(targetDir.Y, targetDir.X) - Math.Atan2(robot.LookDirection.Y, robot.LookDirection.X); ;
            if (angle < -Math.PI)
            {//robot turning is limited to 
                angle += 2 * Math.PI;
            }
            else if (angle > Math.PI)
            {
                angle -= 2 * Math.PI;
            }
            return angle;
        }

        private double rotationSpeedFromAngle(double angle)
        {
            double min = 300; // 160 // cannot move with 60
            double max = 1600; // 1000

            double speed = 0;
            double absAngle = Math.Abs(angle);

            if (absAngle > 2)
            {
                speed = max;
            }
            else if (absAngle < 0.02)
            {
                speed = 0;
            }
            else
            {
                speed = (max - min) * (absAngle - 0.02) / (2 - 0.02) + min;
            }

            return Math.Sign(angle) * speed;
        }
        #endregion


        private void HandleMoveCommand()
        {
            if (distanceToTarget() < 1)
            {
                NextState();
                return;
            }

            //get look direction
            Vector2 direction = targetPoint - startPoint;
            if (this.targetPathLengthSquared == 0)
            {
                this.targetPathLengthSquared = direction.LengthSquared();
            }
            direction.Normalize();

            //find closest match of learned vectors
            var learned = movementLearner.learnedResults.ForNormalizedVector(direction);
            //get best learned motor speed for match
            MotorSpeed speeds = learned.LearnedMotorSpeed;

            if (targetPathLengthSquared <= (robot.Position - startPoint).LengthSquared())
            {
                robot.Speed(0, 0);
                targetPathLengthSquared = 0;
                NextState();
            }
            else
            {
                robot.Speed(speeds.Linear, speeds.Angular);
            }
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

            //return CalculateSignedDistance(targetPoint, targetPoint + orthogonal, robot.Position);
            return (targetPoint - robot.Position).Length();
        }

        public void ImportLearnedData(Learner data)
        {
            this.movementLearner.learnedResults = data.learnedResults;
            this.movementLearner.Generation = data.Generation;
        }

        override public void Stop()
        {
            base.Stop();
            LearnMovement = false;
        }
    }
}
