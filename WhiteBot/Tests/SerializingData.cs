using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WhiteBot.RobotController.MLRobotController;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using WhiteBot;
using Microsoft.Xna.Framework;

namespace Tests
{
    [TestClass]
    public class SerializingData
    {
        [TestMethod]
        public void SerializeAndDeserializeLearnAngleResult()
        {

            var ar = new LearnAngleResult(100, 10, 30);
            var filename = "serialized.xml";

            Serializer.Serialize<LearnAngleResult>(ar, filename);

            ar = null;
            ar = Serializer.Deserialize<LearnAngleResult>(filename);

            Assert.AreEqual(100, ar.LinearSpeed);
            Assert.AreEqual(10, ar.AngularSpeed);
            Assert.AreEqual(30, ar.Deviation);            
        }

        [TestMethod]
        public void SerializeLearner()
        {

            var l = new Learner(null, Vector2.Zero);
            var filename = "serialized.xml";

            l.SelectNewAngleToLearn();

            l.SaveToFile(filename);
            l = null;
            Assert.IsNull(l);
            
            l = Learner.LoadFromFile(null, Vector2.Zero, filename);
            Assert.AreEqual(0, l.Generation);
            Assert.AreEqual(4, l.learnedResults.Count);
        }
    }
}
