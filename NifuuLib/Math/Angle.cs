using Microsoft.Xna.Framework;
using System;
using static System.Math;

namespace NifuuLib.Math
{
    public struct Angle : IComparable<Angle>, IEquatable<Angle>
    {
        public const float TAU = (float)(PI * 2.0);
        public const float TAU_INVERSE = (float)(0.5 / PI);
        public const float DEGREE_TO_RADIAN = (float)(PI / 180.0);
        public const float RADIAN_TO_DEGREE = (float)(180.0 / PI);
        public const float GRADIAN_TO_RADIAN = (float)(PI / 200.0);
        public const float RADIAN_TO_GRADIAN = (float)(200.0 / PI);

        public float Radians { get; set; }

        public float Degrees
        {
            get { return Radians * RADIAN_TO_DEGREE; }
            set { Radians = value * DEGREE_TO_RADIAN; }
        }

        public float Gradians
        {
            get { return Radians * RADIAN_TO_GRADIAN; }
            set { Radians = value * GRADIAN_TO_RADIAN; }
        }

        public float Revolutions
        {
            get { return Radians * TAU_INVERSE; }
            set { Radians = value * TAU; }
        }

        public Angle(float value, AngleType angleType = AngleType.Radian)
        {
            Radians = 0f;
            switch (angleType)
            {
                default:
                    throw new ArgumentOutOfRangeException(nameof(angleType), "Invalid AngleType.");
                case AngleType.Radian:
                    Radians = value;
                    break;
                case AngleType.Degree:
                    Degrees = value;
                    break;
                case AngleType.Gradian:
                    Gradians = value;
                    break;
                case AngleType.Revolution:
                    Revolutions = value;
                    break;
            }
        }

        public void Wrap()
        {
            var angle = Radians % TAU;

            if (angle <= PI)
                angle += TAU;

            if (angle > PI)
                angle -= TAU;

            Radians = angle;
        }

        public void WrapPositive()
        {
            Radians %= TAU;
            if (Radians < 0d)
                Radians += TAU;
        }

        public bool IsBetween(Angle min, Angle end)
        {
            return end < min ?
                this >= min || this <= end :
                this >= min && this <= end;
        }
        
        public static Angle FromVector(Vector2 vector) => new Angle((float)Atan2(-vector.Y, vector.X));

        public Vector2 ToUnitVector() => ToVector(1);
        public Vector2 ToVector(float length) => new Vector2(length * (float)Cos(Radians), -length * (float)Sin(Radians));

        public int CompareTo(Angle other)
        {
            WrapPositive();
            other.WrapPositive();
            return Radians.CompareTo(other.Radians);
        }

        public bool Equals(Angle other)
        {
            WrapPositive();
            other.WrapPositive();
            return Radians.Equals(other.Radians);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;

            return obj is Angle && Equals((Angle)obj);
        }

        public override int GetHashCode()
        {
            return Radians.GetHashCode();
        }

        public static implicit operator float(Angle angle)
        {
            return angle.Radians;
        }

        public static explicit operator Angle(float angle)
        {
            return new Angle(angle);
        }

        public override string ToString()
        {
            return $"{Radians} rad";
        }
    }
}
