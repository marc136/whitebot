using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.IO;
using Microsoft.Xna.Framework;

namespace BLETest
{
    enum TouchState : byte
    {
        Touched = 1,
        Moved = 2,
        Released = 3
    }

    enum SensorType : byte
    {
        RotationVector = 11,
        Gravity = 9
    }

    struct TrackedDirection
    {
        public int timestamp;
        public int robotId;
        public SensorType type;
        public float[] values;
    }

    struct TrackedPoint
    {
        public TouchState state;
        public int timestamp;
        public int id;
        public double x;
        public double y;
    }

    public class OrientationEventArgs : EventArgs
    {
        public int Id { get; private set; }
        public Orientation Orientation { get; private set; }

        public OrientationEventArgs(int id, Orientation orientation)
        {
            Id = id;
            Orientation = orientation;
        }

        public override string ToString()
        {
            return Orientation.GetDirectionVector().ToString();
        }
    }

    public class PositionsEventArgs : EventArgs
    {
        public Dictionary<int, Vector2> Positions { get; private set; }

        public PositionsEventArgs(Dictionary<int, Vector2> positions)
        {
            Positions = positions;
        }
    }

    public class GravityEventArgs : EventArgs
    {
        public int Id { get; private set; }
        public Gravity Gravity { get; private set; }

        public GravityEventArgs(int id, Gravity gravity)
        {
            Id = id;
            Gravity = gravity;
        }
    }

    public class RemoteSensors
    {
        UdpClient net = new UdpClient(12345, AddressFamily.InterNetwork);
        IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.173.206"), 12345);

        public RemoteSensors()
        {
        }
        
        bool _terminated = false;
        IAsyncResult result;
        
        public void Start()
        {
            result = net.BeginReceive(this.receiveCallback, null);
        }

        public void Close()
        {
            _terminated = true;
        }

        private void receiveCallback(IAsyncResult ar)
        {
            if(_terminated)
            {
                return;
            }

            Byte[] receiveBytes = net.EndReceive(ar, ref ep);
            onReceive(receiveBytes);
            net.BeginReceive(this.receiveCallback, null);
        }

        Dictionary<int, Vector2> points = new Dictionary<int, Vector2>();
        Dictionary<int, Vector2> currentPoints = new Dictionary<int, Vector2>();

        Dictionary<int, Orientation> orientations = new Dictionary<int,Orientation>();
        Dictionary<int, Gravity> gravities = new Dictionary<int, Gravity>();

        private void onReceive(byte[] receiveBytes)
        {
            var reader = new BinaryReader(new MemoryStream(receiveBytes));
            
            switch(reader.ReadByte())
            {
                case 0:
                    TrackedDirection direction = parseDirection(reader);
                    update(direction);
                    break;
                case 1:
                    TrackedPoint p = parsePosition(reader);
                    update(p);
                    break;
                default:
                    throw new Exception("Unknown packet type!");
            }
        }

        public Func<Vector2, Vector2> PositionTransform;

        private void update(TrackedDirection trackedDirection)
        {
            if(trackedDirection.type == SensorType.RotationVector)
            {
                var orientation = new Orientation(trackedDirection.values);
                orientations[trackedDirection.robotId] = orientation;

                OnUpdateOrientation(new OrientationEventArgs(trackedDirection.robotId, orientation));
            }
            else if(trackedDirection.type == SensorType.Gravity)
            {
                var gravity = new Gravity(trackedDirection.values);
                gravities[trackedDirection.robotId] = gravity;
                OnUpdateGravity(new GravityEventArgs(trackedDirection.robotId, gravity));
            }
        }

        int lastTimestamp = -1;

        private void update(TrackedPoint p)
        {
            if(p.timestamp != lastTimestamp)
            {
                lastTimestamp = p.timestamp;
                OnUpdatePositions(new PositionsEventArgs(currentPoints));
                points = currentPoints;
                currentPoints = new Dictionary<int, Vector2>();
            }

            switch(p.state)
            {
                case TouchState.Touched:
                case TouchState.Moved:
                    if(PositionTransform != null)
                    {
                        currentPoints[p.id] = PositionTransform(new Vector2((float)p.x, (float)p.y));
                    }
                    else
                    {
                        currentPoints[p.id] = new Vector2((float)p.x, (float)p.y);
                    }
                    break;
            }
        }

        private TrackedDirection parseDirection(BinaryReader reader)
        {
            TrackedDirection result;
            result.timestamp = reader.ReadInt32();
            result.robotId = reader.ReadByte();
            result.type = (SensorType)reader.ReadByte();
            byte count = reader.ReadByte();
            result.values = new float[count];
            for (int i = 0; i < count; ++i)
            {
                result.values[i] = reader.ReadSingle();
            }
            return result;
        }

        private TrackedPoint parsePosition(BinaryReader s)
        {
            TrackedPoint result;
            result.timestamp = s.ReadInt32();
            result.id = s.ReadInt32();
            result.state = (TouchState)s.ReadByte();
            result.x = s.ReadDouble();
            result.y = s.ReadDouble();
            //result.x = s.ReadInt16();
            //result.y = s.ReadInt16();
            return result;
        }

        private void OnUpdatePositions(PositionsEventArgs e)
        {
            if(UpdatePositions != null)
            {
                UpdatePositions(this, e);
            }
        }

        private void OnUpdateOrientation(OrientationEventArgs e)
        {
            if (UpdateOrientation != null)
            {
                UpdateOrientation(this, e);
            }
        }

        private void OnUpdateGravity(GravityEventArgs e)
        {
            if (UpdateGravity != null)
            {
                UpdateGravity(this, e);
            }
        }

        public event EventHandler<PositionsEventArgs> UpdatePositions;
        public event EventHandler<OrientationEventArgs> UpdateOrientation;
        public event EventHandler<GravityEventArgs> UpdateGravity;
    }
}
