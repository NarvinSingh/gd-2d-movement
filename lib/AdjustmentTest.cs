using static Com.NarvinSingh.Utility.Adjustment;

namespace Com.NarvinSingh.UnitTest
{
    public static class AdjustmentTest
    {
        public static bool ItGetsSign()
        {
            if (Sign(0) != 1) return false;
            if (Sign(1) != 1) return false;
            if (Sign(-1) != -1) return false;
            if (Sign(int.MaxValue) != 1) return false;
            if (Sign(int.MinValue) != -1) return false;

            if (Sign(0.0F) != 1) return false;
            if (Sign(1.0F) != 1) return false;
            if (Sign(-1.0F) != -1) return false;
            if (Sign(float.Epsilon) != 1) return false;
            if (Sign(-float.Epsilon) != -1) return false;
            if (Sign(float.MaxValue) != 1) return false;
            if (Sign(float.MinValue) != -1) return false;
            if (Sign(float.PositiveInfinity) != 1) return false;
            if (Sign(float.NegativeInfinity) != -1) return false;

            if (Sign(0.0) != 1) return false;
            if (Sign(1.0) != 1) return false;
            if (Sign(-1.0) != -1) return false;
            if (Sign(double.Epsilon) != 1) return false;
            if (Sign(-double.Epsilon) != -1) return false;
            if (Sign(double.MaxValue) != 1) return false;
            if (Sign(double.MinValue) != -1) return false;
            if (Sign(double.PositiveInfinity) != 1) return false;
            if (Sign(double.NegativeInfinity) != -1) return false;

            return true;
        }

        public static bool ItAbsolutes()
        {
            if (Absolute(0) != 0) return false;
            if (Absolute(1) != 1) return false;
            if (Absolute(-1) != 1) return false;
            if (Absolute(int.MaxValue) != int.MaxValue) return false;
            if (Absolute(-int.MaxValue) != int.MaxValue) return false;

            if (Absolute(0.0F) != 0) return false;
            if (Absolute(1.0F) != 1) return false;
            if (Absolute(-1.0F) != 1) return false;
            if (Absolute(0.0000001F) != 0.0000001F) return false;
            if (Absolute(-0.0000001F) != 0.0000001F) return false;
            if (Absolute(float.Epsilon) != float.Epsilon) return false;
            if (Absolute(-float.Epsilon) != float.Epsilon) return false;
            if (Absolute(float.MinValue) != -float.MinValue) return false;
            if (Absolute(float.MaxValue) != float.MaxValue) return false;
            if (Absolute(float.PositiveInfinity) != float.PositiveInfinity) return false;
            if (Absolute(-float.NegativeInfinity) != float.PositiveInfinity) return false;
            if (!float.IsNaN(Absolute(float.NaN))) return false;

            if (Absolute(0.0) != 0) return false;
            if (Absolute(1.0) != 1) return false;
            if (Absolute(-1.0) != 1) return false;
            if (Absolute(0.0000001) != 0.0000001) return false;
            if (Absolute(-0.0000001) != 0.0000001) return false;
            if (Absolute(double.Epsilon) != double.Epsilon) return false;
            if (Absolute(-double.Epsilon) != double.Epsilon) return false;
            if (Absolute(double.MinValue) != -double.MinValue) return false;
            if (Absolute(double.MaxValue) != double.MaxValue) return false;
            if (Absolute(double.PositiveInfinity) != double.PositiveInfinity) return false;
            if (Absolute(-double.NegativeInfinity) != double.PositiveInfinity) return false;
            if (!double.IsNaN(Absolute(double.NaN))) return false;

            return true;
        }

        public static bool ItClamps()
        {
            if (Clamp(0, 1) != 0) return false;
            if (Clamp(1, 1) != 1) return false;
            if (Clamp(2, 1) != 1) return false;
            if (Clamp(0, -1) != 0) return false;
            if (Clamp(1, -1) != 1) return false;
            if (Clamp(2, -1) != 1) return false;
            if (Clamp(-1, 1) != -1) return false;
            if (Clamp(-2, 1) != -1) return false;
            if (Clamp(-1, -1) != -1) return false;
            if (Clamp(-2, -1) != -1) return false;

            if (Clamp(0.0F, 1.0F) != 0) return false;
            if (Clamp(1.0F, 1.0F) != 1) return false;
            if (Clamp(2.0F, 1.0F) != 1) return false;
            if (Clamp(0.0F, -1.0F) != 0) return false;
            if (Clamp(1.0F, -1.0F) != 1) return false;
            if (Clamp(2.0F, -1.0F) != 1) return false;
            if (Clamp(-1.0F, 1.0F) != -1) return false;
            if (Clamp(-2.0F, 1.0F) != -1) return false;
            if (Clamp(-1.0F, -1.0F) != -1) return false;
            if (Clamp(-2.0F, -1.0F) != -1) return false;

            if (Clamp(0.0, 1.0) != 0) return false;
            if (Clamp(1.0, 1.0) != 1) return false;
            if (Clamp(2.0, 1.0) != 1) return false;
            if (Clamp(0.0, -1.0) != 0) return false;
            if (Clamp(1.0, -1.0) != 1) return false;
            if (Clamp(2.0, -1.0) != 1) return false;
            if (Clamp(-1.0, 1.0) != -1) return false;
            if (Clamp(-2.0, 1.0) != -1) return false;
            if (Clamp(-1.0, -1.0) != -1) return false;
            if (Clamp(-2.0, -1.0) != -1) return false;

            return true;
        }
    }
}
