using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLETest
{
    public struct Gravity
    {
        public readonly Vector3 vec;

        public Gravity(params float[] p)
        {
            vec = new Vector3(p[0], p[1], p[2]);
        }
    }
}
