using System;
using static Com.NarvinSingh.Test.Approximate;
using static System.Math;

namespace Com.NarvinSingh.UnitTest
{
    public static class ApproximateTest
    {
        public static bool ItCalculatesIsEqual()
        {
            if (!IsEqual(0, 0)) return false;

            if (!IsEqual(1, 1)) return false;
            if (!IsEqual(1.0001, 1.0002)) return false;
            if (IsEqual(1.001, 1.002)) return false;

            if (!IsEqual(-1, -1)) return false;
            if (!IsEqual(-1.0001, -1.0002)) return false;
            if (IsEqual(-1.001, -1.002)) return false;

            return true;
        }

        public static bool ItReimannSums()
        {
            if (ReimannSum((double xi, double sum) => 1, 0) != 0) return false;

            if (ReimannSum((double xi, double sum) => 1, 1, 0, 0, 0.25) != 1) return false;
            if (ReimannSum((double xi, double sum) => 1, 1, 0, 0, 0.3) != 1) return false;

            if (ReimannSum((double xi, double sum) => 1, 0, -1, 0, 0.25) != 1) return false;
            if (ReimannSum((double xi, double sum) => 1, 0, -1, 0, 0.3) != 1) return false;

            if (ReimannSum((double xi, double sum) => 1, 2, 1, 0, 0.25) != 1) return false;
            if (ReimannSum((double xi, double sum) => 1, 2, 1, 0, 0.3) != 1) return false;

            if (ReimannSum((double xi, double sum) => 1, -1, -2, 0, 0.25) != 1) return false;
            if (ReimannSum((double xi, double sum) => 1, -1, -2, 0, 0.3) != 1) return false;

            if (!IsEqual(ReimannSum((double xi, double sum) => xi, 1), 0.5)) return false;

            // let the function to sum be: dy = (1 + y^2)dx
            // rearranging: dy / (1 + y^2) = dx
            // integrating: arctan(y) - arctan(y0) = x - x0
            // rearranging: y = tan(x - x0 + arctan(y0))
            // for x0, y0 = 0, the exact solution is: y = tan(x)
            if (!IsEqual(ReimannSum((double xi, double sum) => 1 + sum * sum, 1), Tan(1))) return false;

            try
            {
                ReimannSum((double xi, double sum) => 1, 1, 0, 0, 0);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("dx must be greater than zero.")) return false;
            }

            try
            {
                ReimannSum((double xi, double sum) => 1, 1, 0, 0, -1);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("dx must be greater than zero.")) return false;
            }

            try
            {
                ReimannSum((double xi, double sum) => 1, -1);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("x0 must be less than x.")) return false;
            }

            try
            {
                ReimannSum((double xi, double sum) => 1, uint.MaxValue);
                return false;
            }
            catch (OverflowException)
            {
                // NOP
            }

            return true;
        }

        public static bool ItReimannSumsTo()
        {
            if (ReimannSumTo((double xi, double sum) => 1, 1) != 1) return false;
            if (!IsEqual(ReimannSumTo((double xi, double sum) => 1, 2), 2)) return false;

            if (ReimannSumTo((double xi, double sum) => 1, 1, 1, 0, 0, 0.25) != 1) return false;
            if (ReimannSumTo((double xi, double sum) => 1, 1, 1, 0, 0, 0.3) != 1) return false;

            if (ReimannSumTo((double xi, double sum) => 1, 1, 0, -1, 0, 0.25) != 0) return false;
            if (ReimannSumTo((double xi, double sum) => 1, 1, 0, -1, 0, 0.3) != 0) return false;

            if (ReimannSumTo((double xi, double sum) => 1, 1, 2, 1, 0, 0.25) != 2) return false;
            if (ReimannSumTo((double xi, double sum) => 1, 1, 2, 1, 0, 0.3) != 2) return false;

            if (ReimannSumTo((double xi, double sum) => 1, 1, -1, -2, 0, 0.25) != -1) return false;
            if (ReimannSumTo((double xi, double sum) => 1, 1, -1, -2, 0, 0.3) != -1) return false;

            if (!IsEqual(ReimannSumTo((double xi, double sum) => xi, 0.5), 1)) return false;

            // let the function to sum be: dy = (1 + y^2)dx
            // rearranging: dy / (1 + y^2) = dx
            // integrating: arctan(y) - arctan(y0) = x - x0
            // rearranging: y = tan(x - x0 + arctan(y0))
            // for x0, y0 = 0, the exact solution is: y = tan(x)
            if (!IsEqual(ReimannSumTo((double xi, double sum) => 1 + sum * sum, Tan(1)), 1)) return false;

            try
            {
                ReimannSumTo((double xi, double sum) => 1, 1, 1, 0, 0, 0);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("dx must be greater than zero.")) return false;
            }

            try
            {
                ReimannSumTo((double xi, double sum) => 1, 1, 1, 0, 0, -1);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("dx must be greater than zero.")) return false;
            }

            try
            {
                ReimannSumTo((double xi, double sum) => 1, 1, -1);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("x0 must be less than x.")) return false;
            }

            try
            {
                ReimannSumTo((double xi, double sum) => 1, 2, uint.MaxValue);
                return false;
            }
            catch (OverflowException)
            {
                // NOP
            }

            return true;
        }
    }
}
