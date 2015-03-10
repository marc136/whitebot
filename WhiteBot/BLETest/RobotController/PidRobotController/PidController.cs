using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBot
{
    class PidController
    {
        public double Ki;
        public double Kd;
        public double Kp;

        double prevError = 0;

        double accumError = 0;
        int sampleCount = 0;

        public int MaxSampleCount = 100;

        public double update(double error)
        {
            var P = error * Kp;
            var D = (error - prevError) * Kd;
            prevError = error;

            ++sampleCount;
            accumError += error / sampleCount;

            if (sampleCount > MaxSampleCount)
            {
                sampleCount = MaxSampleCount;
            }

            var I = accumError * Ki;

            return P + I + D;
        }

        public void reset()
        {
            prevError = 0;
            accumError = 0;
            sampleCount = 0;
        }
    }
}
