using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLETest
{
    class SerialDualMotorController : IDualMotorController
    {
        Blidget.Serial m_serial;
        int m_leftSpeed = 0;
        int m_rightSpeed = 0;
        byte[] m_commandBuffer;
        SerialMotorController leftMotor;
        SerialMotorController rightMotor;

        public SerialDualMotorController(Blidget.Serial serial)
        {
            m_serial = serial;
            leftMotor = new SerialMotorController(serial, 0);
            rightMotor = new SerialMotorController(serial, 1);
            m_commandBuffer = new byte[6] { 255, 3, 0, 0, 0, 0 };
        }

        public void speed(int leftSpeed, int rightSpeed)
        {
            /*if(leftSpeed != m_leftSpeed && rightSpeed != m_rightSpeed)
            {
                m_leftSpeed = leftSpeed;
                m_rightSpeed = rightSpeed;
                m_commandBuffer[2] = (byte)(0xFF & (m_leftSpeed >> 8));
                m_commandBuffer[3] = (byte)(0xFF & (m_leftSpeed));
                m_commandBuffer[4] = (byte)(0xFF & (m_rightSpeed >> 8));
                m_commandBuffer[5] = (byte)(0xFF & (m_rightSpeed));
                m_serial.sendData(m_commandBuffer);oooooooooooooooooooooooooooooooo7
            }
            else*/ if(leftSpeed != m_leftSpeed)
            {
                m_leftSpeed = leftSpeed;
                leftMotor.speed(-leftSpeed);
            }
            /*else*/ if(rightSpeed != m_rightSpeed)
            {
                m_rightSpeed = rightSpeed;
                rightMotor.speed(-rightSpeed);
            }
            //Console.WriteLine("Speed left: " + leftSpeed + "\tright:" + rightSpeed );
        }

        int leftSpeed()
        {
            return m_leftSpeed;
        }

        int rightSpeed()
        {
            return m_rightSpeed;
        }

        public int maxSpeed()
        {
            return Math.Min(leftMotor.maxSpeed(), rightMotor.maxSpeed());
        }
    }
}
