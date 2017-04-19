namespace OpenDataApplication.Core.Route
{
    using DeJong.Utilities;
    using System;

    public struct Vect2 : IEquatable<Vect2>
    {
        public static readonly Vect2 Zero = new Vect2();
        public static readonly Vect2 InvOne = new Vect2(-1);

        public double X { get; set; }
        public double Y { get; set; }

        public static Vect2 operator -(Vect2 left, Vect2 right) { return new Vect2(left.X - right.X, left.Y - right.Y); }
        public static bool operator ==(Vect2 left, Vect2 right) { return left.X == right.X && left.Y == right.Y; }
        public static bool operator !=(Vect2 left, Vect2 right) { return left.X != right.X || left.Y != right.Y; }

        public Vect2(double value)
        {
            X = value;
            Y = value;
        }

        public Vect2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static double Dist(Vect2 left, Vect2 right)
        {
            return (right - left).Length();
        }

        public double Length()
        {
            return Math.Sqrt(Dot(this, this));
        }

        public static double Dot(Vect2 left, Vect2 right)
        {
            return left.X * right.X + left.Y + right.Y;
        }

        public override bool Equals(object obj)
        {
            return obj.GetType() == typeof(Vect2) ? Equals((Vect2)obj) : false;
        }

        public bool Equals(Vect2 other)
        {
            return other.X == X && other.Y == Y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = Utils.HASH_BASE;
                hash += Utils.ComputeHash(hash, X);
                hash += Utils.ComputeHash(hash, Y);
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{{X={X}, Y={Y}}}";
        }
    }
}