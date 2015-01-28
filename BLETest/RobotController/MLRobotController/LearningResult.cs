using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BLETest.RobotController.MLRobotController
{
    [Serializable()]
    public class LearningResult : ISerializable
    {
        Dictionary<Vector2, LearnSpecificAngle> learnedAngles;

        public LearningResult()
        {
            learnedAngles = new Dictionary<Vector2, LearnSpecificAngle>();
        }

        public LearningResult(SerializationInfo info, StreamingContext context)
        {
            this.learnedAngles = (Dictionary<Vector2, LearnSpecificAngle>)info.GetValue("learnedAngles",
                                typeof(Dictionary<Vector2, LearnSpecificAngle>));
        }

        public LearnSpecificAngle ForNormalizedVector(Vector2 aVector)
        {//todo: return closest Vector
            LearnSpecificAngle result = null;
            if (learnedAngles.TryGetValue(aVector, out result))
            {
                return result;
            }
            else
            {
                //iterate over all known vectors to find the closest match
                float min = Int32.MaxValue;
                float diff;
                Vector2 resultVector = Vector2.Zero;

                foreach (Vector2 angle in learnedAngles.Keys)
                {
                    diff = (angle - aVector).Length();
                    if (min > diff)
                    {
                        min = diff;
                        resultVector = angle;
                    }
                }

                if (learnedAngles.TryGetValue(resultVector, out result))
                {
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("learnedAngles", learnedAngles);
        }

        internal void AddNewLearningVector(Vector2 vector)
        {
            learnedAngles.Add(vector, new LearnSpecificAngle());
        }

        internal Vector2 GetNotLearnedAngleWithLowestNumberOfTries()
        {
            var useAngle = learnedAngles.First();
            var min = useAngle.Value.NumberOfLearnTries;
            if (min > 0)
            {
                var nextAngle = learnedAngles.FirstOrDefault(item =>
                {
                    return item.Value.NumberOfLearnTries < min && !item.Value.SuccessfullyLearned;
                });
                if (nextAngle.Value != null)
                {
                    useAngle = nextAngle;
                }
            }
            return useAngle.Key;
        }

        internal int GetLowestNumberOfTries()
        {
            int min = Int32.MaxValue;
            foreach (KeyValuePair<Vector2, LearnSpecificAngle> entry in learnedAngles)
            {
                if (min > entry.Value.NumberOfLearnTries) min = entry.Value.NumberOfLearnTries;
            }
            return min;
        }

        public int Count { get { return learnedAngles.Count; } }


    }

}
