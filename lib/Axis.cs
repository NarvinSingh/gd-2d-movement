using System;

namespace Graphing
{
    public class Axis
    {
        private int length;
        private float lowerExtent;
        private float upperExtent;
        private int sign;

        public Axis(int start, int length, float lowerExtent, float upperExtent, bool isInverted = false)
        {
            Init(start, length, lowerExtent, upperExtent, isInverted);
        }

        public Axis(int start, int length, float[] values, bool isInverted = false)
        {
            Range range = GetRange(values);

            if (range.Min != range.Max)
            {
                Init(start, length, CalcLowerExtent(range.Min), CalcUpperExtent(range.Max), isInverted);
            }
            else if (range.Min >= 0) Init(start, length, 0, range.Max * 2, isInverted);
            else Init(start, length, range.Min * 2, 0, isInverted);
        }

        public int Start { get; set; }
        public float Origin {
            get
            {
                return Start - LowerExtent * Step;
            }
        }

        public int Length
        {
            get
            {
                return length;
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Length must be greater than zero.");
                length = value;
            }
        }

        public float LowerExtent
        {
            get
            {
                return lowerExtent;
            }
            set {
                if (value >= upperExtent)
                {
                    throw new ArgumentOutOfRangeException("LowerExtent must be less than UpperExtent.");
                }
                lowerExtent = value;
            }
        }

        public float UpperExtent
        {
            get
            {
                return upperExtent;
            }
            set
            {
                if (value <= lowerExtent)
                {
                    throw new ArgumentOutOfRangeException("UpperExtent must be less than LowerExtent.");
                }
                upperExtent = value;
            }
        }

        public bool Inverted
        {
            get
            {
                return sign != 1 ? true : false;
            }
            set
            {
                sign = value ? -1 : 1;
            }
        }

        public float Step
        {
            get
            {
                return Length / (UpperExtent - LowerExtent);
            }
        }

        public float InverseStep
        {
            get
            {
                return (UpperExtent - LowerExtent) / Length;
            }
        }

        public static Range GetRange(float[] values)
        {
            if (values.Length == 0) throw new ArgumentException("values can't be empty.");

            Range range = new Range(values[0], values[0]);

            foreach (float value in values)
            {
                if (value < range.Min) range.Min = value;
                else if (value > range.Max) range.Max = value;
            }

            return range;
        }

        public float Map(float value)
        {
            return Start + sign * (value - LowerExtent) * Step;
        }

        public float Unmap(float coord)
        {
            return LowerExtent + sign * (coord - Start) * InverseStep;
        }

        public static float CalcLowerExtent(float minValue, int steps = 1)
        {
            return CalcExtent(minValue, true, steps);
        }

        public static float CalcUpperExtent(float maxValue, int steps = 1)
        {
            return CalcExtent(maxValue, false, steps);
        }

        private static float CalcExtent(float value, bool isMin)
        {
            if (value == 0) return value;

            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))));
            double factor = Math.Truncate(value / magnitude);
            double remainder = Math.Round((value - factor * magnitude) / magnitude, 3);

            if (remainder == 0) return value;
            if (value >= 0) return (float)((factor + Convert.ToInt32(isMin)) * magnitude);
            return (float)((factor - Convert.ToInt32(isMin)) * magnitude);
        }

        private static float CalcExtent(float value, bool isMin, int steps = 1)
        {
            if (value == 0) return value; // log(0) is undefined so return the correct answer here

            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))));
            double factor = Math.Truncate(value / magnitude);
            double remainder = Math.Round((value - factor * magnitude) / magnitude, 3);

            if (isMin)
            {
                float result = (float)((factor + Math.Floor(remainder * steps) / steps) * magnitude);
                return result;
            }
            return (float)((factor + Math.Ceiling(remainder * steps) / steps) * magnitude);
        }

        private void Init(int start, int length, float lowerExtent, float upperExtent, bool isInverted = false)
        {
            if (upperExtent <= lowerExtent)
            {
                throw new ArgumentOutOfRangeException("upperExtent must be greatet than lowerExtent.");
            }
            Start = start;
            Length = length;

            // Set the backing field to avoid the range check since the upper extent hasn't been set yet
            this.lowerExtent = lowerExtent;
            UpperExtent = upperExtent;

            Inverted = isInverted;
        }

        public struct Range
        {
            public Range(float min, float max)
            {
                Min = min;
                Max = max;
            }

            public float Min { get; set; }
            public float Max { get; set; }
            public float Size
            {
                get
                {
                    return Math.Abs(Max - Min);
                }
            }
        }
    }
}
