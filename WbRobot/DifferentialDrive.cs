using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLETest
{
    class DifferentialDrive
    {
        IDualMotorController m_motors;
        
        double m_wheelDistance;

        double m_leftSpeed = 0.0;
        double m_rightSpeed = 0.0;

        double m_linear = 0.0;
        double m_angular = 0.0;

        public DifferentialDrive(IDualMotorController motors, double wheelDistance)
        {
            m_motors = motors;
            m_wheelDistance = wheelDistance;
        }

        public void speed(double linear, double angular)
        {
            if(linear == m_linear && angular == m_angular)
            {
                return;
            }

            m_linear = linear;
            m_angular = angular;

            var normalizedAngular = angular * (Math.PI * m_wheelDistance) / 2;

            var leftSpeed = linear - normalizedAngular;
            var rightSpeed = linear + normalizedAngular;

            if(Math.Abs(leftSpeed) > m_motors.maxSpeed())
            {
                var scaling = m_motors.maxSpeed() / Math.Abs(leftSpeed);
                leftSpeed *= scaling;
                rightSpeed *= scaling;
            }
            if (Math.Abs(rightSpeed) > m_motors.maxSpeed())
            {
                var scaling = m_motors.maxSpeed() / Math.Abs(rightSpeed);
                leftSpeed *= scaling;
                rightSpeed *= scaling;
            }

            m_motors.speed((int)Math.Round(leftSpeed), (int)Math.Round(rightSpeed));
            this.m_leftSpeed = leftSpeed;
            this.m_rightSpeed = rightSpeed;
        }

        public void linearSpeed(double linear)
        {
            speed(linear, m_angular);
        }

        public void angularSpeed(double angular)
        {
            speed(m_linear, angular);
        }
    }
}
