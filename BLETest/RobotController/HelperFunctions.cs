using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLETest.RobotController
{
    class HelperFunctions
    {

        /// <summary>
        /// signed distance
        /// + -> right off path (y smaller)
        /// - -> left off path (y greater)
        /// </summary>
        public static double CalculateSignedDistance(Vector2 lineBegin, Vector2 lineEnd, Vector2 point)
        {
            var d = lineEnd - lineBegin;

            var distance = (d.Y * point.X - d.X * point.Y - lineBegin.X * lineEnd.Y + lineEnd.X * lineBegin.Y) / d.Length();

            return distance;
        }
    }
}
