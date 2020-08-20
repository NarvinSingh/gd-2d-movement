using System;
using static System.Math;

namespace Com.NarvinSingh.Test
{
    public static class Approximate
    {
        public delegate double Fxy(double xi, double sum);
        private delegate bool IsSumReached();

        public static bool IsEqual(double a, double b, double tolerance = 0.0001)
        {
            double delta = a - b;
            return delta >= 0 ? delta < tolerance : -delta < tolerance;
        }

        public static double ReimannSum(Fxy f, double x, double x0 = 0, double y0 = 0, double dx = 1e-6)
        {
            if (dx <= 0) throw new ArgumentOutOfRangeException("dx", "dx must be greater than zero.");
            if (x0 > x) throw new ArgumentOutOfRangeException("x0, x", "x0 must be less than x.");

            double intervalSize = Abs(x - x0);
            uint n = checked((uint)(intervalSize / dx));
            double sum = y0;

            for (uint i = 0; i < n; i++) sum += f(x0 + i * dx, sum) * dx;

            double summedIntervalSize = n * dx;

            if (summedIntervalSize < intervalSize)
            {
                double lastDx = intervalSize - summedIntervalSize;
                sum += f(x - lastDx, sum) * lastDx;
            }

            return sum;
        }

        public static double ReimannSumTo(
                Fxy f, double y, double x = 1000, double x0 = 0, double y0 = 0, double dx = 1e-6)
        {
            if (dx <= 0) throw new ArgumentOutOfRangeException("dx", "dx must be greater than zero.");
            if (x0 > x) throw new ArgumentOutOfRangeException("x0, x", "x0 must be less than x.");

            if (y == y0) return x0;

            double intervalSize = Abs(x - x0);
            uint n = checked((uint)(intervalSize / dx));
            double sum = y0;
            IsSumReached isSumReached = y > y0 ? (IsSumReached)(() => sum >= y) : () => sum <= y;

            for (uint i = 0; i < n; i++)
            {
                sum += f(x0 + i * dx, sum) * dx;
                if (isSumReached()) return x0 + (i + 1) * dx;
            }

            double summedIntervalSize = n * dx;

            if (summedIntervalSize < intervalSize)
            {
                double lastDx = intervalSize - summedIntervalSize;
                sum += f(x - lastDx, sum) * lastDx;
                if (isSumReached()) return x;
            }

            return double.NaN;
        }
    }
}
