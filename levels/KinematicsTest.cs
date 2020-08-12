using Godot;
using System;
using Physics;
using Graphing;

public class KinematicsTest : Carousel
{
    private PackedScene graphScene;

    public override void _Ready()
    {
        TestPlotGetLowerExtentQuarter();
        TestPlotGetUpperExtentQuarter();
        TestPlotGetLowerExtentHalf();
        TestPlotGetUpperExtentHalf();
        TestPlotGetLowerExtent();
        TestPlotGetUpperExtent();

        graphScene = (PackedScene)ResourceLoader.Load("res://nodes/Graph.tscn");
        carouselNodes.Add(CreateGraph(GenerateVelocityData(0, 100, 0.01), "1) v0 = 0, a = 100, d = 0.01"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(0, -100, 0.01), "2) v0 = 0, a = -100, d = 0.01"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(200, 100, 0.01), "3) v0 = 200, a = 100, d = 0.01"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(200, -100, 0.01), "4) v0 = 200, a = -100, d = 0.01"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(-200, 100, 0.01), "5) v0 = -200, a = 100, d = 0.01"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(-200, -100, 0.01), "6) v0 = -200, a = -100, d = 0.01"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(0, 100), "7) v0 = 0, a = 100"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(0, -100), "8) v0 = 0, a = -100"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(15, 10), "9) v0 = 15, a = 10"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(15, -10), "10) v0 = 15, a = -10"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(-15, 10), "11) v0 = -15, a = 10"));
        carouselNodes.Add(CreateGraph(GenerateVelocityData(-15, -10), "12) v0 = -15, a = -10"));
        SetActiveNode(0);
    }

    private string Summarize(string description, double actual, double expected, int precision = 10)
    {
        if (Math.Round(actual, precision) == Math.Round(expected, precision))
        {
            return String.Format("Pass {0}: {1}", description, actual);
        }

        return String.Format("Fail {0}: actual = {1}, expected = {2}", description, actual, expected);
    }

    private void TestPlotGetLowerExtentQuarter()
    {
        GD.Print("---GetAxisLowerExtentQuarter Tests---");
        GD.Print(Summarize("Zero", Plot.GetAxisLowerExtentQuarter(0), 0));
        GD.Print(Summarize("Tenths first quartile exact", Plot.GetAxisLowerExtentQuarter(0.1), 0.1));
        GD.Print(Summarize("Tenths first quartile", Plot.GetAxisLowerExtentQuarter(0.11), 0.1));
        GD.Print(Summarize("Tenths second quartile exact", Plot.GetAxisLowerExtentQuarter(0.125), 0.125));
        GD.Print(Summarize("Tenths second quartile", Plot.GetAxisLowerExtentQuarter(0.13), 0.125));
        GD.Print(Summarize("Tenths third quartile exact", Plot.GetAxisLowerExtentQuarter(0.15), 0.15));
        GD.Print(Summarize("Tenths third quartile", Plot.GetAxisLowerExtentQuarter(0.16), 0.15));
        GD.Print(Summarize("Tenths fourth quartile exact", Plot.GetAxisLowerExtentQuarter(0.175), 0.175));
        GD.Print(Summarize("Tenths fourth quartile", Plot.GetAxisLowerExtentQuarter(0.18), 0.175));
        GD.Print(Summarize("Tenths first quartile exact again", Plot.GetAxisLowerExtentQuarter(0.2), 0.2));
        GD.Print(Summarize("Ones first quartile exact", Plot.GetAxisLowerExtentQuarter(1), 1));
        GD.Print(Summarize("Ones first quartile", Plot.GetAxisLowerExtentQuarter(1.1), 1));
        GD.Print(Summarize("Ones second quartile exact", Plot.GetAxisLowerExtentQuarter(1.25), 1.25));
        GD.Print(Summarize("Ones second quartile", Plot.GetAxisLowerExtentQuarter(1.3), 1.25));
        GD.Print(Summarize("Ones third quartile exact", Plot.GetAxisLowerExtentQuarter(1.5), 1.5));
        GD.Print(Summarize("Ones third quartile", Plot.GetAxisLowerExtentQuarter(1.6), 1.5));
        GD.Print(Summarize("Ones fourth quartile exact", Plot.GetAxisLowerExtentQuarter(1.75), 1.75));
        GD.Print(Summarize("Ones fourth quartile", Plot.GetAxisLowerExtentQuarter(1.8), 1.75));
        GD.Print(Summarize("Ones first quartile exact again", Plot.GetAxisLowerExtentQuarter(2), 2));
        GD.Print(Summarize("Tens first quartile exact", Plot.GetAxisLowerExtentQuarter(10), 10));
        GD.Print(Summarize("Tens first quartile", Plot.GetAxisLowerExtentQuarter(11), 10));
        GD.Print(Summarize("Tens second quartile exact", Plot.GetAxisLowerExtentQuarter(12.5), 12.5));
        GD.Print(Summarize("Tens second quartile", Plot.GetAxisLowerExtentQuarter(13), 12.5));
        GD.Print(Summarize("Tens third quartile exact", Plot.GetAxisLowerExtentQuarter(15), 15));
        GD.Print(Summarize("Tens third quartile", Plot.GetAxisLowerExtentQuarter(16), 15));
        GD.Print(Summarize("Tens fourth quartile exact", Plot.GetAxisLowerExtentQuarter(17.5), 17.5));
        GD.Print(Summarize("Tens fourth quartile", Plot.GetAxisLowerExtentQuarter(18), 17.5));
        GD.Print(Summarize("Tens first quartile exact again", Plot.GetAxisLowerExtentQuarter(20), 20));
        GD.Print(Summarize("Negative tenths first quartile exact", Plot.GetAxisLowerExtentQuarter(-0.1), -0.1));
        GD.Print(Summarize("Negative tenths first quartile", Plot.GetAxisLowerExtentQuarter(-0.11), -0.125));
        GD.Print(Summarize("Negative tenths second quartile exact", Plot.GetAxisLowerExtentQuarter(-0.125), -0.125));
        GD.Print(Summarize("Negative tenths second quartile", Plot.GetAxisLowerExtentQuarter(-0.13), -0.15));
        GD.Print(Summarize("Negative tenths third quartile exact", Plot.GetAxisLowerExtentQuarter(-0.15), -0.15));
        GD.Print(Summarize("Negative tenths third quartile", Plot.GetAxisLowerExtentQuarter(-0.16), -0.175));
        GD.Print(Summarize("Negative tenths fourth quartile exact", Plot.GetAxisLowerExtentQuarter(-0.175), -0.175));
        GD.Print(Summarize("Negative tenths fourth quartile", Plot.GetAxisLowerExtentQuarter(-0.18), -0.2));
        GD.Print(Summarize("Negative tenths first quartile exact again", Plot.GetAxisLowerExtentQuarter(-0.2), -0.2));
        GD.Print(Summarize("Negative ones first quartile exact", Plot.GetAxisLowerExtentQuarter(-1), -1));
        GD.Print(Summarize("Negative ones first quartile", Plot.GetAxisLowerExtentQuarter(-1.1), -1.25));
        GD.Print(Summarize("Negative ones second quartile exact", Plot.GetAxisLowerExtentQuarter(-1.25), -1.25));
        GD.Print(Summarize("Negative ones second quartile", Plot.GetAxisLowerExtentQuarter(-1.3), -1.5));
        GD.Print(Summarize("Negative ones third quartile exact", Plot.GetAxisLowerExtentQuarter(-1.5), -1.5));
        GD.Print(Summarize("Negative ones third quartile", Plot.GetAxisLowerExtentQuarter(-1.6), -1.75));
        GD.Print(Summarize("Negative ones fourth quartile exact", Plot.GetAxisLowerExtentQuarter(-1.75), -1.75));
        GD.Print(Summarize("Negative ones fourth quartile", Plot.GetAxisLowerExtentQuarter(-1.8), -2));
        GD.Print(Summarize("Negative ones first quartile exact again", Plot.GetAxisLowerExtentQuarter(-2), -2));
        GD.Print(Summarize("Negative tens first quartile exact", Plot.GetAxisLowerExtentQuarter(-10), -10));
        GD.Print(Summarize("Negative tens first quartile", Plot.GetAxisLowerExtentQuarter(-11), -12.5));
        GD.Print(Summarize("Negative tens second quartile exact", Plot.GetAxisLowerExtentQuarter(-12.5), -12.5));
        GD.Print(Summarize("Negative tens second quartile", Plot.GetAxisLowerExtentQuarter(-13), -15));
        GD.Print(Summarize("Negative tens third quartile exact", Plot.GetAxisLowerExtentQuarter(-15), -15));
        GD.Print(Summarize("Negative tens third quartile", Plot.GetAxisLowerExtentQuarter(-16), -17.5));
        GD.Print(Summarize("Negative tens fourth quartile exact", Plot.GetAxisLowerExtentQuarter(-17.5), -17.5));
        GD.Print(Summarize("Negative tens fourth quartile", Plot.GetAxisLowerExtentQuarter(-18), -20));
        GD.Print(Summarize("Negative tens first quartile exact again", Plot.GetAxisLowerExtentQuarter(-20), -20));
    }

