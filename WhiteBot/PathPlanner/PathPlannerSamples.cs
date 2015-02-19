using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLETest
{
    class PathPlannerSamples
    {

        public static void DrawSineFunction(PathPlanner p)
        {
            Vector2[] points = new Vector2[15];
            float scale = 200.0f / points.Length;
            for (int i = 0; i < points.Length; ++i)
            {
                points[i] = new Vector2(60 + scale * i, (float)(135 - 80 * Math.Sin(i * Math.PI / (points.Length - 1))));
            }
            p.drawCurvedLineStrip(points.First(), points.Skip(1).ToArray());
        }

        public static void StraightSamples(PathPlanner p)
        {
            var startPoint = new Vector2(150, 150);
            //square
            //DrawSquare(p, startPoint);
            //star
            //DrawStar(p, startPoint);
            //
            //DrawAngles(p, startPoint);
            p.DrawLine(new Vector2(30, 40), new Vector2(200, 40));
        }

        public static void DrawSquare(PathPlanner p, Vector2 start, int size = 20)
        {
            var radius = size / 2;
            p.drawStraightLineStrip(start,
                new Vector2(start.X + radius, start.Y + radius), new Vector2(start.X + radius, start.Y - radius),
                new Vector2(start.X - radius, start.Y - radius), new Vector2(start.X - radius, start.Y + radius));
        }

        public static void DrawStar(PathPlanner p, Vector2 start, int size = 25)
        {
            p.drawStraightLineStrip(start, 
                new Vector2(start.X+size, start.Y+size), new Vector2(start.X, start.Y+2*size), 
                new Vector2(start.X-size, start.Y+size), start);
        }

        public static void DrawAngles(PathPlanner p, Vector2 start)
        {
            p.DrawLine(start, new Vector2(start.X + 20, start.Y + 10));
            p.DrawLine(start, new Vector2(start.X + 30, start.Y + 10));
            p.DrawLine(start, new Vector2(start.X + 40, start.Y + 10));
            p.DrawLine(start, new Vector2(start.X + 20, start.Y - 10));
            p.DrawLine(start, new Vector2(start.X + 30, start.Y - 10));
            p.DrawLine(start, new Vector2(start.X + 40, start.Y - 10));

            p.DrawLine(start, new Vector2(start.X - 20, start.Y + 10));
            p.DrawLine(start, new Vector2(start.X - 30, start.Y + 10));
            p.DrawLine(start, new Vector2(start.X - 40, start.Y + 10));
            p.DrawLine(start, new Vector2(start.X - 20, start.Y - 10));
            p.DrawLine(start, new Vector2(start.X - 30, start.Y - 10));
            p.DrawLine(start, new Vector2(start.X - 40, start.Y - 10));


            p.DrawLine(start, new Vector2(start.X + 10, start.Y + 20));
            p.DrawLine(start, new Vector2(start.X + 10, start.Y + 30));
            p.DrawLine(start, new Vector2(start.X + 10, start.Y + 40));
            p.DrawLine(start, new Vector2(start.X - 10, start.Y + 20));
            p.DrawLine(start, new Vector2(start.X - 10, start.Y + 30));
            p.DrawLine(start, new Vector2(start.X - 10, start.Y + 40));

            p.DrawLine(start, new Vector2(start.X + 10, start.Y - 20));
            p.DrawLine(start, new Vector2(start.X + 10, start.Y - 30));
            p.DrawLine(start, new Vector2(start.X + 10, start.Y - 40));
            p.DrawLine(start, new Vector2(start.X - 10, start.Y - 20));
            p.DrawLine(start, new Vector2(start.X - 10, start.Y - 30));
            p.DrawLine(start, new Vector2(start.X - 10, start.Y - 40));
        }

        public static void House(PathPlanner p, Vector2 start, int size = 25)
        {
            p.drawStraightLineStrip(
                start, new Vector2(start.X + size, start.Y), new Vector2(start.X+size, start.Y+size)
                );
        }
    }
}
