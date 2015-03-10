using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBot
{
    public interface IDualMotorController
    {
        void speed(int leftSpeed, int rightSpeed);
        int maxSpeed();
    }
}
