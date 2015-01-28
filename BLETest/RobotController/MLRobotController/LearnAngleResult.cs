using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLETest.RobotController.MLRobotController
{
    [Serializable()]
    public class LearnAngleResult : ISerializable
    {
        public int LinearSpeed = 0;
        public int AngularSpeed = 0;
        public double Deviation = 0;

        public LearnAngleResult() { }
        public LearnAngleResult(SerializationInfo info, StreamingContext context)
        {
            this.LinearSpeed = (int)info.GetValue("LinearSpeed", typeof(int));
            this.AngularSpeed = (int)info.GetValue("AngularSpeed", typeof(int));
            this.Deviation = (double)info.GetValue("Deviation", typeof(double));
        }


        public LearnAngleResult(MotorSpeed speed, double deviation) : this(speed.Linear, speed.Angular, deviation) { }

        public LearnAngleResult(int linearSpeed, int angularSpeed, double deviation)
        {
            LinearSpeed = linearSpeed;
            AngularSpeed = angularSpeed;
            Deviation = deviation;
        }

        public MotorSpeed MotorSpeed { get { return new MotorSpeed(LinearSpeed, AngularSpeed); } }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("LinearSpeed", this.LinearSpeed);
            info.AddValue("AngularSpeed", this.AngularSpeed);
            info.AddValue("Deviation", this.Deviation);
        }
    }

}
