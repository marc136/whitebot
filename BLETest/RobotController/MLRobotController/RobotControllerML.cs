using BLETest.RobotController.MLRobotController;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLETest
{
    class RobotControllerML : AbstractRobotController, IRobotController
    {
        private bool movingWasLearned = false;
        private Learner movementLearner;

        public RobotControllerML(Robot robot) : base(robot)
        {
            movementLearner = new Learner(robot, new Vector2(200, 60));
        }

        public override void InitializeController()
        {
            base.InitializeController();
        }

        protected override void OnTick()
        {
            if (!movingWasLearned)
            {
                LearnHowtoMove();
                return;
            }

            //handle the States Finished, PenUp, PenDown, EraserUp, EraserDown, Idle
            base.OnTick();

            double angularSpeed, linearSpeed, angle;

            switch (State)
            {
                case RobotState.LookAt:
                    break;
                case RobotState.LookInDirection:
                    break;
                case RobotState.RotateTo:
                    break;
                case RobotState.MoveTo:
                    break;
            }
        }

        private void LearnHowtoMove()
        {
            movementLearner.OnTick();
            movingWasLearned = movementLearner.AllAnglesLearned;
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
    }
}
