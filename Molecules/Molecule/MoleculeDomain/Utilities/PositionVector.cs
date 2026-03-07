namespace MoleculeDomain.Utilities
{
    public sealed record PositionVector(double PosX, double PosY, double PosZ)
    {
        public static PositionVector operator +(PositionVector a, PositionVector b) =>
            new(a.PosX + b.PosX, a.PosY + b.PosY, a.PosZ + b.PosZ);

        public static PositionVector operator -(PositionVector a, PositionVector b) =>
            new(a.PosX - b.PosX, a.PosY - b.PosY, a.PosZ - b.PosZ);

        public static PositionVector operator *(PositionVector v, double scalar) =>
            new(v.PosX * scalar, v.PosY * scalar, v.PosZ * scalar);

        public static PositionVector operator *(double scalar, PositionVector v) =>
            v * scalar;

        public static PositionVector operator /(PositionVector v, double scalar) =>
            new(v.PosX / scalar, v.PosY / scalar, v.PosZ / scalar);

        public double Dot(PositionVector other) =>
            PosX * other.PosX + PosY * other.PosY + PosZ * other.PosZ;

        public PositionVector Cross(PositionVector other) =>
            new(
                PosY * other.PosZ - PosZ * other.PosY,
                PosZ * other.PosX - PosX * other.PosZ,
                PosX * other.PosY - PosY * other.PosX
            );
    }
}
