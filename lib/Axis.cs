using System;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Com.NarvinSingh.Graphing
{
    public class Axis
    {
        private float length;
        private float lowerExtent;
        private float upperExtent;

        // Specify the extents
        public Axis(float start, float length, float lowerExtent, float upperExtent, bool isInverted = false)
        {
            Init(start, length, lowerExtent, upperExtent, isInverted);
        }

        // Calculate the extents based on the series
        public Axis(float start, float length, float[] series, bool isInverted = false)
        {
            Range range = new Range(series);

            // Non-constant series, so calculate the extents based on the min and max of the series
            if (range.Min != range.Max)
            {
                Init(start, length, CalcLowerExtent(range.Min), CalcUpperExtent(range.Max), isInverted);
            }
            // Non-negative constant series, so calculate the extents so the constant is above 0 and in the middle of
            // the range 
            else if (range.Min >= 0) Init(start, length, 0, range.Max * 2, isInverted);
            // Negative constant series, so calculate the extents so the constant is below 0 and in the middle of the
            // range
            else Init(start, length, range.Min * 2, 0, isInverted);
        }

        // Left or top end of the axis in screen coordinates 
        public float Start { get; set; }

        // 0 on the axis in screen coordinates
        public float Origin
        {
            get
            {
                if (!Inverted) return Start - LowerExtent * UnitLength;
                return (Start + Length) + LowerExtent * UnitLength;
            }
        }

        // Length of the axis in screen coordinates
        public float Length
        {
            get
            {
                return length;
            }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Length", "Length must be greater than zero.");
                length = value;
            }
        }

        // Value in axis units of the minimum end of the axis
        public float LowerExtent
        {
            get
            {
                return lowerExtent;
            }
            set
            {
                if (value >= upperExtent)
                {
                    throw new ArgumentOutOfRangeException("LowerExtent", "LowerExtent must be less than UpperExtent.");
                }
                lowerExtent = value;
            }
        }

        // Value in axis units of the maximum end of the axis
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
                    throw new ArgumentOutOfRangeException("UpperExtent", "UpperExtent must be less than LowerExtent.");
                }
                upperExtent = value;
            }
        }

        // True if the left or top end of the axis is the upper extent
        public bool Inverted { get; set; }

        // Unit distance along the axis in screen units
        public float UnitLength
        {
            get
            {
                return Length / (UpperExtent - LowerExtent);
            }
        }

        // Unit distance on the screen in axis units
        public float InverseUnitLength
        {
            get
            {
                return (UpperExtent - LowerExtent) / Length;
            }
        }

        // Return a screen coordinate given a value on the axis
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Map(float value)
        {
            if (!Inverted) return Start + (value - LowerExtent) * UnitLength;
            return Start + Length - (value - LowerExtent) * UnitLength;
        }

        // Return a value on the axis given a screen coordinate
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float Unmap(float coord)
        {
            if (!Inverted) return LowerExtent + (coord - Start) * InverseUnitLength;
            return LowerExtent + (Start + Length - coord) * InverseUnitLength;
        }

        public static float CalcLowerExtent(float minValue, int steps = 1)
        {
            return CalcExtent(minValue, true, steps);
        }

        public static float CalcUpperExtent(float maxValue, int steps = 1)
        {
            return CalcExtent(maxValue, false, steps);
        }

        // To calculate the extent:
        //   1) Get the magnitude of the extema (power of 10), e.g., 0.11 -> 0.1, 2.3 -> 1, 56 -> 10
        //   2) Multiply the magnitude by the first non-zero digit of the extema, e.g., 0.11 -> 0.1, 2.3 -> 2, 56 -> 50
        //   3) Round up/down for a max/min in discrete steps, e.g., 1 step rounds between {0,  1}, 2 steps rounds
        //      between {0, 0.5, 1}, 4 steps rounds between {0, 0.25, 0.5, 0.75, 1}, so using 4 steps we would get for a
        //      maxima 0.11 -> 0.125, 2.3 -> 2.5, 56 -> 57.5, and for a minima 0.11 -> 0.1, 2.3 -> 2.25, 56 -> 55
        private static float CalcExtent(float value, bool isMin, int steps = 1)
        {
            if (value == 0) return value; // log(0) is undefined so return the correct answer here

            double magnitude = Pow(10, Floor(Log10(Abs(value))));
            double factor = Truncate(value / magnitude);
            double remainder = Round((value - factor * magnitude) / magnitude, 3);

            if (isMin) return (float)((factor + Floor(remainder * steps) / steps) * magnitude);
            return (float)((factor + Ceiling(remainder * steps) / steps) * magnitude);
        }

        private void Init(float start, float length, float lowerExtent, float upperExtent, bool isInverted = false)
        {
            if (upperExtent <= lowerExtent)
            {
                throw new ArgumentOutOfRangeException("lowerExtent, upperExtent",
                        "lowerExtent must be less than upperExtent.");
            }
            Start = start;
            Length = length;

            // Set the backing field to avoid the range check since the upper extent hasn't been set yet
            this.lowerExtent = lowerExtent;
            UpperExtent = upperExtent;

            Inverted = isInverted;
        }
    }
}
