using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnBenchmark
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
}
