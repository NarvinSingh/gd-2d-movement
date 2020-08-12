using System;
using System.Numerics;

namespace Physics
{
	public static class Kinematics
	{
		public static double Velocity(double v0, double t, double a = 0, double d = 0)
		{
			if (d < 0) throw new ArgumentOutOfRangeException("d must be greater than or equal to zero.");
			if (d == 0) return v0 + a * t;
			if (a == 0) return v0 / (d * v0 * t + 1);

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
				double tMax = Math.Min(t, TimeToStop(v0, a, d));
				double r1 = Math.Sqrt(absA / d);
				double r2 = Math.Sqrt(d / absA);
				double r3 = Math.Sqrt(absA * d);
				double v = sign * r1 * Math.Tan(Math.Atan(r2 * absV0) - r3 * tMax);

				if (tMax == t) return v;
				else
				{
					absV2 = 0;
					t2 = t - tMax;
				}
			}

			Complex c1 = Complex.Sqrt(-absA / d);
			Complex c2 = Complex.Sqrt(d / -absA);
			Complex c3 = Complex.Sqrt(-absA * d);
			return (sign * c1 * Complex.Tan(Complex.Atan(c2 * absV2) + c3 * t2)).Real;
		}

		public static double TimeToStop(double v0, double a = 0, double d = 0)
		{
			if (d < 0) throw new ArgumentOutOfRangeException("d must be greater than or equal to zero.");

			int phase = (v0 >= 0 && a > 0) || (v0 <= 0 && a < 0) ? 1 : -1;

			if (a != 0)
			{
				if (phase == 1) return double.PositiveInfinity;

				double absV0 = Math.Abs(v0);
				double absA = Math.Abs(a);
				return Math.Atan(Math.Sqrt(d / absA) * absV0) / Math.Sqrt(absA * d);
			}

			return v0 / a;
		}
	}
}
