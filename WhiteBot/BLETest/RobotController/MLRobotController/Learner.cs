using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBot.RobotController.MLRobotController
{
    public struct MotorSpeed 
    {
        public int Linear;
        public int Angular;

        public MotorSpeed(int linear, int angular)
        {
            Linear = linear; Angular = angular;
        }
    }
    
    enum LearningState
    {
        LookToCenterBox, MoveToCenterBox, LookInFirstDirection, MoveInFirstDirection, LookInSecondDirection, MoveInSecondDirection,
        StartAgain
    }

    
    public class Learner
    {
        private const int LearningVectorLength = 50;
        
        private Robot robot;

        private Rectangle centerBox;
        private Vector2 centerPoint;

        private Vector2 startPoint;
        private Vector2 targetPoint;
        private Vector2 currentAngle;

        public LearningResult learnedResults;
        private LearnSpecificAngle currentSpecificLearningAngle;

        private RobotState currentState;
        private LearningState currentLearningState = LearningState.StartAgain;
        private int numberOfTicksWithCurrentState = 0;


        /// <summary>
        /// A command queue is used to navigate in a given direction and then move back to the center box
        /// if the queue is empty, another angle is learned
        /// </summary>
        private Queue<ACommand> commandQueue = new Queue<ACommand>();

        public bool AllAnglesLearned { get; private set; }

        public int Generation { get; set; }

        public Learner(Robot robot, Vector2 centerPoint) : this(robot, centerPoint, null) { }

        public Learner (Robot robot, Vector2 centerPoint, LearningResult learnedData)
        {
            const int radius = 15;

            this.currentState = RobotState.Idle;
            this.AllAnglesLearned = false;

            this.robot = robot;
            this.centerPoint = centerPoint;
            centerBox = new Rectangle((int)centerPoint.X - radius, (int)centerPoint.Y - radius, 3 * radius, 2 * radius);

            if (learnedData == null)
            {
                InitializeLearningAngles();
                Generation = 0;
            }
            else
            {
                learnedResults = learnedData;
                Generation = learnedData.GetLowestNumberOfTries();
            }
        }

        private void InitializeLearningAngles()
        {
            this.learnedResults = new LearningResult();
            /*
            for (int angle = 0; angle < 360; angle += 90)
            {
                var vector = AngleToVector(angle);
                learnedResults.AddNewLearningVector(vector);
            }
            /**/
            learnedResults.AddNewLearningVector(new Vector2(1, 0));
            /**/
            learnedResults.AddNewLearningVector(new Vector2(0, 1));
            learnedResults.AddNewLearningVector(new Vector2(-1, 0));
            learnedResults.AddNewLearningVector(new Vector2(0, -1));
            /**/
        }

        private Vector2 AngleToVector(float angle)
        {
            var vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            /*var len = vector.Length();
            if (len > 1 + float.Epsilon || len < 1 - float.Epsilon) */
            vector.Normalize(); //normalization is unfortunately needed
            
            return vector;
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
                    StartNextCommandInQueue();
                    break;
                default:
                    break;
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
                FinishState();
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

        private double rotationSpeedFromAngle(double angle)
        {
            double min = 300; // 160
            double max = 800; // 1000

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
                if (currentLearningState == LearningState.MoveToCenterBox)
                {
                    if (PositionInCenterBox(robot.Position))
                    {
                        FinishState();
                    }
                    else
                    {
                        robot.Speed(300, 5);
                        var i = 1;
                    }
                    return;
                }

                var movedDistance = Math.Abs((robot.Position - startPoint).Length());

                if (movedDistance >= LearningVectorLength)
                {
                    FinishState(reachedTargetDestination:true);
                }
                else
                {
                    var speed = currentSpecificLearningAngle.CurrentMotorSpeed;
                    robot.Speed(speed.Linear, speed.Angular);
                }
            }
            else
            {
                robot.Speed(0, 0);
            }

            if (numberOfTicksWithCurrentState > 100)
            {//catch if the angular Speed was too high and the robot is only turning in circles
                FinishState(reachedTargetDestination:true);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void FinishState(bool reachedTargetDestination = false)
        {
            robot.Speed(0, 0);

            if (reachedTargetDestination && currentSpecificLearningAngle != null)
            {//robot has moved to the desired destination
                var distanceFromTarget = RobotController.HelperFunctions.CalculateSignedDistance(startPoint, targetPoint, robot.Position);
                currentSpecificLearningAngle.SetCurrentDeviation(distanceFromTarget);
                currentSpecificLearningAngle = null;
            }

            var newGeneration = learnedResults.GetLowestNumberOfTries();
            if (Generation < newGeneration)
            {
                Generation = newGeneration;
                SaveCurrentLearningProcess();
            }

            currentState = RobotState.Finished;
            currentLearningState++;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void StartNextCommandInQueue()
        {
            ticker = 10;
            numberOfTicksWithCurrentState = 0;

            if (currentLearningState >= LearningState.StartAgain)
            {
                currentLearningState = LearningState.LookToCenterBox;
            }

            switch (currentLearningState)
            {
                case LearningState.LookToCenterBox:
                    if (PositionInCenterBox(robot.Position))
                    {//don't need to move to the center box, skip to look in the first direction (on next tick)
                        currentLearningState = LearningState.LookInFirstDirection;
                    }
                    else
                    {
                        currentState = RobotState.LookAt;
                        this.startPoint = robot.Position;
                        this.targetPoint = centerPoint;
                    }
                    break;

                case LearningState.MoveToCenterBox:
                    currentState = RobotState.MoveTo;
                    this.startPoint = robot.Position;
                    this.targetPoint = centerPoint;
                    break;

                case LearningState.LookInFirstDirection:
                    SelectNewAngleToLearn();
                    currentState = RobotState.LookInDirection;
                    this.startPoint = Vector2.Zero;
                    this.targetPoint = currentAngle;
                    break;

                case LearningState.MoveInFirstDirection:
                case LearningState.MoveInSecondDirection:
                    currentState = RobotState.MoveTo;
                    this.startPoint = robot.Position;
                    this.targetPoint = this.startPoint + currentAngle * LearningVectorLength;
                    break;

                case LearningState.LookInSecondDirection:
                    var invertedAngle = -currentAngle;

                    //debug: this optimizes the number of movements (learn another vector when trying to move back to center box)
                    //this is currently deactivated
                    currentLearningState = LearningState.StartAgain;
                    currentState = RobotState.Finished;
                    if (true) return;
                    //end debug

                    LearnSpecificAngle invertedSpecific = learnedResults.ForNormalizedVector(invertedAngle);
                    if (invertedSpecific != null)
                    {
                        currentLearningState = LearningState.StartAgain;
                        currentState = RobotState.Finished;
                    }
                    else
                    {
                        currentAngle = invertedAngle;
                        currentSpecificLearningAngle = invertedSpecific;

                        currentState = RobotState.LookInDirection;
                        this.startPoint = Vector2.Zero;
                        this.targetPoint = currentAngle;
                    }
                    break;

                default:
                    currentLearningState = LearningState.StartAgain;
                    currentState = RobotState.Finished;
                    break;
            }
        }

        /// <summary>
        /// <para>
        /// The center box is set in the constructor
        /// </para>
        /// <para>
        /// todo: pick another random vector from learnedAngles to return to the center box
        /// </para>
        /// </summary>
        private bool PositionInCenterBox(Vector2 point)
        {
            return centerBox.Contains((int)point.X, (int)point.Y);
        }

        /// <summary>
        /// sets currentAngle and currentSpecificLearningAngle
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SelectNewAngleToLearn()
        {
            //select a new angle
            currentAngle = learnedResults.GetNotLearnedAngleWithLowestNumberOfTries();
            
            currentSpecificLearningAngle = learnedResults.ForNormalizedVector(currentAngle);

            currentSpecificLearningAngle.CreateNextTry();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SaveCurrentLearningProcess()
        {
            const string folder = "learned";
            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }
            var filename = DateTime.Now.ToString("yy-MM-dd") + "_after-generation-" + Generation + ".json";
            this.SaveToFile(folder + @"\" + filename);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SaveToFile(string filename)
        {
            try
            {
                Serializer.Serialize<LearningResult>(this.learnedResults, filename);
            }
            catch
            {
                throw;
            }
        }

        public static Learner LoadFromFile(Robot robot, Vector2 centerPoint, string filename)
        {

            try
            {
                var savedLearnData = Serializer.Deserialize<LearningResult>(filename);
                if (savedLearnData == null) return null;
                var learner = new Learner(robot, centerPoint, savedLearnData);
                return learner;
            }
            catch
            {
                throw;
                //return null;
            }
        }
    }
}
