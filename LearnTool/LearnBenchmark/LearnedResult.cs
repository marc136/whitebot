using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnBenchmark
{
    public class LearnedResult
    {
        public string Title { get; set; }
        public double Deviation { get; set; }

        public LearnedResult(string title, double deviation)
        {
            Title = title;
            Deviation = deviation;
        }

        public static LearnedResult LoadFromFile(string filename)
        {
            var title = System.IO.Path.GetFileName(filename);

            LearningResult imported = Serializer.Deserialize<LearningResult>(filename);
            double sum = 0;
            int count = 0;
            double deviation;

            foreach (var entry in imported.learnedAngles)
            {
                LearnSpecificAngle angle = entry.Value;
                deviation = angle.triedConfigurations[0].Deviation;
                sum += Math.Abs(deviation);
                count++;
            }

            deviation = sum / count;

            var result = new LearnedResult(title, deviation);
            return result;
        }
    }
}
