using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBot
{
    class SerialMotorController
    {
        Blidget.Serial m_serial;
        int m_motorId;
        byte[] m_commandBuffer = new byte[4];

        public SerialMotorController(Blidget.Serial serial, byte motorId)
        {
            m_serial = serial;
            m_motorId = motorId;
            m_commandBuffer = new byte[4] { 255, motorId, 0, 0};
        }

        public void speed(int speed)
        {
            m_commandBuffer[2] = (byte)(0xFF & (speed >> 8));
            m_commandBuffer[3] = (byte)(0xFF & (speed));
            m_serial.sendData(m_commandBuffer);
        }

        public int maxSpeed()
        {
            return 255;
        }
    }
}