    private void TestPlotGetUpperExtentQuarter()
    {
        GD.Print("---GetAxisUpperExtentQuarter Tests---");
        GD.Print(Summarize("Zero", Plot.GetAxisUpperExtentQuarter(0), 0));
        GD.Print(Summarize("Tenths first quartile exact", Plot.GetAxisUpperExtentQuarter(0.1), 0.1));
        GD.Print(Summarize("Tenths first quartile", Plot.GetAxisUpperExtentQuarter(0.11), 0.125));
        GD.Print(Summarize("Tenths second quartile exact", Plot.GetAxisUpperExtentQuarter(0.125), 0.125));
        GD.Print(Summarize("Tenths second quartile", Plot.GetAxisUpperExtentQuarter(0.13), 0.15));
        GD.Print(Summarize("Tenths third quartile exact", Plot.GetAxisUpperExtentQuarter(0.15), 0.15));
        GD.Print(Summarize("Tenths third quartile", Plot.GetAxisUpperExtentQuarter(0.16), 0.175));
        GD.Print(Summarize("Tenths fourth quartile exact", Plot.GetAxisUpperExtentQuarter(0.175), 0.175));
        GD.Print(Summarize("Tenths fourth quartile", Plot.GetAxisUpperExtentQuarter(0.18), 0.2));
        GD.Print(Summarize("Tenths first quartile exact again", Plot.GetAxisUpperExtentQuarter(0.2), 0.2));
        GD.Print(Summarize("Ones first quartile exact", Plot.GetAxisUpperExtentQuarter(1), 1));
        GD.Print(Summarize("Ones first quartile", Plot.GetAxisUpperExtentQuarter(1.1), 1.25));
        GD.Print(Summarize("Ones second quartile exact", Plot.GetAxisUpperExtentQuarter(1.25), 1.25));
        GD.Print(Summarize("Ones second quartile", Plot.GetAxisUpperExtentQuarter(1.3), 1.5));
        GD.Print(Summarize("Ones third quartile exact", Plot.GetAxisUpperExtentQuarter(1.5), 1.5));
        GD.Print(Summarize("Ones third quartile", Plot.GetAxisUpperExtentQuarter(1.6), 1.75));
        GD.Print(Summarize("Ones fourth quartile exact", Plot.GetAxisUpperExtentQuarter(1.75), 1.75));
        GD.Print(Summarize("Ones fourth quartile", Plot.GetAxisUpperExtentQuarter(1.8), 2));
        GD.Print(Summarize("Ones first quartile exact again", Plot.GetAxisUpperExtentQuarter(2), 2));
        GD.Print(Summarize("Tens first quartile exact", Plot.GetAxisUpperExtentQuarter(10), 10));
        GD.Print(Summarize("Tens first quartile", Plot.GetAxisUpperExtentQuarter(11), 12.5));
        GD.Print(Summarize("Tens second quartile exact", Plot.GetAxisUpperExtentQuarter(12.5), 12.5));
        GD.Print(Summarize("Tens second quartile", Plot.GetAxisUpperExtentQuarter(13), 15));
        GD.Print(Summarize("Tens third quartile exact", Plot.GetAxisUpperExtentQuarter(15), 15));
        GD.Print(Summarize("Tens third quartile", Plot.GetAxisUpperExtentQuarter(16), 17.5));
        GD.Print(Summarize("Tens fourth quartile exact", Plot.GetAxisUpperExtentQuarter(17.5), 17.5));
        GD.Print(Summarize("Tens fourth quartile", Plot.GetAxisUpperExtentQuarter(18), 20));
        GD.Print(Summarize("Tens first quartile exact again", Plot.GetAxisUpperExtentQuarter(20), 20));
        GD.Print(Summarize("Negative tenths first quartile exact", Plot.GetAxisUpperExtentQuarter(-0.1), -0.1));
        GD.Print(Summarize("Negative tenths first quartile", Plot.GetAxisUpperExtentQuarter(-0.11), -0.1));
        GD.Print(Summarize("Negative tenths second quartile exact", Plot.GetAxisUpperExtentQuarter(-0.125), -0.125));
        GD.Print(Summarize("Negative tenths second quartile", Plot.GetAxisUpperExtentQuarter(-0.13), -0.125));
        GD.Print(Summarize("Negative tenths third quartile exact", Plot.GetAxisUpperExtentQuarter(-0.15), -0.15));
        GD.Print(Summarize("Negative tenths third quartile", Plot.GetAxisUpperExtentQuarter(-0.16), -0.15));
        GD.Print(Summarize("Negative tenths fourth quartile exact", Plot.GetAxisUpperExtentQuarter(-0.175), -0.175));
        GD.Print(Summarize("Negative tenths fourth quartile", Plot.GetAxisUpperExtentQuarter(-0.18), -0.175));
        GD.Print(Summarize("Negative tenths first quartile exact again", Plot.GetAxisUpperExtentQuarter(-0.2), -0.2));
        GD.Print(Summarize("Negative ones first quartile exact", Plot.GetAxisUpperExtentQuarter(-1), -1));
        GD.Print(Summarize("Negative ones first quartile", Plot.GetAxisUpperExtentQuarter(-1.1), -1));
        GD.Print(Summarize("Negative ones second quartile exact", Plot.GetAxisUpperExtentQuarter(-1.25), -1.25));
        GD.Print(Summarize("Negative ones second quartile", Plot.GetAxisUpperExtentQuarter(-1.3), -1.25));
        GD.Print(Summarize("Negative ones third quartile exact", Plot.GetAxisUpperExtentQuarter(-1.5), -1.5));
        GD.Print(Summarize("Negative ones third quartile", Plot.GetAxisUpperExtentQuarter(-1.6), -1.5));
        GD.Print(Summarize("Negative ones fourth quartile exact", Plot.GetAxisUpperExtentQuarter(-1.75), -1.75));
        GD.Print(Summarize("Negative ones fourth quartile", Plot.GetAxisUpperExtentQuarter(-1.8), -1.75));
        GD.Print(Summarize("Negative ones first quartile exact again", Plot.GetAxisUpperExtentQuarter(-2), -2));
        GD.Print(Summarize("Negative tens first quartile exact", Plot.GetAxisUpperExtentQuarter(-10), -10));
        GD.Print(Summarize("Negative tens first quartile", Plot.GetAxisUpperExtentQuarter(-11), -10));
        GD.Print(Summarize("Negative tens second quartile exact", Plot.GetAxisUpperExtentQuarter(-12.5), -12.5));
        GD.Print(Summarize("Negative tens second quartile", Plot.GetAxisUpperExtentQuarter(-13), -12.5));
        GD.Print(Summarize("Negative tens third quartile exact", Plot.GetAxisUpperExtentQuarter(-15), -15));
        GD.Print(Summarize("Negative tens third quartile", Plot.GetAxisUpperExtentQuarter(-16), -15));
        GD.Print(Summarize("Negative tens fourth quartile exact", Plot.GetAxisUpperExtentQuarter(-17.5), -17.5));
        GD.Print(Summarize("Negative tens fourth quartile", Plot.GetAxisUpperExtentQuarter(-18), -17.5));
        GD.Print(Summarize("Negative tens first quartile exact again", Plot.GetAxisUpperExtentQuarter(-20), -20));
    }

