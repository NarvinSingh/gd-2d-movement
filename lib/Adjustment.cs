﻿namespace Com.NarvinSingh.Utility
{
    public static class Adjustment
    {
        public static int Sign(int value)
        {
            return value >= 0 ? 1 : -1;
        }

        public static int Sign(float value)
        {
            return value >= 0 ? 1 : -1;
        }

        public static int Sign(double value)
        {
            return value >= 0 ? 1 : -1;
        }

        public static int Phase(int a, int b)
        {
            if (a == 0) return 0;
            if ((a > 0 && b >= 0) || (a < 0 && b <= 0)) return 1;
            return -1;
        }

        public static int Phase(float a, float b)
        {
            if (a == 0) return 0;
            if ((a > 0 && b >= 0) || (a < 0 && b <= 0)) return 1;
            return -1;
        }

        public static int Phase(double a, double b)
        {
            if (a == 0) return 0;
            if ((a > 0 && b >= 0) || (a < 0 && b <= 0)) return 1;
            return -1;
        }

        public static int Absolute(int value)
        {
            return value >= 0 ? value : -value;
        }

        public static float Absolute(float value)
        {
            return value >= 0 ? value : -value;
        }

        public static double Absolute(double value)
        {
            return value >= 0 ? value : -value;
        }

        public static int Clamp(int value, int maxValue)
        {
            int absMaxValue = Absolute(maxValue);
            if (value >= 0) return value < absMaxValue ? value : absMaxValue;
            return value > -absMaxValue ? value : -absMaxValue;
        }

        public static float Clamp(float value, float maxValue)
        {
            float absMaxValue = Absolute(maxValue);
            if (value >= 0) return value < absMaxValue ? value : absMaxValue;
            return value > -absMaxValue ? value : -absMaxValue;
        }

        public static double Clamp(double value, double maxValue)
        {
            double absMaxValue = Absolute(maxValue);
            if (value >= 0) return value < absMaxValue ? value : absMaxValue;
            return value > -absMaxValue ? value : -absMaxValue;
        }
    }
}
