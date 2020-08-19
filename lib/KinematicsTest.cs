using System;
using static Com.NarvinSingh.Test.Approximate;
using static Com.NarvinSingh.Physics.Kinematics;

namespace Test
{
    public static class KinematicsTest
    {
        public static bool ItCalculatesTimeToStop()
        {
            if (TimeToStop(0, 0, 0) != 0) return false;
            if (TimeToStop(100, 0, 0) != double.PositiveInfinity) return false;

            if (TimeToStop(100, -100, 0) != 1) return false;
            if (TimeToStop(-100, 100, 0) != 1) return false;
            if (TimeToStop(100, 100, 0) != -1) return false;
            if (TimeToStop(-100, -100, 0) != -1) return false;

            if (!double.IsNaN(TimeToStop(100, 100, 0.01))) return false;
            if (!IsEqual(TimeToStop(50, 100, 0.01), -SumAccelerationTo(50, 1000, 0, 100, 0.01))) return false;
            if (!IsEqual(TimeToStop(100, -100, 0.01), SumAccelerationTo(0, 1000, 100, -100, 0.01))) return false;

            try
            {
                TimeToStop(0, 0, -0.01);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("d must be greater than or equal to zero.")) return false;
            }

            return true;
        }

        public static bool ItCalculatesVelocity()
        {
            if (Velocity(1, 0, 0, 0) != 0) return false;
            if (Velocity(1, 100, 0, 0) != 100) return false;
            if (Velocity(1, -100, 0, 0) != -100) return false;
            
            if (Velocity(1, 0, 100, 0) != 100) return false;
            if (Velocity(1, 0, -100, 0) != -100) return false;
            if (Velocity(1, 100, 100, 0) != 200) return false;
            if (Velocity(1, 100, -100, 0) != 0) return false;
            if (Velocity(1, -100, 100, 0) != 0) return false;
            if (Velocity(1, -100, -100, 0) != -200) return false;

            if (Velocity(1, 0, 0, 0.01) != 0) return false;
            if (Velocity(1, 100, 0, 0.01) != 50) return false;
            if (Velocity(1, -100, 0, 0.01) != -50) return false;

            if (!IsEqual(Velocity(1, 0, 100, 0.01), SumAcceleration(1, 0, 100, 0.01))) return false;
            if (!IsEqual(Velocity(1, 0, -100, 0.01), SumAcceleration(1, 0, -100, 0.01))) return false;
            if (!IsEqual(Velocity(1, 50, 100, 0.01), SumAcceleration(1, 50, 100, 0.01))) return false;
            if (!IsEqual(Velocity(1, -50, -100, 0.01), SumAcceleration(1, -50, -100, 0.01))) return false;
            if (!IsEqual(Velocity(1, 200, -100, 0.01), SumAcceleration(1, 200, -100, 0.01))) return false;
            if (!IsEqual(Velocity(1, -200, 100, 0.01), SumAcceleration(1, -200, 100, 0.01))) return false;
            if (!IsEqual(Velocity(1, 100, -100, 0.01), SumAcceleration(1, 100, -100, 0.01))) return false;
            if (!IsEqual(Velocity(1, -100, 100, 0.01), SumAcceleration(1, -100, 100, 0.01))) return false;

            try
            {
                Velocity(0, 1, 0, -0.01);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("d must be greater than or equal to zero.")) return false;
            }
            
            return true;
        }

        private static Fxy NetAcceleration(double a, double d)
        {
            return (double ti, double v) =>
            {
                return (v >= 0 && d >= 0) || (v < 0 && d < 0) ? a - d * v * v : a + d * v * v;
            };
        }

        private static double SumAccelerationTo(double v, double t, double v0, double a, double d)
        {
            return ReimannSumTo(NetAcceleration(a, d), v, t, 0, v0);
        }

        private static double SumAcceleration(double t, double v0, double a, double d)
        {
            return ReimannSum(NetAcceleration(a, d), t, 0, v0);
        }
    }
}