    private void TestPlotGetLowerExtentHalf()
    {
        GD.Print("---GetAxisLowerExtentHalf Tests---");
        GD.Print(Summarize("Zero", Plot.GetAxisLowerExtentHalf(0), 0));
        GD.Print(Summarize("Tenths first quartile exact", Plot.GetAxisLowerExtentHalf(0.1), 0.1));
        GD.Print(Summarize("Tenths first quartile", Plot.GetAxisLowerExtentHalf(0.11), 0.1));
        GD.Print(Summarize("Tenths second quartile exact", Plot.GetAxisLowerExtentHalf(0.125), 0.1));
        GD.Print(Summarize("Tenths second quartile", Plot.GetAxisLowerExtentHalf(0.13), 0.1));
        GD.Print(Summarize("Tenths third quartile exact", Plot.GetAxisLowerExtentHalf(0.15), 0.15));
        GD.Print(Summarize("Tenths third quartile", Plot.GetAxisLowerExtentHalf(0.16), 0.15));
        GD.Print(Summarize("Tenths fourth quartile exact", Plot.GetAxisLowerExtentHalf(0.175), 0.15));
        GD.Print(Summarize("Tenths fourth quartile", Plot.GetAxisLowerExtentHalf(0.18), 0.15));
        GD.Print(Summarize("Tenths first quartile exact again", Plot.GetAxisLowerExtentHalf(0.2), 0.2));
        GD.Print(Summarize("Ones first quartile exact", Plot.GetAxisLowerExtentHalf(1), 1));
        GD.Print(Summarize("Ones first quartile", Plot.GetAxisLowerExtentHalf(1.1), 1));
        GD.Print(Summarize("Ones second quartile exact", Plot.GetAxisLowerExtentHalf(1.25), 1));
        GD.Print(Summarize("Ones second quartile", Plot.GetAxisLowerExtentHalf(1.3), 1));
        GD.Print(Summarize("Ones third quartile exact", Plot.GetAxisLowerExtentHalf(1.5), 1.5));
        GD.Print(Summarize("Ones third quartile", Plot.GetAxisLowerExtentHalf(1.6), 1.5));
        GD.Print(Summarize("Ones fourth quartile exact", Plot.GetAxisLowerExtentHalf(1.75), 1.5));
        GD.Print(Summarize("Ones fourth quartile", Plot.GetAxisLowerExtentHalf(1.8), 1.5));
        GD.Print(Summarize("Ones first quartile exact again", Plot.GetAxisLowerExtentHalf(2), 2));
        GD.Print(Summarize("Tens first quartile exact", Plot.GetAxisLowerExtentHalf(10), 10));
        GD.Print(Summarize("Tens first quartile", Plot.GetAxisLowerExtentHalf(11), 10));
        GD.Print(Summarize("Tens second quartile exact", Plot.GetAxisLowerExtentHalf(12.5), 10));
        GD.Print(Summarize("Tens second quartile", Plot.GetAxisLowerExtentHalf(13), 10));
        GD.Print(Summarize("Tens third quartile exact", Plot.GetAxisLowerExtentHalf(15), 15));
        GD.Print(Summarize("Tens third quartile", Plot.GetAxisLowerExtentHalf(16), 15));
        GD.Print(Summarize("Tens fourth quartile exact", Plot.GetAxisLowerExtentHalf(17.5), 15));
        GD.Print(Summarize("Tens fourth quartile", Plot.GetAxisLowerExtentHalf(18), 15));
        GD.Print(Summarize("Tens first quartile exact again", Plot.GetAxisLowerExtentHalf(20), 20));
        GD.Print(Summarize("Negative tenths first quartile exact", Plot.GetAxisLowerExtentHalf(-0.1), -0.1));
        GD.Print(Summarize("Negative tenths first quartile", Plot.GetAxisLowerExtentHalf(-0.11), -0.15));
        GD.Print(Summarize("Negative tenths second quartile exact", Plot.GetAxisLowerExtentHalf(-0.125), -0.15));
        GD.Print(Summarize("Negative tenths second quartile", Plot.GetAxisLowerExtentHalf(-0.13), -0.15));
        GD.Print(Summarize("Negative tenths third quartile exact", Plot.GetAxisLowerExtentHalf(-0.15), -0.15));
        GD.Print(Summarize("Negative tenths third quartile", Plot.GetAxisLowerExtentHalf(-0.16), -0.2));
        GD.Print(Summarize("Negative tenths fourth quartile exact", Plot.GetAxisLowerExtentHalf(-0.175), -0.2));
        GD.Print(Summarize("Negative tenths fourth quartile", Plot.GetAxisLowerExtentHalf(-0.18), -0.2));
        GD.Print(Summarize("Negative tenths first quartile exact again", Plot.GetAxisLowerExtentHalf(-0.2), -0.2));
        GD.Print(Summarize("Negative ones first quartile exact", Plot.GetAxisLowerExtentHalf(-1), -1));
        GD.Print(Summarize("Negative ones first quartile", Plot.GetAxisLowerExtentHalf(-1.1), -1.5));
        GD.Print(Summarize("Negative ones second quartile exact", Plot.GetAxisLowerExtentHalf(-1.25), -1.5));
        GD.Print(Summarize("Negative ones second quartile", Plot.GetAxisLowerExtentHalf(-1.3), -1.5));
        GD.Print(Summarize("Negative ones third quartile exact", Plot.GetAxisLowerExtentHalf(-1.5), -1.5));
        GD.Print(Summarize("Negative ones third quartile", Plot.GetAxisLowerExtentHalf(-1.6), -2));
        GD.Print(Summarize("Negative ones fourth quartile exact", Plot.GetAxisLowerExtentHalf(-1.75), -2));
        GD.Print(Summarize("Negative ones fourth quartile", Plot.GetAxisLowerExtentHalf(-1.8), -2));
        GD.Print(Summarize("Negative ones first quartile exact again", Plot.GetAxisLowerExtentHalf(-2), -2));
        GD.Print(Summarize("Negative tens first quartile exact", Plot.GetAxisLowerExtentHalf(-10), -10));
        GD.Print(Summarize("Negative tens first quartile", Plot.GetAxisLowerExtentHalf(-11), -15));
        GD.Print(Summarize("Negative tens second quartile exact", Plot.GetAxisLowerExtentHalf(-12.5), -15));
        GD.Print(Summarize("Negative tens second quartile", Plot.GetAxisLowerExtentHalf(-13), -15));
        GD.Print(Summarize("Negative tens third quartile exact", Plot.GetAxisLowerExtentHalf(-15), -15));
        GD.Print(Summarize("Negative tens third quartile", Plot.GetAxisLowerExtentHalf(-16), -20));
        GD.Print(Summarize("Negative tens fourth quartile exact", Plot.GetAxisLowerExtentHalf(-17.5), -20));
        GD.Print(Summarize("Negative tens fourth quartile", Plot.GetAxisLowerExtentHalf(-18), -20));
        GD.Print(Summarize("Negative tens first quartile exact again", Plot.GetAxisLowerExtentHalf(-20), -20));
    }

