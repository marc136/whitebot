using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BLETest.RobotController.MLRobotController
{
    struct MotorSpeed 
    {
        public int Linear;
        public int Angular;

        public MotorSpeed(int linear, int angular)
        {
            Linear = linear; Angular = angular;
        }
    }

    class LearnSpecificAngle
	{
        public LearnSpecificAngle()
        {
            SuccessfullyLearned = false;
        }
        /// <summary>
        /// this list is sorted ascending by the distance from the target
        /// </summary>
        List<LearnAngleResult> triedConfigurations = new List<LearnAngleResult>();
        public int NumberOfLearnTries { get { return triedConfigurations.Count; } }

        public MotorSpeed CurrentConfiguration { get; private set; }
        public void SetCurrentDeviation(double deviation)
        {
            var currentResult = new LearnAngleResult(CurrentConfiguration, deviation);
            triedConfigurations.Add(currentResult);
            triedConfigurations = triedConfigurations.OrderBy(element => Math.Abs(element.Deviation)).ToList();

            if (Math.Abs(deviation) < 1) SuccessfullyLearned = true;
        }

        public bool SuccessfullyLearned { get; private set; }
        /// <summary>
        /// returns the best MotorSpeed configuration (so far)
        /// </summary>
        public MotorSpeed LearnedMotorSpeed { get { return triedConfigurations[0].MotorSpeed; } }

        public MotorSpeed CreateNextTry() {
            if (SuccessfullyLearned)
            {
                return LearnedMotorSpeed;
            }

            var result = new MotorSpeed(100, 0);

            switch (NumberOfLearnTries)
            {
                case 0:
                    //try without angular Speed -> this might be already the best solution
                    break;
                case 1:
                    result.Angular = 100; //800;
                    break;
                case 2:
                    result.Angular = -100; //-800;
                    break;
                default:
                    result.Angular = CalculateAngularSpeedFromBestTwoTries();
                    break;
            }
            
            return result;
        }

        private int CalculateAngularSpeedFromBestTwoTries()
        {
            //TODO add some random-ness to escape local minima?
            return (int)(triedConfigurations[0].AngularSpeed + triedConfigurations[1].AngularSpeed) / 2;
        }
	}


    class LearnAngleResult
    {
        public int LinearSpeed;
        public int AngularSpeed;
        public double Deviation;

        public LearnAngleResult(MotorSpeed speed, double deviation) : this(speed.Linear, speed.Angular, deviation) {}

        public LearnAngleResult(int linearSpeed, int angularSpeed, double deviation)
        {
            LinearSpeed = linearSpeed;
            AngularSpeed = angularSpeed;
            Deviation = deviation;
        }

        public MotorSpeed MotorSpeed { get { return new MotorSpeed(LinearSpeed, AngularSpeed); } }
    }

    class Learner
    {
        private const int LearningVectorLength = 30;
        
        private Robot robot;

        private Rectangle centerBox;
        private Vector2 centerPoint;

        private Vector2 startPoint;
        private Vector2 targetPoint;

        Dictionary<Vector2, LearnSpecificAngle> learnedAngles = new Dictionary<Vector2, LearnSpecificAngle>();
        private LearnSpecificAngle currentSpecificLearningAngle;

        private RobotState currentState;
        private int numberOfTicksWithCurrentState = 0;

        /// <summary>
        /// A command queue is used to navigate in a given direction and then move back to the center box
        /// if the queue is empty, another angle is learned
        /// </summary>
        private Queue<ACommand> commandQueue = new Queue<ACommand>();

        public bool AllAnglesLearned { get; private set; }

        public Learner(Robot robot, Vector2 centerPoint)
        {
            this.robot = robot;
            const int radius = 15;
            this.currentState = RobotState.Idle;

            this.AllAnglesLearned = false;
            this.centerPoint = centerPoint;
            centerBox = new Rectangle((int)centerPoint.X - radius, (int)centerPoint.Y - radius, 2 * radius, 2 * radius);
            InitializeLearningAngles();
        }

        private void InitializeLearningAngles()
        {
            for (int angle = 0; angle < 360; angle += 5)
            {
                var vector = AngleToVector(angle);
                learnedAngles.Add(vector, new LearnSpecificAngle());
            }
        }

        private Vector2 AngleToVector(float angle)
        {
            var vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            vector.Normalize(); //is this needed?
            return vector;
        }

        private void NextState(bool reachedTargetDestination = false)
        {
            if (reachedTargetDestination && currentSpecificLearningAngle != null)
            {
                var distanceFromTarget = RobotController.HelperFunctions.CalculateSignedDistance(startPoint, targetPoint, robot.Position);
                currentSpecificLearningAngle.SetCurrentDeviation(distanceFromTarget);
            }

            robot.Speed(0, 0);

            currentSpecificLearningAngle = null;
            numberOfTicksWithCurrentState = 0;
            currentState = RobotState.Finished;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void OnTick()
        {
            //weiter bewegen?
            switch (currentState)
            {
                case RobotState.RotateTo:
                case RobotState.LookAt:
                case RobotState.LookInDirection:
                    HandleRotationCommands();
                    break;

                case RobotState.MoveTo:
                    HandleMoveCommand();
                    break;

                case RobotState.Finished:
                case RobotState.Idle:
                    if (commandQueue.Count > 0)
                    {
                        StartNextCommandInQueue();
                    }
                    else
                    {//learn next angle or move to center box
                        StartNewLearningQueue();
                    }
                    break;
                default:
                    break;
            }
        }

        #region Rotation Commands (LookAt, LookInDirection, RotateTo)
        private void HandleRotationCommands()
        {
            var angle = directionalDeviation();
            if (Double.IsNaN(angle))
            {//robot orientation is unknown -> stop robot
                robot.Speed(0, 0);
                return;
            }
            var angularSpeed = rotationSpeedFromAngle(-angle); // we have to correct the deviation, so negative angle

            Console.WriteLine("angle: " + angle + "\tangular: " + angularSpeed);
            robot.Speed(0, angularSpeed);

            if (angularSpeed == 0)
            {
                NextState();
            }
            /**/
        }

        /// <summary>
        /// What angle needs the robot to turn to reach the target direction
        /// </summary>
        private double directionalDeviation()
        {
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
        #endregion

        #region linear and angular Speed
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

        private double rotationSpeedFromAngle(double angle)
        {
            double min = 500; // 160
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
            //if (target || length of desired vector) was reached
            if (robot.KnowsPosition)
            {
                var movedDistance = Math.Abs((robot.Position - startPoint).Length());

                if (movedDistance >= LearningVectorLength)
                {
                    NextState(reachedTargetDestination:true);
                }
                else
                {
                    //continue to move
                }
            }

            if (numberOfTicksWithCurrentState > 100)
            {//catch if the angular Speed was too high and the robot is only turning in circles
                NextState(reachedTargetDestination:true);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void StartNextCommandInQueue()
        {
            var command = commandQueue.Dequeue() as DirectionalCommand;

            if (command == null)
            {
                currentState = RobotState.Idle;
                return;
            }

            currentState = command.CommandType;

            this.targetPoint = startPoint + (LearningVectorLength * command.Vector);
            switch (command.CommandType)
            {
                case RobotState.LookAt:
                    //Console.WriteLine("angle: " + directionalDeviation());
                    this.startPoint = robot.Position;
                    break;
                case RobotState.LookInDirection:
                    this.startPoint = Vector2.Zero;
                    break;
                case RobotState.MoveTo:
                    this.startPoint = robot.Position;
                    break;
                default:
                    throw new ArgumentException("Command " + command.ToString() + " is not a supported directional command");
            }
        }

        /// <summary>
        /// Learn an angle or move to the center box
        /// </summary>
        public void StartNewLearningQueue()
        {
            var startPoint = robot.Position;

            //(1)pick an angle
            //enqueue commands: RotateTo(Angle), Move(Angle), rotateTo(AngleInverted), move(AngleInverted)
            //when queue is empty, check if robot is in centerbox
            //  if not, move to centerbox
            //then start again (1)

            if (PositionInCenterBox(robot.Position))
            {//learn a new vector
                var angle = SelectNewAngleToLearn();

                //enqueue commands: RotateTo(Angle), Move(Angle), rotateTo(AngleInverted), move(AngleInverted)
                commandQueue.Enqueue(new LookInDirectionCommand(angle));
                commandQueue.Enqueue(new MoveToCommand(angle));

                angle = angle * -1;
                commandQueue.Enqueue(new LookInDirectionCommand(angle));
                commandQueue.Enqueue(new MoveToCommand(angle));
                //debug: look at command Queue
            }
            else
            {//moveToCenterBox
                commandQueue.Enqueue(new LookAtCommand(centerPoint));
                commandQueue.Enqueue(new MoveToCommand(centerPoint));
            }
        }

        /// <summary>
        /// The center box is set in the constructor
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private bool PositionInCenterBox(Vector2 point)
        {
            return centerBox.Contains((int)point.X, (int)point.Y);
        }

        private Vector2 SelectNewAngleToLearn()
        {
            //initial debugging
            return new Vector2(1, 0);

            //select a new angle
            var useAngle = learnedAngles.First();
            var min = useAngle.Value.NumberOfLearnTries;
            if (min > 0)
            {
                var nextAngle = learnedAngles.FirstOrDefault(item => {
                        return item.Value.NumberOfLearnTries < min && !item.Value.SuccessfullyLearned;
                    });
                if (nextAngle.Value != null)
                {
                    useAngle = nextAngle;
                }
            }

            return useAngle.Key;
        }
    }
}
