using Graphing;
using System;

namespace Test
{
    public static class RangeTest
    {
        public static bool ItInstantiates()
        {
            Range range = new Range(0, 0);
            if (range.Min != 0) return false;
            if (range.Max != 0) return false;
            if (range.Size != 0) return false;

            Range range2 = new Range(new float[] { 1, 2, 3, 4, 5 });
            if (range2.Min != 1) return false;
            if (range2.Max != 5) return false;
            if (range2.Size != 4) return false;

            Range range3 = new Range(new float[] { -1, -2, -3, -4, -5 });
            if (range3.Min != -5) return false;
            if (range3.Max != -1) return false;
            if (range3.Size != 4) return false;

            Range range4 = new Range(new float[] { 5, -1, -3, 4, 2 });
            if (range4.Min != -3) return false;
            if (range4.Max != 5) return false;
            if (range4.Size != 8) return false;

            Range range5 = new Range(new float[] { 0 });
            if (range5.Min != 0) return false;
            if (range5.Max != 0) return false;
            if (range5.Size != 0) return false;

            Range range6 = new Range(new float[] { 0, 0, 0 });
            if (range6.Min != 0) return false;
            if (range6.Max != 0) return false;
            if (range6.Size != 0) return false;

            Range range7 = new Range(new float[] { 5, 5, 5 });
            if (range7.Min != 5) return false;
            if (range7.Max != 5) return false;
            if (range7.Size != 0) return false;

            Range range8 = new Range(new float[] { -5, -5, -5 });
            if (range8.Min != -5) return false;
            if (range8.Max != -5) return false;
            if (range8.Size != 0) return false;

            try
            {
                Range range9 = new Range(1, -1);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("min must be less than or equal to max")) return false;
            }

            try
            {
                Range range10 = new Range(new float[] {});
                return false;
            }
            catch (ArgumentException e)
            {
                if (!e.Message.StartsWith("values can't be empty.")) return false;
            }

            try
            {
                range.Min = 1;
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("Min must be less than or equal to Max")) return false;
            }

            try
            {
                range.Max = -1;
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("Max must be greater than or equal to Min")) return false;
            }

            return true;
        }

        public static bool ItIncludes()
        {
            Range range = new Range(0, 0);
            range.Include(1);
            range.Include(-1);
            if (range.Min != -1) return false;
            if (range.Max != 1) return false;
            if (range.Size != 2) return false;
            return true;
        }
    }
}