    private void TestPlotGetUpperExtentHalf()
    {
        GD.Print("---GetAxisUpperExtentHalf Tests---");
        GD.Print(Summarize("Zero", Plot.GetAxisUpperExtentHalf(0), 0));
        GD.Print(Summarize("Tenths first quartile exact", Plot.GetAxisUpperExtentHalf(0.1), 0.1));
        GD.Print(Summarize("Tenths first quartile", Plot.GetAxisUpperExtentHalf(0.11), 0.15));
        GD.Print(Summarize("Tenths second quartile exact", Plot.GetAxisUpperExtentHalf(0.125), 0.15));
        GD.Print(Summarize("Tenths second quartile", Plot.GetAxisUpperExtentHalf(0.13), 0.15));
        GD.Print(Summarize("Tenths third quartile exact", Plot.GetAxisUpperExtentHalf(0.15), 0.15));
        GD.Print(Summarize("Tenths third quartile", Plot.GetAxisUpperExtentHalf(0.16), 0.2));
        GD.Print(Summarize("Tenths fourth quartile exact", Plot.GetAxisUpperExtentHalf(0.175), 0.2));
        GD.Print(Summarize("Tenths fourth quartile", Plot.GetAxisUpperExtentHalf(0.18), 0.2));
        GD.Print(Summarize("Tenths first quartile exact again", Plot.GetAxisUpperExtentHalf(0.2), 0.2));
        GD.Print(Summarize("Ones first quartile exact", Plot.GetAxisUpperExtentHalf(1), 1));
        GD.Print(Summarize("Ones first quartile", Plot.GetAxisUpperExtentHalf(1.1), 1.5));
        GD.Print(Summarize("Ones second quartile exact", Plot.GetAxisUpperExtentHalf(1.25), 1.5));
        GD.Print(Summarize("Ones second quartile", Plot.GetAxisUpperExtentHalf(1.3), 1.5));
        GD.Print(Summarize("Ones third quartile exact", Plot.GetAxisUpperExtentHalf(1.5), 1.5));
        GD.Print(Summarize("Ones third quartile", Plot.GetAxisUpperExtentHalf(1.6), 2));
        GD.Print(Summarize("Ones fourth quartile exact", Plot.GetAxisUpperExtentHalf(1.75), 2));
        GD.Print(Summarize("Ones fourth quartile", Plot.GetAxisUpperExtentHalf(1.8), 2));
        GD.Print(Summarize("Ones first quartile exact again", Plot.GetAxisUpperExtentHalf(2), 2));
        GD.Print(Summarize("Tens first quartile exact", Plot.GetAxisUpperExtentHalf(10), 10));
        GD.Print(Summarize("Tens first quartile", Plot.GetAxisUpperExtentHalf(11), 15));
        GD.Print(Summarize("Tens second quartile exact", Plot.GetAxisUpperExtentHalf(12.5), 15));
        GD.Print(Summarize("Tens second quartile", Plot.GetAxisUpperExtentHalf(13), 15));
        GD.Print(Summarize("Tens third quartile exact", Plot.GetAxisUpperExtentHalf(15), 15));
        GD.Print(Summarize("Tens third quartile", Plot.GetAxisUpperExtentHalf(16), 20));
        GD.Print(Summarize("Tens fourth quartile exact", Plot.GetAxisUpperExtentHalf(17.5), 20));
        GD.Print(Summarize("Tens fourth quartile", Plot.GetAxisUpperExtentHalf(18), 20));
        GD.Print(Summarize("Tens first quartile exact again", Plot.GetAxisUpperExtentHalf(20), 20));
        GD.Print(Summarize("Negative tenths first quartile exact", Plot.GetAxisUpperExtentHalf(-0.1), -0.1));
        GD.Print(Summarize("Negative tenths first quartile", Plot.GetAxisUpperExtentHalf(-0.11), -0.1));
        GD.Print(Summarize("Negative tenths second quartile exact", Plot.GetAxisUpperExtentHalf(-0.125), -0.1));
        GD.Print(Summarize("Negative tenths second quartile", Plot.GetAxisUpperExtentHalf(-0.13), -0.1));
        GD.Print(Summarize("Negative tenths third quartile exact", Plot.GetAxisUpperExtentHalf(-0.15), -0.15));
        GD.Print(Summarize("Negative tenths third quartile", Plot.GetAxisUpperExtentHalf(-0.16), -0.15));
        GD.Print(Summarize("Negative tenths fourth quartile exact", Plot.GetAxisUpperExtentHalf(-0.175), -0.15));
        GD.Print(Summarize("Negative tenths fourth quartile", Plot.GetAxisUpperExtentHalf(-0.18), -0.15));
        GD.Print(Summarize("Negative tenths first quartile exact again", Plot.GetAxisUpperExtentHalf(-0.2), -0.2));
        GD.Print(Summarize("Negative ones first quartile exact", Plot.GetAxisUpperExtentHalf(-1), -1));
        GD.Print(Summarize("Negative ones first quartile", Plot.GetAxisUpperExtentHalf(-1.1), -1));
        GD.Print(Summarize("Negative ones second quartile exact", Plot.GetAxisUpperExtentHalf(-1.25), -1));
        GD.Print(Summarize("Negative ones second quartile", Plot.GetAxisUpperExtentHalf(-1.3), -1));
        GD.Print(Summarize("Negative ones third quartile exact", Plot.GetAxisUpperExtentHalf(-1.5), -1.5));
        GD.Print(Summarize("Negative ones third quartile", Plot.GetAxisUpperExtentHalf(-1.6), -1.5));
        GD.Print(Summarize("Negative ones fourth quartile exact", Plot.GetAxisUpperExtentHalf(-1.75), -1.5));
        GD.Print(Summarize("Negative ones fourth quartile", Plot.GetAxisUpperExtentHalf(-1.8), -1.5));
        GD.Print(Summarize("Negative ones first quartile exact again", Plot.GetAxisUpperExtentHalf(-2), -2));
        GD.Print(Summarize("Negative tens first quartile exact", Plot.GetAxisUpperExtentHalf(-10), -10));
        GD.Print(Summarize("Negative tens first quartile", Plot.GetAxisUpperExtentHalf(-11), -10));
        GD.Print(Summarize("Negative tens second quartile exact", Plot.GetAxisUpperExtentHalf(-12.5), -10));
        GD.Print(Summarize("Negative tens second quartile", Plot.GetAxisUpperExtentHalf(-13), -10));
        GD.Print(Summarize("Negative tens third quartile exact", Plot.GetAxisUpperExtentHalf(-15), -15));
        GD.Print(Summarize("Negative tens third quartile", Plot.GetAxisUpperExtentHalf(-16), -15));
        GD.Print(Summarize("Negative tens fourth quartile exact", Plot.GetAxisUpperExtentHalf(-17.5), -15));
        GD.Print(Summarize("Negative tens fourth quartile", Plot.GetAxisUpperExtentHalf(-18), -15));
        GD.Print(Summarize("Negative tens first quartile exact again", Plot.GetAxisUpperExtentHalf(-20), -20));
    }

