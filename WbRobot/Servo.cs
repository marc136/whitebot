using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLETest
{
    public class Servo
    {
        Blidget.Pwm pwm;
        const int motorFrequency = 20;
        int minDuty;
        int maxDuty;
        int channel;
        int duty = 0;

        public Servo(Blidget.Pwm pwm, int channel)
        {
            this.pwm = pwm;
            this.channel = channel;
            int prescaler = 4;
            int scaled = 16000000 / (1 << prescaler);
            int max = scaled / 20;
            pwm.reset(prescaler, max);
            minDuty = 50000 / 20;
            maxDuty = (50000 / 20) * 2;
        }
        
        public void setPosition(double pos)
        {
            int duty = (int)(minDuty + (maxDuty - minDuty) * pos);
            Console.WriteLine("Duty: " + duty);
            setDuty(duty);
        }

        public void setDuty(int duty)
        {
            if(duty != this.duty)
            {
                this.duty = duty;
                pwm.setValue(channel, duty);
            }
        }
        
        public void disable()
        {
            if(duty != 0)
            {
                duty = 0;
                pwm.setValue(channel, 0);
            }
        }
    }
}
