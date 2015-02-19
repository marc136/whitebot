using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLETest
{
    /// <summary>
    /// vertical -> on a wall || horizontal -> on a table
    /// </summary>
    public enum Alignment
    {
        Unknown,
        Vertical,
        Horizontal
    }

    public class AlignmentEventArgs : EventArgs
    {
        public Alignment Alignment { get; private set; }

        public AlignmentEventArgs(Alignment alignment)
        {
            Alignment = alignment;
        }

        /// <summary>
        /// returns the enumeration name of a value converted to string
        /// </summary>
        public override string ToString()
        {// see: http://msdn.microsoft.com/de-de/library/c3s1ez6e(v=vs.110).aspx
            return Alignment.ToString("F");
        }
    }

    public class PositionEventArgs : EventArgs
    {
        public bool KnowsPosition { get; private set; }

        public Vector2 Position { get; private set; }

        public PositionEventArgs()
        {
            KnowsPosition = false;
        }

        public PositionEventArgs(Vector2 position)
        {
            KnowsPosition = true;
            Position = position;
        }

        public override string ToString()
        {
            return KnowsPosition ? 
                String.Format("(x:{0}|y:{1})", Position.X, Position.Y) : 
                "(x:?|y:?)";
        }
    }

    public class Robot
    {
        Blidget blidget;
        DifferentialDrive drive;
        Servo pen;
        Servo eraser;
        public int Id { get; private set; }
        RemoteSensors sensors;

        public Vector2 Position { get; private set; }
        public bool KnowsPosition { get; private set; }
        public Orientation Orientation { get; private set; }
        public Gravity Gravity { get; private set; }
        public Alignment Alignment { get; private set; }
        /// <summary>
        /// normalized look direction vector of the robot
        /// </summary>
        public Vector2 LookDirection { get; private set; }
        public bool CanNavigate { get; private set; }

        public int? TrackingId { get; set; }

        public Robot(int index, int id, RemoteSensors sensors)
        {
            try
            {
                blidget = Blidget.createFromIndex(index);
                initializeBlidgetComponents();
                CanNavigate = true;
            }
            catch (Exception)
            {
                Console.WriteLine("Roboter" + (index + 1) + " not present");
                CanNavigate = false;
            }

            initialize(id, sensors);
        }

        public Action<string> Log;
        private void OnLog(string message)
        {
            if (Log != null)
            {
                Log("Robot\t" + message);
            }
        }

        public Robot(string name, int id, RemoteSensors sensors)
        {
            try
            {
                blidget = Blidget.createFromName(name);
                initializeBlidgetComponents();
                CanNavigate = true;
            }
            catch (Exception)
            {
                Console.WriteLine(name + " not present");
                CanNavigate = false;
            }

            initialize(id, sensors);
        }

        private void initialize(int id, RemoteSensors sensors)
        {
            this.Id = id;
            Inverted = false;
            this.sensors = sensors;
            registerEventHandlers();
        }

        private void registerEventHandlers()
        {
            sensors.UpdateGravity += sensors_UpdateGravity;
            sensors.UpdateOrientation += sensors_UpdateOrientation;
            sensors.UpdatePositions += sensors_UpdatePositions;
        }

        void sensors_UpdatePositions(object sender, PositionsEventArgs e)
        {
            Vector2 newPosition;
            if (TrackingId.HasValue && e.Positions.TryGetValue(TrackingId.Value, out newPosition))
            {
                this.Position = newPosition;
                KnowsPosition = true;
                OnChangedPosition(new PositionEventArgs(Position));
            }
            else
            {
                KnowsPosition = false;
                OnChangedPosition(new PositionEventArgs());
            }
        }

        void sensors_UpdateOrientation(object sender, OrientationEventArgs e)
        {
            if(e.Id == Id)
            {
                //OnLog(e.ToString());
                Orientation = e.Orientation;
            }
        }

        void sensors_UpdateGravity(object sender, GravityEventArgs e)
        {
            if(e.Id == Id)
            {
                Gravity = e.Gravity;

                var length = Math.Asin(new Vector2(Gravity.vec.X, Gravity.vec.Y).Length() / 9.81) / (Math.PI / 2);
                if (length < 0.07)
                {
                    var m = Orientation.getRotationMatrix();
                    LookDirection = new Vector2(-m.M11, m.M12);
                    LookDirection.Normalize();

                    if (Alignment != Alignment.Horizontal)
                    {
                        Alignment = Alignment.Horizontal;
                        OnChangedAlignment(new AlignmentEventArgs(Alignment));
                    }
                }
                else if (length > 0.5) //0.93) //2015-01-04
                {
                    LookDirection = new Vector2(-Gravity.vec.X / 9.81f, -Gravity.vec.Y / 9.81f);
                    LookDirection.Normalize();
                    if (Alignment != Alignment.Vertical)
                    {
                        Alignment = Alignment.Vertical;
                        OnChangedAlignment(new AlignmentEventArgs(Alignment));
                    }
                }
                else
                {
                    LookDirection = Vector2.Zero;
                    if (Alignment != Alignment.Unknown)
                    {
                        Alignment = Alignment.Unknown;
                        OnChangedAlignment(new AlignmentEventArgs(Alignment));
                    }
                }
            }           
        }

        private void OnChangedAlignment(AlignmentEventArgs e)
        {
            OnLog(e.ToString());
            if(ChangedAlignment != null)
            {
                ChangedAlignment(this, e);
            }
        }

        private void OnChangedPosition(PositionEventArgs e)
        {
            OnLog(e.ToString());
            if (ChangedPosition != null)
            {
                ChangedPosition(this, e);
            }
        }

        public event EventHandler<AlignmentEventArgs> ChangedAlignment;

        public event EventHandler<PositionEventArgs> ChangedPosition;

        private void initializeBlidgetComponents()
        {
            var pwm = blidget.createPwm(1, 3, 4);
            //MediaNight Hack Eraser und pen vertauschen
            eraser = new Servo(pwm, 1);
            pen = new Servo(pwm, 0);

            var serial = blidget.createSerial(1, 0);

            drive = new DifferentialDrive(new SerialDualMotorController(serial), 0.08);
        }

        public bool Inverted { get; set; }

        public void Speed(double linear, double angular)
        {
            if(drive != null)
            {
                drive.speed(Inverted ? linear : -linear, Inverted ? -angular : angular);
            }
        }

        public void LinearSpeed(double speed)
        {
            if (drive != null)
            {
                drive.linearSpeed(Inverted ? speed : -speed);
            }
        }

        public void angularSpeed(double speed)
        {
            if (drive != null)
            {
                drive.angularSpeed(Inverted ? -speed : speed);
            }
        }

        public void penDown()
        {
            pen.setDuty(1300);
        }

        public void penUp()
        {
            pen.setDuty(1650);
        }

        public void penRelease()
        {
            pen.setDuty(0);
        }

        public void eraserDown()
        {
            eraser.setDuty(800);
        }

        public void eraserUp()
        {
            eraser.setDuty(550);
        }

        public void eraserRelease()
        {
            eraser.setDuty(0);
        }

        
    }
}