    private void TestPlotGetLowerExtent()
    {
        GD.Print("---GetAxisLowerExtent Tests---");
        GD.Print(Summarize("Zero", Plot.GetAxisLowerExtent(0), 0));
        GD.Print(Summarize("Tenths first quartile exact", Plot.GetAxisLowerExtent(0.1), 0.1));
        GD.Print(Summarize("Tenths first quartile", Plot.GetAxisLowerExtent(0.11), 0.1));
        GD.Print(Summarize("Tenths second quartile exact", Plot.GetAxisLowerExtent(0.125), 0.1));
        GD.Print(Summarize("Tenths second quartile", Plot.GetAxisLowerExtent(0.13), 0.1));
        GD.Print(Summarize("Tenths third quartile exact", Plot.GetAxisLowerExtent(0.15), 0.1));
        GD.Print(Summarize("Tenths third quartile", Plot.GetAxisLowerExtent(0.16), 0.1));
        GD.Print(Summarize("Tenths fourth quartile exact", Plot.GetAxisLowerExtent(0.175), 0.1));
        GD.Print(Summarize("Tenths fourth quartile", Plot.GetAxisLowerExtent(0.18), 0.1));
        GD.Print(Summarize("Tenths first quartile exact again", Plot.GetAxisLowerExtent(0.2), 0.2));
        GD.Print(Summarize("Ones first quartile exact", Plot.GetAxisLowerExtent(1), 1));
        GD.Print(Summarize("Ones first quartile", Plot.GetAxisLowerExtent(1.1), 1));
        GD.Print(Summarize("Ones second quartile exact", Plot.GetAxisLowerExtent(1.25), 1));
        GD.Print(Summarize("Ones second quartile", Plot.GetAxisLowerExtent(1.3), 1));
        GD.Print(Summarize("Ones third quartile exact", Plot.GetAxisLowerExtent(1.5), 1));
        GD.Print(Summarize("Ones third quartile", Plot.GetAxisLowerExtent(1.6), 1));
        GD.Print(Summarize("Ones fourth quartile exact", Plot.GetAxisLowerExtent(1.75), 1));
        GD.Print(Summarize("Ones fourth quartile", Plot.GetAxisLowerExtent(1.8), 1));
        GD.Print(Summarize("Ones first quartile exact again", Plot.GetAxisLowerExtent(2), 2));
        GD.Print(Summarize("Tens first quartile exact", Plot.GetAxisLowerExtent(10), 10));
        GD.Print(Summarize("Tens first quartile", Plot.GetAxisLowerExtent(11), 10));
        GD.Print(Summarize("Tens second quartile exact", Plot.GetAxisLowerExtent(12.5), 10));
        GD.Print(Summarize("Tens second quartile", Plot.GetAxisLowerExtent(13), 10));
        GD.Print(Summarize("Tens third quartile exact", Plot.GetAxisLowerExtent(15), 10));
        GD.Print(Summarize("Tens third quartile", Plot.GetAxisLowerExtent(16), 10));
        GD.Print(Summarize("Tens fourth quartile exact", Plot.GetAxisLowerExtent(17.5), 10));
        GD.Print(Summarize("Tens fourth quartile", Plot.GetAxisLowerExtent(18), 10));
        GD.Print(Summarize("Tens first quartile exact again", Plot.GetAxisLowerExtent(20), 20));
        GD.Print(Summarize("Negative tenths first quartile exact", Plot.GetAxisLowerExtent(-0.1), -0.1));
        GD.Print(Summarize("Negative tenths first quartile", Plot.GetAxisLowerExtent(-0.11), -0.2));
        GD.Print(Summarize("Negative tenths second quartile exact", Plot.GetAxisLowerExtent(-0.125), -0.2));
        GD.Print(Summarize("Negative tenths second quartile", Plot.GetAxisLowerExtent(-0.13), -0.2));
        GD.Print(Summarize("Negative tenths third quartile exact", Plot.GetAxisLowerExtent(-0.15), -0.2));
        GD.Print(Summarize("Negative tenths third quartile", Plot.GetAxisLowerExtent(-0.16), -0.2));
        GD.Print(Summarize("Negative tenths fourth quartile exact", Plot.GetAxisLowerExtent(-0.175), -0.2));
        GD.Print(Summarize("Negative tenths fourth quartile", Plot.GetAxisLowerExtent(-0.18), -0.2));
        GD.Print(Summarize("Negative tenths first quartile exact again", Plot.GetAxisLowerExtent(-0.2), -0.2));
        GD.Print(Summarize("Negative ones first quartile exact", Plot.GetAxisLowerExtent(-1), -1));
        GD.Print(Summarize("Negative ones first quartile", Plot.GetAxisLowerExtent(-1.1), -2));
        GD.Print(Summarize("Negative ones second quartile exact", Plot.GetAxisLowerExtent(-1.25), -2));
        GD.Print(Summarize("Negative ones second quartile", Plot.GetAxisLowerExtent(-1.3), -2));
        GD.Print(Summarize("Negative ones third quartile exact", Plot.GetAxisLowerExtent(-1.5), -2));
        GD.Print(Summarize("Negative ones third quartile", Plot.GetAxisLowerExtent(-1.6), -2));
        GD.Print(Summarize("Negative ones fourth quartile exact", Plot.GetAxisLowerExtent(-1.75), -2));
        GD.Print(Summarize("Negative ones fourth quartile", Plot.GetAxisLowerExtent(-1.8), -2));
        GD.Print(Summarize("Negative ones first quartile exact again", Plot.GetAxisLowerExtent(-2), -2));
        GD.Print(Summarize("Negative tens first quartile exact", Plot.GetAxisLowerExtent(-10), -10));
        GD.Print(Summarize("Negative tens first quartile", Plot.GetAxisLowerExtent(-11), -20));
        GD.Print(Summarize("Negative tens second quartile exact", Plot.GetAxisLowerExtent(-12.5), -20));
        GD.Print(Summarize("Negative tens second quartile", Plot.GetAxisLowerExtent(-13), -20));
        GD.Print(Summarize("Negative tens third quartile exact", Plot.GetAxisLowerExtent(-15), -20));
        GD.Print(Summarize("Negative tens third quartile", Plot.GetAxisLowerExtent(-16), -20));
        GD.Print(Summarize("Negative tens fourth quartile exact", Plot.GetAxisLowerExtent(-17.5), -20));
        GD.Print(Summarize("Negative tens fourth quartile", Plot.GetAxisLowerExtent(-18), -20));
        GD.Print(Summarize("Negative tens first quartile exact again", Plot.GetAxisLowerExtent(-20), -20));
    }

