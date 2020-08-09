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

			Complex c1 = Complex.Sqrt(-a / d);
			Complex c2 = Complex.Sqrt(d / -a);
			Complex c3 = Complex.Sqrt(-a * d);
			Complex v = c1 * Complex.Tan(Complex.Atan(c2 * v0) + c3 * t);
			return v.Real;
		}

		public static double StopTime(double v0, double a = 0, double d = 0)
		{
			if (d < 0) throw new ArgumentOutOfRangeException("d must be greater than or equal to zero.");
			if (a >= 0) return double.PositiveInfinity;
			if (d == 0) return v0 / a;
			return Math.Atan(Math.Sqrt(d / -a) * v0) / Math.Sqrt(-a * d);
		}
	}
}
