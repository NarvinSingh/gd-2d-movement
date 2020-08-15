using System;

namespace Graphing
{
    public class Axis
    {
        private float length;
        private float lowerExtent;
        private float upperExtent;

        public Axis(float start, float length, float lowerExtent, float upperExtent, bool isInverted = false)
        {
            Init(start, length, lowerExtent, upperExtent, isInverted);
        }

        public Axis(float start, float length, float[] series, bool isInverted = false)
        {
            SeriesRange = GetRange(series);

            if (SeriesRange.Min != SeriesRange.Max)
            {
                Init(start, length, CalcLowerExtent(SeriesRange.Min), CalcUpperExtent(SeriesRange.Max), isInverted);
            }
            else if (SeriesRange.Min >= 0) Init(start, length, 0, SeriesRange.Max * 2, isInverted);
            else Init(start, length, SeriesRange.Min * 2, 0, isInverted);
        }

        public float Start { get; set; }
        
        public float Origin {
            get
            {
                if (!Inverted) return Start - LowerExtent * Step;
                return (Start + Length) + LowerExtent * Step;
            }
        }

        public float Length
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

        public Range SeriesRange { get; private set; }
       
        public bool Inverted { get; set; }

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
            if (!Inverted) return Start + (value - LowerExtent) * Step;
            return Start + Length - (value - LowerExtent) * Step;
        }

        public float Unmap(float coord)
        {
            if (!Inverted) return LowerExtent + (coord - Start) * InverseStep;
            return LowerExtent + (Start + Length - coord) * InverseStep;
        }

        public static float CalcLowerExtent(float minValue, int steps = 1)
        {
            return CalcExtent(minValue, true, steps);
        }

        public static float CalcUpperExtent(float maxValue, int steps = 1)
        {
            return CalcExtent(maxValue, false, steps);
        }

        private static float CalcExtent(float value, bool isMin, int steps = 1)
        {
            if (value == 0) return value; // log(0) is undefined so return the correct answer here

            double magnitude = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))));
            double factor = Math.Truncate(value / magnitude);
            double remainder = Math.Round((value - factor * magnitude) / magnitude, 3);

            if (isMin) return (float)((factor + Math.Floor(remainder * steps) / steps) * magnitude);
            return (float)((factor + Math.Ceiling(remainder * steps) / steps) * magnitude);
        }

        private void Init(float start, float length, float lowerExtent, float upperExtent, bool isInverted = false)
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