    private void TestPlotGetUpperExtent()
    {
        GD.Print("---GetAxisUpperExtent Tests---");
        GD.Print(Summarize("Zero", Plot.GetAxisUpperExtent(0), 0));
        GD.Print(Summarize("Tenths first quartile exact", Plot.GetAxisUpperExtent(0.1), 0.1));
        GD.Print(Summarize("Tenths first quartile", Plot.GetAxisUpperExtent(0.11), 0.2));
        GD.Print(Summarize("Tenths second quartile exact", Plot.GetAxisUpperExtent(0.125), 0.2));
        GD.Print(Summarize("Tenths second quartile", Plot.GetAxisUpperExtent(0.13), 0.2));
        GD.Print(Summarize("Tenths third quartile exact", Plot.GetAxisUpperExtent(0.15), 0.2));
        GD.Print(Summarize("Tenths third quartile", Plot.GetAxisUpperExtent(0.16), 0.2));
        GD.Print(Summarize("Tenths fourth quartile exact", Plot.GetAxisUpperExtent(0.175), 0.2));
        GD.Print(Summarize("Tenths fourth quartile", Plot.GetAxisUpperExtent(0.18), 0.2));
        GD.Print(Summarize("Tenths first quartile exact again", Plot.GetAxisUpperExtent(0.2), 0.2));
        GD.Print(Summarize("Ones first quartile exact", Plot.GetAxisUpperExtent(1), 1));
        GD.Print(Summarize("Ones first quartile", Plot.GetAxisUpperExtent(1.1), 2));
        GD.Print(Summarize("Ones second quartile exact", Plot.GetAxisUpperExtent(1.25), 2));
        GD.Print(Summarize("Ones second quartile", Plot.GetAxisUpperExtent(1.3), 2));
        GD.Print(Summarize("Ones third quartile exact", Plot.GetAxisUpperExtent(1.5), 2));
        GD.Print(Summarize("Ones third quartile", Plot.GetAxisUpperExtent(1.6), 2));
        GD.Print(Summarize("Ones fourth quartile exact", Plot.GetAxisUpperExtent(1.75), 2));
        GD.Print(Summarize("Ones fourth quartile", Plot.GetAxisUpperExtent(1.8), 2));
        GD.Print(Summarize("Ones first quartile exact again", Plot.GetAxisUpperExtent(2), 2));
        GD.Print(Summarize("Tens first quartile exact", Plot.GetAxisUpperExtent(10), 10));
        GD.Print(Summarize("Tens first quartile", Plot.GetAxisUpperExtent(11), 20));
        GD.Print(Summarize("Tens second quartile exact", Plot.GetAxisUpperExtent(12.5), 20));
        GD.Print(Summarize("Tens second quartile", Plot.GetAxisUpperExtent(13), 20));
        GD.Print(Summarize("Tens third quartile exact", Plot.GetAxisUpperExtent(15), 20));
        GD.Print(Summarize("Tens third quartile", Plot.GetAxisUpperExtent(16), 20));
        GD.Print(Summarize("Tens fourth quartile exact", Plot.GetAxisUpperExtent(17.5), 20));
        GD.Print(Summarize("Tens fourth quartile", Plot.GetAxisUpperExtent(18), 20));
        GD.Print(Summarize("Tens first quartile exact again", Plot.GetAxisUpperExtent(20), 20));
        GD.Print(Summarize("Negative tenths first quartile exact", Plot.GetAxisUpperExtent(-0.1), -0.1));
        GD.Print(Summarize("Negative tenths first quartile", Plot.GetAxisUpperExtent(-0.11), -0.1));
        GD.Print(Summarize("Negative tenths second quartile exact", Plot.GetAxisUpperExtent(-0.125), -0.1));
        GD.Print(Summarize("Negative tenths second quartile", Plot.GetAxisUpperExtent(-0.13), -0.1));
        GD.Print(Summarize("Negative tenths third quartile exact", Plot.GetAxisUpperExtent(-0.15), -0.1));
        GD.Print(Summarize("Negative tenths third quartile", Plot.GetAxisUpperExtent(-0.16), -0.1));
        GD.Print(Summarize("Negative tenths fourth quartile exact", Plot.GetAxisUpperExtent(-0.175), -0.1));
        GD.Print(Summarize("Negative tenths fourth quartile", Plot.GetAxisUpperExtent(-0.18), -0.1));
        GD.Print(Summarize("Negative tenths first quartile exact again", Plot.GetAxisUpperExtent(-0.2), -0.2));
        GD.Print(Summarize("Negative ones first quartile exact", Plot.GetAxisUpperExtent(-1), -1));
        GD.Print(Summarize("Negative ones first quartile", Plot.GetAxisUpperExtent(-1.1), -1));
        GD.Print(Summarize("Negative ones second quartile exact", Plot.GetAxisUpperExtent(-1.25), -1));
        GD.Print(Summarize("Negative ones second quartile", Plot.GetAxisUpperExtent(-1.3), -1));
        GD.Print(Summarize("Negative ones third quartile exact", Plot.GetAxisUpperExtent(-1.5), -1));
        GD.Print(Summarize("Negative ones third quartile", Plot.GetAxisUpperExtent(-1.6), -1));
        GD.Print(Summarize("Negative ones fourth quartile exact", Plot.GetAxisUpperExtent(-1.75), -1));
        GD.Print(Summarize("Negative ones fourth quartile", Plot.GetAxisUpperExtent(-1.8), -1));
        GD.Print(Summarize("Negative ones first quartile exact again", Plot.GetAxisUpperExtent(-2), -2));
        GD.Print(Summarize("Negative tens first quartile exact", Plot.GetAxisUpperExtent(-10), -10));
        GD.Print(Summarize("Negative tens first quartile", Plot.GetAxisUpperExtent(-11), -10));
        GD.Print(Summarize("Negative tens second quartile exact", Plot.GetAxisUpperExtent(-12.5), -10));
        GD.Print(Summarize("Negative tens second quartile", Plot.GetAxisUpperExtent(-13), -10));
        GD.Print(Summarize("Negative tens third quartile exact", Plot.GetAxisUpperExtent(-15), -10));
        GD.Print(Summarize("Negative tens third quartile", Plot.GetAxisUpperExtent(-16), -10));
        GD.Print(Summarize("Negative tens fourth quartile exact", Plot.GetAxisUpperExtent(-17.5), -10));
        GD.Print(Summarize("Negative tens fourth quartile", Plot.GetAxisUpperExtent(-18), -10));
        GD.Print(Summarize("Negative tens first quartile exact again", Plot.GetAxisUpperExtent(-20), -20));
    }

