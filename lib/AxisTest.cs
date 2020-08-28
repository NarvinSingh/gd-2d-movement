using Com.NarvinSingh.Graphing;
using System;
using static Com.NarvinSingh.Graphing.Axis;
using static System.Math;

namespace Com.NarvinSingh.UnitTest
{
    public static class AxisTest
    {
        public static bool ItCalcsExtents()
        {
            if (Round(CalcLowerExtent(0F), 3) != 0) return false;
            if (Round(CalcLowerExtent(0.1F), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.11F), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.125F), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.13F), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.15F), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.16F), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.175F), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.18F), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.2F), 3) != 0.2) return false;
            if (Round(CalcLowerExtent(1F), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.1F), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.25F), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.3F), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.5F), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.6F), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.75F), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.8F), 3) != 1) return false;
            if (Round(CalcLowerExtent(2F), 3) != 2) return false;
            if (Round(CalcLowerExtent(10F), 3) != 10) return false;
            if (Round(CalcLowerExtent(11F), 3) != 10) return false;
            if (Round(CalcLowerExtent(12.5F), 3) != 10) return false;
            if (Round(CalcLowerExtent(13F), 3) != 10) return false;
            if (Round(CalcLowerExtent(15F), 3) != 10) return false;
            if (Round(CalcLowerExtent(16F), 3) != 10) return false;
            if (Round(CalcLowerExtent(17.5F), 3) != 10) return false;
            if (Round(CalcLowerExtent(18F), 3) != 10) return false;
            if (Round(CalcLowerExtent(20F), 3) != 20) return false;
            if (Round(CalcLowerExtent(-0.1F), 3) != -0.1) return false;
            if (Round(CalcLowerExtent(-0.11F), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.125F), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.13F), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.15F), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.16F), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.175F), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.18F), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.2F), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-1F), 3) != -1) return false;
            if (Round(CalcLowerExtent(-1.1F), 3) != -2) return false;
            if (Round(CalcLowerExtent(-1.25F), 3) != -2) return false;
            if (Round(CalcLowerExtent(-1.3F), 3) != -2) return false;
            if (Round(CalcLowerExtent(-1.5F), 3) != -2) return false;
            if (Round(CalcLowerExtent(-1.6F), 3) != -2) return false;
            if (Round(CalcLowerExtent(-1.75F), 3) != -2) return false;
            if (Round(CalcLowerExtent(-1.8F), 3) != -2) return false;
            if (Round(CalcLowerExtent(-2F), 3) != -2) return false;
            if (Round(CalcLowerExtent(-10F), 3) != -10) return false;
            if (Round(CalcLowerExtent(-11F), 3) != -20) return false;
            if (Round(CalcLowerExtent(-12.5F), 3) != -20) return false;
            if (Round(CalcLowerExtent(-13F), 3) != -20) return false;
            if (Round(CalcLowerExtent(-15F), 3) != -20) return false;
            if (Round(CalcLowerExtent(-16F), 3) != -20) return false;
            if (Round(CalcLowerExtent(-17.5F), 3) != -20) return false;
            if (Round(CalcLowerExtent(-18F), 3) != -20) return false;
            if (Round(CalcLowerExtent(-20F), 3) != -20) return false;

            if (Round(CalcUpperExtent(0F), 3) != 0) return false;
            if (Round(CalcUpperExtent(0.1F), 3) != 0.1) return false;
            if (Round(CalcUpperExtent(0.11F), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.125F), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.13F), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.15F), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.16F), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.175F), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.18F), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.2F), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(1F), 3) != 1) return false;
            if (Round(CalcUpperExtent(1.1F), 3) != 2) return false;
            if (Round(CalcUpperExtent(1.25F), 3) != 2) return false;
            if (Round(CalcUpperExtent(1.3F), 3) != 2) return false;
            if (Round(CalcUpperExtent(1.5F), 3) != 2) return false;
            if (Round(CalcUpperExtent(1.6F), 3) != 2) return false;
            if (Round(CalcUpperExtent(1.75F), 3) != 2) return false;
            if (Round(CalcUpperExtent(1.8F), 3) != 2) return false;
            if (Round(CalcUpperExtent(2F), 3) != 2) return false;
            if (Round(CalcUpperExtent(10F), 3) != 10) return false;
            if (Round(CalcUpperExtent(11F), 3) != 20) return false;
            if (Round(CalcUpperExtent(12.5F), 3) != 20) return false;
            if (Round(CalcUpperExtent(13F), 3) != 20) return false;
            if (Round(CalcUpperExtent(15F), 3) != 20) return false;
            if (Round(CalcUpperExtent(16F), 3) != 20) return false;
            if (Round(CalcUpperExtent(17.5F), 3) != 20) return false;
            if (Round(CalcUpperExtent(18F), 3) != 20) return false;
            if (Round(CalcUpperExtent(20F), 3) != 20) return false;
            if (Round(CalcUpperExtent(-0.1F), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.11F), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.125F), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.13F), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.15F), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.16F), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.175F), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.18F), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.2F), 3) != -0.2) return false;
            if (Round(CalcUpperExtent(-1F), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.1F), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.25F), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.3F), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.5F), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.6F), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.75F), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.8F), 3) != -1) return false;
            if (Round(CalcUpperExtent(-2F), 3) != -2) return false;
            if (Round(CalcUpperExtent(-10F), 3) != -10) return false;
            if (Round(CalcUpperExtent(-11F), 3) != -10) return false;
            if (Round(CalcUpperExtent(-12.5F), 3) != -10) return false;
            if (Round(CalcUpperExtent(-13F), 3) != -10) return false;
            if (Round(CalcUpperExtent(-15F), 3) != -10) return false;
            if (Round(CalcUpperExtent(-16F), 3) != -10) return false;
            if (Round(CalcUpperExtent(-17.5F), 3) != -10) return false;
            if (Round(CalcUpperExtent(-18F), 3) != -10) return false;
            if (Round(CalcUpperExtent(-20F), 3) != -20) return false;

            if (Round(CalcLowerExtent(0F, 2), 3) != 0) return false;
            if (Round(CalcLowerExtent(0.1F, 2), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.11F, 2), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.125F, 2), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.13F, 2), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.15F, 2), 3) != 0.15) return false;
            if (Round(CalcLowerExtent(0.16F, 2), 3) != 0.15) return false;
            if (Round(CalcLowerExtent(0.175F, 2), 3) != 0.15) return false;
            if (Round(CalcLowerExtent(0.18F, 2), 3) != 0.15) return false;
            if (Round(CalcLowerExtent(0.2F, 2), 3) != 0.2) return false;
            if (Round(CalcLowerExtent(1F, 2), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.1F, 2), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.25F, 2), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.3F, 2), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.5F, 2), 3) != 1.5) return false;
            if (Round(CalcLowerExtent(1.6F, 2), 3) != 1.5) return false;
            if (Round(CalcLowerExtent(1.75F, 2), 3) != 1.5) return false;
            if (Round(CalcLowerExtent(1.8F, 2), 3) != 1.5) return false;
            if (Round(CalcLowerExtent(2F, 2), 3) != 2) return false;
            if (Round(CalcLowerExtent(10F, 2), 3) != 10) return false;
            if (Round(CalcLowerExtent(11F, 2), 3) != 10) return false;
            if (Round(CalcLowerExtent(12.5F, 2), 3) != 10) return false;
            if (Round(CalcLowerExtent(13F, 2), 3) != 10) return false;
            if (Round(CalcLowerExtent(15F, 2), 3) != 15) return false;
            if (Round(CalcLowerExtent(16F, 2), 3) != 15) return false;
            if (Round(CalcLowerExtent(17.5F, 2), 3) != 15) return false;
            if (Round(CalcLowerExtent(18F, 2), 3) != 15) return false;
            if (Round(CalcLowerExtent(20F, 2), 3) != 20) return false;
            if (Round(CalcLowerExtent(-0.1F, 2), 3) != -0.1) return false;
            if (Round(CalcLowerExtent(-0.11F, 2), 3) != -0.15) return false;
            if (Round(CalcLowerExtent(-0.125F, 2), 3) != -0.15) return false;
            if (Round(CalcLowerExtent(-0.13F, 2), 3) != -0.15) return false;
            if (Round(CalcLowerExtent(-0.15F, 2), 3) != -0.15) return false;
            if (Round(CalcLowerExtent(-0.16F, 2), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.175F, 2), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.18F, 2), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.2F, 2), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-1F, 2), 3) != -1) return false;
            if (Round(CalcLowerExtent(-1.1F, 2), 3) != -1.5) return false;
            if (Round(CalcLowerExtent(-1.25F, 2), 3) != -1.5) return false;
            if (Round(CalcLowerExtent(-1.3F, 2), 3) != -1.5) return false;
            if (Round(CalcLowerExtent(-1.5F, 2), 3) != -1.5) return false;
            if (Round(CalcLowerExtent(-1.6F, 2), 3) != -2) return false;
            if (Round(CalcLowerExtent(-1.75F, 2), 3) != -2) return false;
            if (Round(CalcLowerExtent(-1.8F, 2), 3) != -2) return false;
            if (Round(CalcLowerExtent(-2F, 2), 3) != -2) return false;
            if (Round(CalcLowerExtent(-10F, 2), 3) != -10) return false;
            if (Round(CalcLowerExtent(-11F, 2), 3) != -15) return false;
            if (Round(CalcLowerExtent(-12.5F, 2), 3) != -15) return false;
            if (Round(CalcLowerExtent(-13F, 2), 3) != -15) return false;
            if (Round(CalcLowerExtent(-15F, 2), 3) != -15) return false;
            if (Round(CalcLowerExtent(-16F, 2), 3) != -20) return false;
            if (Round(CalcLowerExtent(-17.5F, 2), 3) != -20) return false;
            if (Round(CalcLowerExtent(-18F, 2), 3) != -20) return false;
            if (Round(CalcLowerExtent(-20F, 2), 3) != -20) return false;

            if (Round(CalcUpperExtent(0F, 2), 3) != 0) return false;
            if (Round(CalcUpperExtent(0.1F, 2), 3) != 0.1) return false;
            if (Round(CalcUpperExtent(0.11F, 2), 3) != 0.15) return false;
            if (Round(CalcUpperExtent(0.125F, 2), 3) != 0.15) return false;
            if (Round(CalcUpperExtent(0.13F, 2), 3) != 0.15) return false;
            if (Round(CalcUpperExtent(0.15F, 2), 3) != 0.15) return false;
            if (Round(CalcUpperExtent(0.16F, 2), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.175F, 2), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.18F, 2), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.2F, 2), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(1F, 2), 3) != 1) return false;
            if (Round(CalcUpperExtent(1.1F, 2), 3) != 1.5) return false;
            if (Round(CalcUpperExtent(1.25F, 2), 3) != 1.5) return false;
            if (Round(CalcUpperExtent(1.3F, 2), 3) != 1.5) return false;
            if (Round(CalcUpperExtent(1.5F, 2), 3) != 1.5) return false;
            if (Round(CalcUpperExtent(1.6F, 2), 3) != 2) return false;
            if (Round(CalcUpperExtent(1.75F, 2), 3) != 2) return false;
            if (Round(CalcUpperExtent(1.8F, 2), 3) != 2) return false;
            if (Round(CalcUpperExtent(2F, 2), 3) != 2) return false;
            if (Round(CalcUpperExtent(10F, 2), 3) != 10) return false;
            if (Round(CalcUpperExtent(11F, 2), 3) != 15) return false;
            if (Round(CalcUpperExtent(12.5F, 2), 3) != 15) return false;
            if (Round(CalcUpperExtent(13F, 2), 3) != 15) return false;
            if (Round(CalcUpperExtent(15F, 2), 3) != 15) return false;
            if (Round(CalcUpperExtent(16F, 2), 3) != 20) return false;
            if (Round(CalcUpperExtent(17.5F, 2), 3) != 20) return false;
            if (Round(CalcUpperExtent(18F, 2), 3) != 20) return false;
            if (Round(CalcUpperExtent(20F, 2), 3) != 20) return false;
            if (Round(CalcUpperExtent(-0.1F, 2), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.11F, 2), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.125F, 2), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.13F, 2), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.15F, 2), 3) != -0.15) return false;
            if (Round(CalcUpperExtent(-0.16F, 2), 3) != -0.15) return false;
            if (Round(CalcUpperExtent(-0.175F, 2), 3) != -0.15) return false;
            if (Round(CalcUpperExtent(-0.18F, 2), 3) != -0.15) return false;
            if (Round(CalcUpperExtent(-0.2F, 2), 3) != -0.2) return false;
            if (Round(CalcUpperExtent(-1F, 2), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.1F, 2), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.25F, 2), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.3F, 2), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.5F, 2), 3) != -1.5) return false;
            if (Round(CalcUpperExtent(-1.6F, 2), 3) != -1.5) return false;
            if (Round(CalcUpperExtent(-1.75F, 2), 3) != -1.5) return false;
            if (Round(CalcUpperExtent(-1.8F, 2), 3) != -1.5) return false;
            if (Round(CalcUpperExtent(-2F, 2), 3) != -2) return false;
            if (Round(CalcUpperExtent(-10F, 2), 3) != -10) return false;
            if (Round(CalcUpperExtent(-11F, 2), 3) != -10) return false;
            if (Round(CalcUpperExtent(-12.5F, 2), 3) != -10) return false;
            if (Round(CalcUpperExtent(-13F, 2), 3) != -10) return false;
            if (Round(CalcUpperExtent(-15F, 2), 3) != -15) return false;
            if (Round(CalcUpperExtent(-16F, 2), 3) != -15) return false;
            if (Round(CalcUpperExtent(-17.5F, 2), 3) != -15) return false;
            if (Round(CalcUpperExtent(-18F, 2), 3) != -15) return false;
            if (Round(CalcUpperExtent(-20F, 2), 3) != -20) return false;

            if (Round(CalcLowerExtent(0F, 4), 3) != 0) return false;
            if (Round(CalcLowerExtent(0.1F, 4), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.11F, 4), 3) != 0.1) return false;
            if (Round(CalcLowerExtent(0.125F, 4), 3) != 0.125) return false;
            if (Round(CalcLowerExtent(0.13F, 4), 3) != 0.125) return false;
            if (Round(CalcLowerExtent(0.15F, 4), 3) != 0.15) return false;
            if (Round(CalcLowerExtent(0.16F, 4), 3) != 0.15) return false;
            if (Round(CalcLowerExtent(0.175F, 4), 3) != 0.175) return false;
            if (Round(CalcLowerExtent(0.18F, 4), 3) != 0.175) return false;
            if (Round(CalcLowerExtent(0.2F, 4), 3) != 0.2) return false;
            if (Round(CalcLowerExtent(1F, 4), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.1F, 4), 3) != 1) return false;
            if (Round(CalcLowerExtent(1.25F, 4), 3) != 1.25) return false;
            if (Round(CalcLowerExtent(1.3F, 4), 3) != 1.25) return false;
            if (Round(CalcLowerExtent(1.5F, 4), 3) != 1.5) return false;
            if (Round(CalcLowerExtent(1.6F, 4), 3) != 1.5) return false;
            if (Round(CalcLowerExtent(1.75F, 4), 3) != 1.75) return false;
            if (Round(CalcLowerExtent(1.8F, 4), 3) != 1.75) return false;
            if (Round(CalcLowerExtent(2F, 4), 3) != 2) return false;
            if (Round(CalcLowerExtent(10F, 4), 3) != 10) return false;
            if (Round(CalcLowerExtent(11F, 4), 3) != 10) return false;
            if (Round(CalcLowerExtent(12.5F, 4), 3) != 12.5) return false;
            if (Round(CalcLowerExtent(13F, 4), 3) != 12.5) return false;
            if (Round(CalcLowerExtent(15F, 4), 3) != 15) return false;
            if (Round(CalcLowerExtent(16F, 4), 3) != 15) return false;
            if (Round(CalcLowerExtent(17.5F, 4), 3) != 17.5) return false;
            if (Round(CalcLowerExtent(18F, 4), 3) != 17.5) return false;
            if (Round(CalcLowerExtent(20F, 4), 3) != 20) return false;
            if (Round(CalcLowerExtent(-0.1F, 4), 3) != -0.1) return false;
            if (Round(CalcLowerExtent(-0.11F, 4), 3) != -0.125) return false;
            if (Round(CalcLowerExtent(-0.125F, 4), 3) != -0.125) return false;
            if (Round(CalcLowerExtent(-0.13F, 4), 3) != -0.15) return false;
            if (Round(CalcLowerExtent(-0.15F, 4), 3) != -0.15) return false;
            if (Round(CalcLowerExtent(-0.16F, 4), 3) != -0.175) return false;
            if (Round(CalcLowerExtent(-0.175F, 4), 3) != -0.175) return false;
            if (Round(CalcLowerExtent(-0.18F, 4), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-0.2F, 4), 3) != -0.2) return false;
            if (Round(CalcLowerExtent(-1F, 4), 3) != -1) return false;
            if (Round(CalcLowerExtent(-1.1F, 4), 3) != -1.25) return false;
            if (Round(CalcLowerExtent(-1.25F, 4), 3) != -1.25) return false;
            if (Round(CalcLowerExtent(-1.3F, 4), 3) != -1.5) return false;
            if (Round(CalcLowerExtent(-1.5F, 4), 3) != -1.5) return false;
            if (Round(CalcLowerExtent(-1.6F, 4), 3) != -1.75) return false;
            if (Round(CalcLowerExtent(-1.75F, 4), 3) != -1.75) return false;
            if (Round(CalcLowerExtent(-1.8F, 4), 3) != -2) return false;
            if (Round(CalcLowerExtent(-2F, 4), 3) != -2) return false;
            if (Round(CalcLowerExtent(-10F, 4), 3) != -10) return false;
            if (Round(CalcLowerExtent(-11F, 4), 3) != -12.5) return false;
            if (Round(CalcLowerExtent(-12.5F, 4), 3) != -12.5) return false;
            if (Round(CalcLowerExtent(-13F, 4), 3) != -15) return false;
            if (Round(CalcLowerExtent(-15F, 4), 3) != -15) return false;
            if (Round(CalcLowerExtent(-16F, 4), 3) != -17.5) return false;
            if (Round(CalcLowerExtent(-17.5F, 4), 3) != -17.5) return false;
            if (Round(CalcLowerExtent(-18F, 4), 3) != -20) return false;
            if (Round(CalcLowerExtent(-20F, 4), 3) != -20) return false;

            if (Round(CalcUpperExtent(0F, 4), 3) != 0) return false;
            if (Round(CalcUpperExtent(0.1F, 4), 3) != 0.1) return false;
            if (Round(CalcUpperExtent(0.11F, 4), 3) != 0.125) return false;
            if (Round(CalcUpperExtent(0.125F, 4), 3) != 0.125) return false;
            if (Round(CalcUpperExtent(0.13F, 4), 3) != 0.15) return false;
            if (Round(CalcUpperExtent(0.15F, 4), 3) != 0.15) return false;
            if (Round(CalcUpperExtent(0.16F, 4), 3) != 0.175) return false;
            if (Round(CalcUpperExtent(0.175F, 4), 3) != 0.175) return false;
            if (Round(CalcUpperExtent(0.18F, 4), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(0.2F, 4), 3) != 0.2) return false;
            if (Round(CalcUpperExtent(1F, 4), 3) != 1) return false;
            if (Round(CalcUpperExtent(1.1F, 4), 3) != 1.25) return false;
            if (Round(CalcUpperExtent(1.25F, 4), 3) != 1.25) return false;
            if (Round(CalcUpperExtent(1.3F, 4), 3) != 1.5) return false;
            if (Round(CalcUpperExtent(1.5F, 4), 3) != 1.5) return false;
            if (Round(CalcUpperExtent(1.6F, 4), 3) != 1.75) return false;
            if (Round(CalcUpperExtent(1.75F, 4), 3) != 1.75) return false;
            if (Round(CalcUpperExtent(1.8F, 4), 3) != 2) return false;
            if (Round(CalcUpperExtent(2F, 4), 3) != 2) return false;
            if (Round(CalcUpperExtent(10F, 4), 3) != 10) return false;
            if (Round(CalcUpperExtent(11F, 4), 3) != 12.5) return false;
            if (Round(CalcUpperExtent(12.5F, 4), 3) != 12.5) return false;
            if (Round(CalcUpperExtent(13F, 4), 3) != 15) return false;
            if (Round(CalcUpperExtent(15F, 4), 3) != 15) return false;
            if (Round(CalcUpperExtent(16F, 4), 3) != 17.5) return false;
            if (Round(CalcUpperExtent(17.5F, 4), 3) != 17.5) return false;
            if (Round(CalcUpperExtent(18F, 4), 3) != 20) return false;
            if (Round(CalcUpperExtent(20F, 4), 3) != 20) return false;
            if (Round(CalcUpperExtent(-0.1F, 4), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.11F, 4), 3) != -0.1) return false;
            if (Round(CalcUpperExtent(-0.125F, 4), 3) != -0.125) return false;
            if (Round(CalcUpperExtent(-0.13F, 4), 3) != -0.125) return false;
            if (Round(CalcUpperExtent(-0.15F, 4), 3) != -0.15) return false;
            if (Round(CalcUpperExtent(-0.16F, 4), 3) != -0.15) return false;
            if (Round(CalcUpperExtent(-0.175F, 4), 3) != -0.175) return false;
            if (Round(CalcUpperExtent(-0.18F, 4), 3) != -0.175) return false;
            if (Round(CalcUpperExtent(-0.2F, 4), 3) != -0.2) return false;
            if (Round(CalcUpperExtent(-1F, 4), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.1F, 4), 3) != -1) return false;
            if (Round(CalcUpperExtent(-1.25F, 4), 3) != -1.25) return false;
            if (Round(CalcUpperExtent(-1.3F, 4), 3) != -1.25) return false;
            if (Round(CalcUpperExtent(-1.5F, 4), 3) != -1.5) return false;
            if (Round(CalcUpperExtent(-1.6F, 4), 3) != -1.5) return false;
            if (Round(CalcUpperExtent(-1.75F, 4), 3) != -1.75) return false;
            if (Round(CalcUpperExtent(-1.8F, 4), 3) != -1.75) return false;
            if (Round(CalcUpperExtent(-2F, 4), 3) != -2) return false;
            if (Round(CalcUpperExtent(-10F, 4), 3) != -10) return false;
            if (Round(CalcUpperExtent(-11F, 4), 3) != -10) return false;
            if (Round(CalcUpperExtent(-12.5F, 4), 3) != -12.5) return false;
            if (Round(CalcUpperExtent(-13F, 4), 3) != -12.5) return false;
            if (Round(CalcUpperExtent(-15F, 4), 3) != -15) return false;
            if (Round(CalcUpperExtent(-16F, 4), 3) != -15) return false;
            if (Round(CalcUpperExtent(-17.5F, 4), 3) != -17.5) return false;
            if (Round(CalcUpperExtent(-18F, 4), 3) != -17.5) return false;
            if (Round(CalcUpperExtent(-20F, 4), 3) != -20) return false;

            return true;
        }

        public static bool ItInstantiates()
        {
            Axis axis = new Axis(20, 100, -5, 15);
            if (axis.Start != 20) return false;
            if (Round(axis.Origin, 3) != 45) return false;
            if (axis.Length != 100) return false;
            if (axis.LowerExtent != -5) return false;
            if (axis.UpperExtent != 15) return false;
            if (axis.Inverted) return false;
            if (axis.UnitLength != 5) return false;
            if (Round(axis.InverseUnitLength, 3) != 0.2) return false;

            Axis axis2 = new Axis(20, 100, new float[] { -15, 0, 5 });
            if (axis2.Start != 20) return false;
            if (Round(axis2.Origin, 3) != 100) return false;
            if (axis2.Length != 100) return false;
            if (axis2.LowerExtent != -20) return false;
            if (axis2.UpperExtent != 5) return false;
            if (axis2.Inverted) return false;
            if (axis2.UnitLength != 4) return false;
            if (Round(axis2.InverseUnitLength, 3) != 0.25) return false;

            Axis axis3 = new Axis(20, 100, new float[] { 10, 10, 10 });
            if (axis3.Start != 20) return false;
            if (Round(axis3.Origin, 3) != 20) return false;
            if (axis3.Length != 100) return false;
            if (axis3.LowerExtent != 0) return false;
            if (axis3.UpperExtent != 20) return false;
            if (axis3.Inverted) return false;
            if (axis3.UnitLength != 5) return false;
            if (Round(axis3.InverseUnitLength, 3) != 0.2) return false;

            Axis axis4 = new Axis(20, 100, new float[] { -10, -10, -10 });
            if (axis4.Start != 20) return false;
            if (Round(axis4.Origin, 3) != 120) return false;
            if (axis4.Length != 100) return false;
            if (axis4.LowerExtent != -20) return false;
            if (axis4.UpperExtent != 0) return false;
            if (axis4.Inverted) return false;
            if (axis4.UnitLength != 5) return false;
            if (Round(axis4.InverseUnitLength, 3) != 0.2) return false;

            Axis axis5 = new Axis(20, 100, 10, 30);
            if (axis5.Start != 20) return false;
            if (Round(axis5.Origin, 3) != -30) return false;
            if (axis5.Length != 100) return false;
            if (axis5.LowerExtent != 10) return false;
            if (axis5.UpperExtent != 30) return false;
            if (axis5.Inverted) return false;
            if (axis5.UnitLength != 5) return false;
            if (Round(axis5.InverseUnitLength, 3) != 0.2) return false;

            Axis axis6 = new Axis(20, 100, 10, 30, true);
            if (axis6.Start != 20) return false;
            if (Round(axis6.Origin, 3) != 170) return false;
            if (axis6.Length != 100) return false;
            if (axis6.LowerExtent != 10) return false;
            if (axis6.UpperExtent != 30) return false;
            if (!axis6.Inverted) return false;
            if (axis6.UnitLength != 5) return false;
            if (Round(axis6.InverseUnitLength, 3) != 0.2) return false;

            Axis axis7 = new Axis(20, 100, -15, 5, true);
            if (axis7.Start != 20) return false;
            if (Round(axis7.Origin, 3) != 45) return false;
            if (axis7.Length != 100) return false;
            if (axis7.LowerExtent != -15) return false;
            if (axis7.UpperExtent != 5) return false;
            if (!axis7.Inverted) return false;
            if (axis7.UnitLength != 5) return false;
            if (Round(axis7.InverseUnitLength, 3) != 0.2) return false;

            try
            {
                Axis axis8 = new Axis(20, 100, 0, -1, true);
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("lowerExtent must be less than upperExtent.")) return false;
            }

            try
            {
                axis.Length = 0;
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("Length must be greater than zero.")) return false;
            }

            try
            {
                axis.LowerExtent = axis.UpperExtent;
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("LowerExtent must be less than UpperExtent.")) return false;
            }

            try
            {
                axis.UpperExtent = axis.LowerExtent;
                return false;
            }
            catch (ArgumentOutOfRangeException e)
            {
                if (!e.Message.StartsWith("UpperExtent must be less than LowerExtent.")) return false;
            }

            return true;
        }

        public static bool ItMaps()
        {
            Axis axis = new Axis(20, 100, -10, 10);
            if (Round(axis.Map(5), 3) != 95) return false;
            if (Round(axis.Map(-5), 3) != 45) return false;

            Axis axis2 = new Axis(20, 100, -10, 10, true);
            if (Round(axis2.Map(5), 3) != 45) return false;
            if (Round(axis2.Map(-5), 3) != 95) return false;

            return true;
        }

        public static bool ItUnmaps()
        {
            Axis axis = new Axis(20, 100, -10, 10);
            if (Round(axis.Unmap(95), 3) != 5) return false;
            if (Round(axis.Unmap(45), 3) != -5) return false;

            Axis axis2 = new Axis(20, 100, -10, 10, true);
            if (Round(axis2.Unmap(45), 3) != 5) return false;
            if (Round(axis2.Unmap(95), 3) != -5) return false;

            return true;
        }
    }
}
