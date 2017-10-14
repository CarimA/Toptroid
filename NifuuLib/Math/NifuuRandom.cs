using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NifuuLib.Math
{
    public class NifuuRandom
    {
        private int _State;

        public NifuuRandom() : this(1) { }
        public NifuuRandom(int seed)
        {
            if (seed < 1)
                throw new ArgumentOutOfRangeException(nameof(seed), "Seed must be greater than zero.");

            _State = seed;
        }

        public int Next()
        {
            _State = 214013 * _State + 2531011;
            return (_State >> 16) & 0x7fff;
        }

        public int Next(int max) => (int)(max * NextSingle() + 0.5f);        
        public int Next(int min, int max) => (int)((max - min) * NextSingle() + 0.5f) + min;

        public float NextSingle() => Next() / (float)short.MaxValue;
        public float NextSingle(float max) => max * NextSingle();
        public float NextSingle(float min, float max) => (max - min) * NextSingle() + min;

        public float NextAngle() => NextSingle(-MathHelper.Pi, MathHelper.Pi);

        public Vector2 NextUnitVector()
        {
            var angle = NextAngle();
            return new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle));
        }

    }
}
