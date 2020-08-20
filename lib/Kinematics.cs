using System;
using System.Numerics;

namespace Com.NarvinSingh.Physics
{
    public static class Kinematics
    {
        // (a - d * v^2)dt = dv
        // dt = dv / (a - d * v^2)
        // t = arctan(sqrt(d / -a) * [v = 0]) / sqrt(-a * d) - arctan(sqrt(d / -a) * v0) / sqrt(-a * d)
        // t = -arctan(sqrt(d / -a) * v0) / sqrt(-a * d)
        public static double TimeToStop(double v0, double a = 0, double d = 0)
        {
            if (d < 0) throw new ArgumentOutOfRangeException("d", "d must be greater than or equal to zero.");

            if (v0 == 0) return 0;

            int phase = (v0 >= 0 && a > 0) || (v0 <= 0 && a < 0) ? 1 : -1;

            // Drag and acceleration
            if (a != 0 && d != 0)
            {
                double absV0 = Math.Abs(v0);
                double absA = Math.Abs(a);

                // Accelaration, so return a negative time (in the past) when v was 0
                if (phase == 1)
                {
                    // If v >= terminal velocity, it would have been accelerating since forever
                    if (absV0 >= TerminalVelocity(absA, d)) return double.NegativeInfinity;

                    return -(Complex.Atan(Complex.Sqrt(d / -absA) * absV0) / Complex.Sqrt(-absA * d)).Real;
                }

                // Deceleration
                return Math.Atan(Math.Sqrt(d / absA) * absV0) / Math.Sqrt(absA * d);
            }

            // No drag
            return a != 0 ?
                    -v0 / a : // Acceleration
                    double.PositiveInfinity; // No acceleration, so v is constant and will never be 0
        }

        // dv = (a - d * v^2)dt
        // dv / (a - d * v^2) = dt
        // arctan(sqrt(d / -a) * v) / sqrt(-a * d) - arctan(sqrt(d / -a) * v0) / sqrt(-a * d) = t
        // arctan(sqrt(d / -a) * v) = arctan(sqrt(d / -a) * v0) + sqrt(-a * d)t
        // sqrt(d / -a) * v = tan(arctan(sqrt(d / -a) * v0) + sqrt(-a * d)t)
        // v = sqrt(-a / d) * tan(arctan(sqrt(d / -a) * v0) + sqrt(-a * d)t)
        public static double Velocity(double t, double v0 = 0, double a = 0, double d = 0)
        {

            if (d < 0) throw new ArgumentOutOfRangeException("d", "d must be greater than or equal to zero.");

            // No drag, only acceleration
            if (d == 0) return v0 + a * t;

            // No acceleration, only drag
            if (a == 0) return v0 >= 0 ? v0 / (d * v0 * t + 1) : v0 / (d * -v0 * t + 1);

            // Drag and acceleration
            int phase = (v0 >= 0 && a > 0) || (v0 <= 0 && a < 0) ? 1 : -1;
            double absV0 = Math.Abs(v0);
            double absA = Math.Abs(a);
            double absV2 = absV0;
            double t2 = t;
            int sign = v0 == 0 ?
                    (int)(-absA / a) :
                    v0 > 0 ? -phase : phase;

            // Deceleration
            if (phase == -1)
            {
                // Once v = 0, then decelration becomes aceleration for the remaining time
                double tStop = TimeToStop(v0, a, d);

                // Not enough time to stop, so only decelration
                if (tStop >= t)
                {
                    double r1 = Math.Sqrt(absA / d);
                    double r2 = Math.Sqrt(d / absA);
                    double r3 = Math.Sqrt(absA * d);
                    return sign * r1 * Math.Tan(Math.Atan(r2 * absV0) - r3 * t);
                }

                // Stopped, so fall through to acceleration with the remaining time
                absV2 = 0;
                t2 = t - tStop;
            }

            // Acceleration
            Complex c1 = Complex.Sqrt(-absA / d);
            Complex c2 = Complex.Sqrt(d / -absA);
            Complex c3 = Complex.Sqrt(-absA * d);
            return (sign * c1 * Complex.Tan(Complex.Atan(c2 * absV2) + c3 * t2)).Real;
        }

        // [dv = 0] = (a - d * v^2)dt
        // 0 = a - d * v^2
        // v = sqrt(a / d)
        public static double TerminalVelocity(double a, double d)
        {

            if (a <= 0) throw new ArgumentOutOfRangeException("a", "a must be greater than zero.");
            if (d <= 0) throw new ArgumentOutOfRangeException("d", "d must be greater than zero.");

            return Math.Sqrt(a / d);
        }
    }
}
