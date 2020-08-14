using System;

namespace Graphing
{
    public static class Plot
    {
        public static MinMax GetMinMax(double[] values)
        {
            if (values.Length == 0) throw new ArgumentException("values can't be empty.");

            MinMax minMax = new MinMax(values[0], values[0]);

            foreach (double value in values)
            {
                if (value < minMax.Min) minMax.Min = value;
                else if (value > minMax.Max) minMax.Max = value;
            }

            return minMax;
        }

        public static MinMax GetMinMax(float[] values)
        {
            if (values.Length == 0) throw new ArgumentException("values can't be empty.");

            MinMax minMax = new MinMax(values[0], values[0]);

            foreach (float value in values)
            {
                if (value < minMax.Min) minMax.Min = value;
                else if (value > minMax.Max) minMax.Max = value;
            }

            return minMax;
        }

        public static double GetAxisLowerExtentQuarter(double x)
        {
            return GetAxisExtentQuarter(x, true);
        }

        public static double GetAxisUpperExtentQuarter(double x)
        {
            return GetAxisExtentQuarter(x, false);
        }

        public static double GetAxisLowerExtentHalf(double x)
        {
            return GetAxisExtentHalf(x, true);
        }

        public static double GetAxisUpperExtentHalf(double x)
        {
            return GetAxisExtentHalf(x, false);
        }

        public static double GetAxisLowerExtent(double x)
        {
            return GetAxisExtent(x, true);
        }

        public static double GetAxisUpperExtent(double x)
        {
            return GetAxisExtent(x, false);
        }

        public static double Translate(
                double value, double screenZero, double axisLength, double axisLowerExtent, double axisUpperExtent,
                bool isIncInverted)
        {
            if (axisLength < 0) throw new ArgumentOutOfRangeException("axisLength must be greater than zero.");
            if (axisLowerExtent >= axisUpperExtent)
            {
                throw new ArgumentOutOfRangeException("axisLowerExtent must be less than axisUpperExtent.");
            }

            int sign = isIncInverted ? -1 : 1;

            return screenZero + sign * (value - axisLowerExtent) * axisLength / (axisUpperExtent - axisLowerExtent);
        }

        public static double TranslateX(
                double value, double screenZero, double axisLength, double axisLowerExtent, double axisUpperExtent,
                bool isIncInverted = false)
        {
            return Translate(value, screenZero, axisLength, axisLowerExtent, axisUpperExtent, isIncInverted);
        }

        public static double TranslateY(
                double value, double screenZero, double axisLength, double axisLowerExtent, double axisUpperExtent,
                bool isIncInverted = true)
        {
            return Translate(value, screenZero, axisLength, axisLowerExtent, axisUpperExtent, isIncInverted);
        }

        public static double Untranslate(
                double value, double screenZero, double axisLength, double axisLowerExtent, double axisUpperExtent,
                bool isIncInverted)
        {
            if (axisLength < 0) throw new ArgumentOutOfRangeException("axisLength must be greater than zero.");
            if (axisLowerExtent >= axisUpperExtent)
            {
                throw new ArgumentOutOfRangeException("axisLowerExtent must be less than axisUpperExtent.");
            }

            int sign = isIncInverted ? -1 : 1;

            // x = o + s * (x0 - p) * l
            //         ----------------
            //               q - p
            // x0 = (x - o) * (q - p) + p
            //      -----------------
            //             s * l
            // return screenZero + sign * (value - axisLowerExtent) * axisLength / (axisUpperExtent - axisLowerExtent);
            return axisLowerExtent + sign * (value - screenZero) * (axisUpperExtent - axisLowerExtent) / axisLength;
        }

        public static double UntranslateX(
                double value, double screenZero, double axisLength, double axisLowerExtent, double axisUpperExtent,
                bool isIncInverted = false)
        {
            return Untranslate(value, screenZero, axisLength, axisLowerExtent, axisUpperExtent, isIncInverted);
        }

        public static double UntranslateY(
                double value, double screenZero, double axisLength, double axisLowerExtent, double axisUpperExtent,
                bool isIncInverted = true)
        {
            return Untranslate(value, screenZero, axisLength, axisLowerExtent, axisUpperExtent, isIncInverted);
        }

        private static double GetAxisExtentQuarter(double x, bool isLowerExtent)
        {
            if (x == 0) return x;

            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(x))));
            double factor = Math.Truncate(x / magnitude);
            double remainder = Math.Round((x - factor * magnitude) / magnitude, 3);

            if (remainder == 0) return x;

            if (isLowerExtent)
            {
                if (x >= 0)
                {
                    if (remainder < 0.25) return factor * magnitude;
                    if (remainder < 0.5) return (factor + 0.25) * magnitude;
                    if (remainder < 0.75) return (factor + 0.5) * magnitude;
                    return (factor + 0.75) * magnitude;
                }

                if (remainder >= -0.25) return (factor - 0.25) * magnitude;
                if (remainder >= -0.5) return (factor - 0.5) * magnitude;
                if (remainder >= -0.75) return (factor - 0.75) * magnitude;
                return (factor - 1) * magnitude;
            }

            if (x >= 0)
            {
                if (remainder <= 0.25) return (factor + 0.25) * magnitude;
                if (remainder <= 0.5) return (factor + 0.5) * magnitude;
                if (remainder <= 0.75) return (factor + 0.75) * magnitude;
                return (factor + 1) * magnitude;
            }

            if (remainder > -0.25) return factor * magnitude;
            if (remainder > -0.5) return (factor - 0.25) * magnitude;
            if (remainder > -0.75) return (factor - 0.5) * magnitude;
            return (factor - 0.75) * magnitude;
        }

        private static double GetAxisExtentHalf(double x, bool isLowerExtent)
        {
            if (x == 0) return x;

            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(x))));
            double factor = Math.Truncate(x / magnitude);
            double remainder = Math.Round((x - factor * magnitude) / magnitude, 3);

            if (remainder == 0) return x;

            if (isLowerExtent)
            {
                if (x >= 0)
                {
                    if (remainder < 0.5) return factor * magnitude;
                    return (factor + 0.5) * magnitude;
                }

                if (remainder >= -0.5) return (factor - 0.5) * magnitude;
                return (factor - 1) * magnitude;
            }

            if (x >= 0)
            {
                if (remainder <= 0.5) return (factor + 0.5) * magnitude;
                return (factor + 1) * magnitude;
            }

            if (remainder > -0.5) return factor * magnitude;
            return (factor - 0.5) * magnitude;
        }

        private static double GetAxisExtent(double x, bool isLowerExtent)
        {
            if (x == 0) return x;

            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(x))));
            double factor = Math.Truncate(x / magnitude);
            double remainder = Math.Round((x - factor * magnitude) / magnitude, 3);

            if (remainder == 0) return x;

            if (isLowerExtent)
            {
                if (x >= 0) return factor * magnitude;
                return (factor - 1) * magnitude;
            }

            if (x >= 0) return (factor + 1) * magnitude;
            return factor * magnitude;
        }

        public struct MinMax
        {
            public MinMax(double min, double max)
            {
                Min = min;
                Max = max;
            }

            public double Min { get; set; }
            public double Max { get; set; }
            public double Length
            {
                get
                {
                    return Max - Min;
                }
            }
        }
    }
}