    private float[][] GenerateVelocityData(
            double v0 = 0, double a = 0, double d = 0, double t = 5, double dt = 1.0 / 60)
    {
        int frames = (int)(t / dt) + 1;
        float[] ts = new float[frames];
        float[] vs = new float[frames];

        ts[0] = 0;
        vs[0] = (float)v0;

        for (int i = 1; i < frames; i++)
        {
            ts[i] = (float)(i * dt);
            // vs[i] = (float)Kinematics.Velocity(vs[i - 1], dt, a, d);
            vs[i] = (float)Kinematics.Velocity(v0, ts[i], a, d);
        }

        GD.Print(vs[150]);

        return new float[][] { ts, vs };
    }

    private Graph CreateGraph(
            float[] xSeries, float[] ySeries, string title, string xLabel = "Time", string yLabel = "Velocity")
    {
        Graph graph = (Graph)graphScene.Instance();

        graph.Title = title;
        graph.XAxisLabel = xLabel;
        graph.YAxisLabel = yLabel;
        graph.XSeries = xSeries;
        graph.YSeries = ySeries;

        return graph;
    }

    private Graph CreateGraph(float[][] data, string title, string xLabel = "Time", string yLabel = "Velocity")
    {
        if (data.Length < 2) throw new ArgumentOutOfRangeException("data must contain 2 array elements.");
        return CreateGraph(data[0], data[1], title, xLabel, yLabel);
    }
}
