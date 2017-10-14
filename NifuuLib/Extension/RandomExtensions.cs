using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Extension
{
    public static class RandomExtensions
    {
        public static float NextSingle(this Random random) => (float)random.NextDouble();
        public static float NextSingle(this Random random, float max) => max * random.NextSingle();
        public static float NextSingle(this Random random, float min, float max) => (max - min) * random.NextSingle() + min;

        public static double NextDouble(this Random random, double minValue, double maxValue) => random.NextDouble() * (maxValue - minValue) + minValue;
       
        public static float NextAngle(this Random random) => random.NextSingle(-MathHelper.Pi, MathHelper.Pi);

        public static Vector2 NextPoint(this Random random, Rectangle rectangle) => new Vector2(random.NextSingle(rectangle.Left, rectangle.Right), random.NextSingle(rectangle.Top, rectangle.Bottom));

        public static T RandomItem<T>(this Random random, List<T> list)
        {
            int index = random.Next(list.Count);
            return list[index];
        }

        public static Vector2 NextCirclePoint(this Random random, Vector2 origin, float radius)
        {
            var angle = random.NextDouble() * System.Math.PI * 2;
            var rad = System.Math.Sqrt(random.NextDouble() * radius);
            var x = origin.X + rad * System.Math.Cos(angle);
            var y = origin.Y + rad * System.Math.Sin(angle);
            return new Vector2((float)x, (float)y);
        }
    }
}
