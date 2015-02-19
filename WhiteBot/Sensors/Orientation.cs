using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLETest
{
    public struct Orientation
    {
        private Quaternion rotation;

        public Orientation(params float[] p)
        {
            rotation = new Quaternion(p[0], p[1], p[2], p[3]);
        }

        public Matrix getRotationMatrix()
        {
            Matrix matrix;
            Matrix.CreateFromQuaternion(ref rotation, out matrix);
            return matrix;
        }

        public Vector3 GetDirectionVector()
        {
            return getRotationMatrix().Forward;
        }
    }
}
