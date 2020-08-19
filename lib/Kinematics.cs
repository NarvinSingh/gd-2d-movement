using System;
using System.Numerics;

namespace Com.NarvinSingh.Physics
{
	public static class Kinematics
	{
		public static double TimeToStop(double v0, double a = 0, double d = 0)
		{
			if (d < 0) throw new ArgumentOutOfRangeException("d", "d must be greater than or equal to zero.");

			if (v0 == 0) return 0;

			int phase = (v0 >= 0 && a > 0) || (v0 <= 0 && a < 0) ? 1 : -1;

			if (a != 0 && d != 0)
			{
	
				if (phase == 1) return double.PositiveInfinity;

				double absV0 = Math.Abs(v0);
				double absA = Math.Abs(a);
				return Math.Atan(Math.Sqrt(d / absA) * absV0) / Math.Sqrt(absA * d);
			}

			return a != 0 ? -v0 / a : double.PositiveInfinity;
		}

		public static double Velocity(double t, double v0 = 0, double a = 0, double d = 0)
		{
			if (d < 0) throw new ArgumentOutOfRangeException("d", "d must be greater than or equal to zero.");
			if (d == 0) return v0 + a * t;
			if (a == 0) return v0 >= 0 ? v0 / (d * v0 * t + 1) : v0 / (d * (-v0) * t + 1);

			int phase = (v0 >= 0 && a > 0) || (v0 <= 0 && a < 0) ? 1 : -1;
			double absV0 = Math.Abs(v0);
			double absA = Math.Abs(a);
			double absV2 = absV0;
			double t2 = t;
			int sign = v0 == 0 ?
					(int)(-absA / a) :
					v0 > 0 ? -phase : phase;

			if (phase == -1)
			{
				double tStop = TimeToStop(v0, a, d);

				if (tStop >= t)
				{
					double r1 = Math.Sqrt(absA / d);
					double r2 = Math.Sqrt(d / absA);
					double r3 = Math.Sqrt(absA * d);
					return sign * r1 * Math.Tan(Math.Atan(r2 * absV0) - r3 * t);
				}

				absV2 = 0;
				t2 = t - tStop;
			}

			Complex c1 = Complex.Sqrt(-absA / d);
			Complex c2 = Complex.Sqrt(d / -absA);
			Complex c3 = Complex.Sqrt(-absA * d);
			return (sign * c1 * Complex.Tan(Complex.Atan(c2 * absV2) + c3 * t2)).Real;
		}
	}
}
