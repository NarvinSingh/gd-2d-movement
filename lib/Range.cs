using System;
using System.Runtime.CompilerServices;

namespace Com.NarvinSingh.Graphing
{
    public class Range
    {
        private float min;
        private float max;

        public Range(float min, float max)
        {
            Init(min, max);
        }

        public Range(float[] values)
        {
            if (values.Length == 0) throw new ArgumentException("values can't be empty.", "values");

            Init(values[0], values[0]);

            for (int i = values.Length - 1; i > 0; i--) Include(values[i]);
        }

        public float Min
        {
            get
            {
                return min;
            }
            set
            {
                if (value > Max) throw new ArgumentOutOfRangeException("Min", "Min must be less than or equal to Max");
                min = value;
            }
        }

        public float Max
        {
            get
            {
                return max;
            }
            set
            {
                if (value < Min)
                {
                    throw new ArgumentOutOfRangeException("Max", "Max must be greater than or equal to Min");
                }
                max = value;
            }
        }

        public float Size
        {
            get
            {
                return Max - Min;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Include(float value)
        {
            if (value < Min) Min = value;
            else if (value > Max) Max = value;
        }

        private void Init(float min, float max)
        {
            if (min > max) throw new ArgumentOutOfRangeException("min, max", "min must be less than or equal to max");

            this.min = min;
            this.max = max;
        }
    }
}
