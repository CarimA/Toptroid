using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Toptroid
{
    public class Physics
    {
        private static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }

        private static List<Vector2> BresenhamLine(int x0, int y0, int x1, int y1)
        {
            // Optimization: it would be preferable to calculate in
            // advance the size of "result" and to use a fixed-size array
            // instead of a list.
            List<Vector2> result = new List<Vector2>();
            
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                Swap(ref x0, ref y0);
                Swap(ref x1, ref y1);
            }
            if (x0 > x1)
            {
                Swap(ref x0, ref x1);
                Swap(ref y0, ref y1);
            }

            int deltax = x1 - x0;
            int deltay = Math.Abs(y1 - y0);
            int error = 0;
            int ystep;
            int y = y0;
            if (y0 < y1) ystep = 1; else ystep = -1;
            for (int x = x0; x <= x1; x++)
            {
                if (steep) result.Add(new Vector2(y, x));
                else result.Add(new Vector2(x, y));
                error += deltay;
                if (2 * error >= deltax)
                {
                    y += ystep;
                    error -= deltax;
                }
            }

            return result;
        }

        public static bool Raycast(Vector2 pointA, Vector2 rayDirection, float rayLength, bool[,] mask, out Vector2 collisionPoint, out float collisionDistance)
        {
            if (rayDirection == Vector2.Zero || float.IsNaN(rayDirection.X) || float.IsNaN(rayDirection.Y))
            {
                collisionDistance = 0;
                collisionPoint = pointA;
                return false;
            }

            if (rayLength == 0)
            {
                rayLength = 1;
            }

            rayDirection.Normalize();
            List<Vector2> points = BresenhamLine((int)pointA.X, (int)pointA.Y, (int)(pointA.X + (rayDirection.X * rayLength)), (int)(pointA.Y + (rayDirection.Y * rayLength)));
            points.Sort((a, b) =>
            {
                if (Vector2.Distance(pointA, a) < Vector2.Distance(pointA, b))
                {
                    return -1;
                }
                else if (Vector2.Distance(pointA, a) > Vector2.Distance(pointA, b))
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            });

            foreach (Vector2 pointB in points)
            {
                if (mask[(int)pointB.X, (int)pointB.Y])
                {
                    collisionPoint = pointB;
                    collisionDistance = Vector2.Distance(pointA, pointB);
                    return true;
                }
            }
            collisionDistance = rayLength;
            collisionPoint = points.Last();
            return false;
        }
    }
}
