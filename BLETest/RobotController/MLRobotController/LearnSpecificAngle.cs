using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLETest.RobotController.MLRobotController
{
    [Serializable()]
    public class LearnSpecificAngle : ISerializable
    {
        public LearnSpecificAngle()
        {
            SuccessfullyLearned = false;
        }

        public LearnSpecificAngle(SerializationInfo info, StreamingContext context)
        {
            this.triedConfigurations = (List<LearnAngleResult>)info.GetValue("triedConfigurations", typeof(List<LearnAngleResult>));
        }

        /// <summary>
        /// this list is sorted ascending by the distance from the target
        /// </summary>
        List<LearnAngleResult> triedConfigurations = new List<LearnAngleResult>();
        public int NumberOfLearnTries { get { return triedConfigurations.Count; } }

        public MotorSpeed CurrentMotorSpeed { get; private set; }
        public void SetCurrentDeviation(double deviation)
        {
            var currentResult = new LearnAngleResult(CurrentMotorSpeed, deviation);
            triedConfigurations.Add(currentResult);
            triedConfigurations = triedConfigurations.OrderBy(element => Math.Abs(element.Deviation)).ToList();

            if (Math.Abs(deviation) < 3) SuccessfullyLearned = true;
        }

        public bool SuccessfullyLearned { get; private set; }
        /// <summary>
        /// returns the best MotorSpeed configuration (so far)
        /// </summary>
        public MotorSpeed LearnedMotorSpeed { get { return triedConfigurations[0].MotorSpeed; } }

        public MotorSpeed CreateNextTry()
        {
            if (SuccessfullyLearned)
            {
                return LearnedMotorSpeed;
            }

            var motorSpeed = new MotorSpeed(100, 0);

            switch (NumberOfLearnTries)
            {
                case 0:
                    //try without angular Speed -> this might be already the best solution
                    break;
                case 1:
                    motorSpeed.Angular = 100; //300;
                    break;
                case 2:
                    motorSpeed.Angular = -100; //-300; 300 -> move in circles
                    break;
                default:
                    motorSpeed.Angular = CalculateAngularSpeedFromBestTwoTries();
                    break;
            }

            CurrentMotorSpeed = motorSpeed;

            return CurrentMotorSpeed;
        }

        private int CalculateAngularSpeedFromBestTwoTries()
        {
            //local minima won't occur
            int angularSpeed;
            int modSize = 10;

            //make sure the same angular speed is not tried again
            // this is needed because it happened and messed up the system...
            do
            {
                angularSpeed = (int)(triedConfigurations[0].AngularSpeed + triedConfigurations[1].AngularSpeed) / 2;
                int modifier = (new Random()).Next(2*modSize) - modSize;
                if (modifier == 0) modifier = modSize;
                modSize++;

            } while (AngularSpeedWasAlreadyTried(angularSpeed));

            return angularSpeed;
        }

        private bool AngularSpeedWasAlreadyTried(int angularSpeed)
        {
            foreach (var item in triedConfigurations)
            {
                if (item.AngularSpeed == angularSpeed) return true;
            }
            return false;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("triedConfigurations", this.triedConfigurations);
        }
    }

}
