﻿using System;
using static System.Math;

namespace Com.NarvinSingh.Graphing
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
            SeriesRange = new Range(series);

            if (SeriesRange.Min != SeriesRange.Max)
            {
                Init(start, length, CalcLowerExtent(SeriesRange.Min), CalcUpperExtent(SeriesRange.Max), isInverted);
            }
            else if (SeriesRange.Min >= 0) Init(start, length, 0, SeriesRange.Max * 2, isInverted);
            else Init(start, length, SeriesRange.Min * 2, 0, isInverted);
        }

        public float Start { get; set; }

        public float Origin
        {
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
                if (value <= 0) throw new ArgumentOutOfRangeException("Length", "Length must be greater than zero.");
                length = value;
            }
        }

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
