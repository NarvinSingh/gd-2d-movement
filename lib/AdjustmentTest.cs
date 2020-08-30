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

        public static bool ItGetsPhase()
        {
            if (Phase(0, 0) != 0) return false;
            if (Phase(0, 1) != 0) return false;
            if (Phase(0, -1) != 0) return false;
            if (Phase(1, 0) != 1) return false;
            if (Phase(-1, 0) != 1) return false;
            if (Phase(1, 1) != 1) return false;
            if (Phase(-1, -1) != 1) return false;
            if (Phase(1, -1) != -1) return false;
            if (Phase(-1, 1) != -1) return false;
            if (Phase(0, int.MaxValue) != 0) return false;
            if (Phase(1, int.MaxValue) != 1) return false;
            if (Phase(-1, int.MaxValue) != -1) return false;
            if (Phase(int.MaxValue, 0) != 1) return false;
            if (Phase(0, int.MinValue) != 0) return false;
            if (Phase(1, int.MinValue) != -1) return false;
            if (Phase(-1, int.MinValue) != 1) return false;
            if (Phase(int.MinValue, 0) != 1) return false;
            if (Phase(int.MinValue, int.MaxValue) != -1) return false;
            if (Phase(int.MaxValue, int.MinValue) != -1) return false;

            if (Phase(0.0F, 0.0F) != 0) return false;
            if (Phase(0.0F, 1.0F) != 0) return false;
            if (Phase(0.0F, -.01F) != 0) return false;
            if (Phase(1.0F, 0.0F) != 1) return false;
            if (Phase(-1.0F, 0.0F) != 1) return false;
            if (Phase(1.0F, 1.0F) != 1) return false;
            if (Phase(-1.0F, -.01F) != 1) return false;
            if (Phase(1.0F, -.01F) != -1) return false;
            if (Phase(-1.0F, 1.0F) != -1) return false;
            if (Phase(0.0F, float.MaxValue) != 0) return false;
            if (Phase(1.0F, float.MaxValue) != 1) return false;
            if (Phase(-1.0F, float.MaxValue) != -1) return false;
            if (Phase(float.MaxValue, 0.0F) != 1) return false;
            if (Phase(0.0F, float.MinValue) != 0) return false;
            if (Phase(1.0F, float.MinValue) != -1) return false;
            if (Phase(-1.0F, float.MinValue) != 1) return false;
            if (Phase(float.MinValue, 0.0F) != 1) return false;
            if (Phase(float.MinValue, float.MaxValue) != -1) return false;
            if (Phase(float.MaxValue, float.MinValue) != -1) return false;
            if (Phase(0.0F, float.Epsilon) != 0) return false;
            if (Phase(1.0F, float.Epsilon) != 1) return false;
            if (Phase(-1.0F, float.Epsilon) != -1) return false;
            if (Phase(float.Epsilon, 0.0F) != 1) return false;
            if (Phase(0.0F, -float.Epsilon) != 0) return false;
            if (Phase(1.0F, -float.Epsilon) != -1) return false;
            if (Phase(-1.0F, -float.Epsilon) != 1) return false;
            if (Phase(-float.Epsilon, 0.0F) != 1) return false;
            if (Phase(-float.Epsilon, float.Epsilon) != -1) return false;
            if (Phase(float.Epsilon, -float.Epsilon) != -1) return false;
            if (Phase(0.0F, float.PositiveInfinity) != 0) return false;
            if (Phase(1.0F, float.PositiveInfinity) != 1) return false;
            if (Phase(-1.0F, float.PositiveInfinity) != -1) return false;
            if (Phase(float.PositiveInfinity, 0.0F) != 1) return false;
            if (Phase(0.0F, float.NegativeInfinity) != 0) return false;
            if (Phase(1.0F, float.NegativeInfinity) != -1) return false;
            if (Phase(-1.0F, float.NegativeInfinity) != 1) return false;
            if (Phase(float.NegativeInfinity, 0.0F) != 1) return false;
            if (Phase(float.NegativeInfinity, float.PositiveInfinity) != -1) return false;
            if (Phase(float.PositiveInfinity, float.NegativeInfinity) != -1) return false;

            if (Phase(0.0, 0.0) != 0) return false;
            if (Phase(0.0, 1.0) != 0) return false;
            if (Phase(0.0, -.01) != 0) return false;
            if (Phase(1.0, 0.0) != 1) return false;
            if (Phase(-1.0, 0.0) != 1) return false;
            if (Phase(1.0, 1.0) != 1) return false;
            if (Phase(-1.0, -.01) != 1) return false;
            if (Phase(1.0, -.01) != -1) return false;
            if (Phase(-1.0, 1.0) != -1) return false;
            if (Phase(0.0, double.MaxValue) != 0) return false;
            if (Phase(1.0, double.MaxValue) != 1) return false;
            if (Phase(-1.0, double.MaxValue) != -1) return false;
            if (Phase(double.MaxValue, 0.0) != 1) return false;
            if (Phase(0.0, double.MinValue) != 0) return false;
            if (Phase(1.0, double.MinValue) != -1) return false;
            if (Phase(-1.0, double.MinValue) != 1) return false;
            if (Phase(double.MinValue, 0.0) != 1) return false;
            if (Phase(double.MinValue, double.MaxValue) != -1) return false;
            if (Phase(double.MaxValue, double.MinValue) != -1) return false;
            if (Phase(0.0, double.Epsilon) != 0) return false;
            if (Phase(1.0, double.Epsilon) != 1) return false;
            if (Phase(-1.0, double.Epsilon) != -1) return false;
            if (Phase(double.Epsilon, 0.0) != 1) return false;
            if (Phase(0.0, -double.Epsilon) != 0) return false;
            if (Phase(1.0, -double.Epsilon) != -1) return false;
            if (Phase(-1.0, -double.Epsilon) != 1) return false;
            if (Phase(-double.Epsilon, 0.0) != 1) return false;
            if (Phase(-double.Epsilon, double.Epsilon) != -1) return false;
            if (Phase(double.Epsilon, -double.Epsilon) != -1) return false;
            if (Phase(0.0, double.PositiveInfinity) != 0) return false;
            if (Phase(1.0, double.PositiveInfinity) != 1) return false;
            if (Phase(-1.0, double.PositiveInfinity) != -1) return false;
            if (Phase(double.PositiveInfinity, 0.0) != 1) return false;
            if (Phase(0.0, double.NegativeInfinity) != 0) return false;
            if (Phase(1.0, double.NegativeInfinity) != -1) return false;
            if (Phase(-1.0, double.NegativeInfinity) != 1) return false;
            if (Phase(double.NegativeInfinity, 0.0) != 1) return false;
            if (Phase(double.NegativeInfinity, double.PositiveInfinity) != -1) return false;
            if (Phase(double.PositiveInfinity, double.NegativeInfinity) != -1) return false;

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
