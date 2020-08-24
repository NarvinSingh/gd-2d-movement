using Com.NarvinSingh.Physics;
using Com.NarvinSingh.Test;
using Godot;
using System;

public class KinematicsTest : Node2D
{
    private Carousel carousel;
    private PackedScene graphScene;

    public override void _Ready()
    {
        // lib class tests
        GD.Print(Summarize("ApproximateTest.ItCalculatesIsEqual", ApproximateTest.ItCalculatesIsEqual()));
        GD.Print(Summarize("ApproximateTest.ItReimannSums", ApproximateTest.ItReimannSums()));
        GD.Print(Summarize("ApproximateTest.ItReimannSumsTo", ApproximateTest.ItReimannSumsTo()));

        GD.Print(Summarize("KinematicsTest.ItCalculatesTimeToStop", Test.KinematicsTest.ItCalculatesTimeToStop()));
        GD.Print(Summarize("KinematicsTest.ItCalculatesVelocity", Test.KinematicsTest.ItCalculatesVelocity()));
        GD.Print(Summarize(
                "KinematicsTest.ItCalculatesTerminalVelocity", Test.KinematicsTest.ItCalculatesTerminalVelocity()));

        GD.Print(Summarize("RangeTest.ItInstantiates", RangeTest.ItInstantiates()));
        GD.Print(Summarize("RangeTest.ItIncludes", RangeTest.ItIncludes()));

        GD.Print(Summarize("AxisTest.ItCalcsExtents", AxisTest.ItCalcsExtents()));
        GD.Print(Summarize("AxisTest.ItInstantiates", AxisTest.ItInstantiates()));
        GD.Print(Summarize("AxisTest.ItMaps", AxisTest.ItMaps()));
        GD.Print(Summarize("AxisTest.ItUnmaps", AxisTest.ItUnmaps()));

        // Graph tests
        carousel = (Carousel)GetNode("Carousel");
        graphScene = (PackedScene)ResourceLoader.Load("res://nodes/Graph.tscn");

        carousel.AddNode(CreateGraph(new float[][] { new float[] { }, new float[] { } }, "1) NULL", "x", "y"));
        carousel.AddNode(CreateGraph(new float[][] { new float[] { 1 }, new float[] { 1 } }, "2) (1, 1)", "x", "y"));
        carousel.AddNode(
                CreateGraph(new float[][] { new float[] { -1 }, new float[] { -1 } }, "3) (-1, -1)", "x", "y"));
        carousel.AddNode(CreateGraph(new float[][] { new float[] { 1 }, new float[] { -1 } }, "4) (1, -1)", "x", "y"));
        carousel.AddNode(CreateGraph(new float[][] { new float[] { -1 }, new float[] { 1 } }, "5) (-1, 1)", "x", "y"));
        carousel.AddNode(CreateGraph(new float[][] { new float[] { 1, 2 }, new float[] { 1, 2 } }, "6) Q1", "x", "y"));
        carousel.AddNode(
                CreateGraph(new float[][] { new float[] { -2, -1 }, new float[] { 1, 2 } }, "7) Q2", "x", "y"));
        carousel.AddNode(
                CreateGraph(new float[][] { new float[] { -2, -1 }, new float[] { -2, -1 } }, "8) Q3", "x", "y"));
        carousel.AddNode(
                CreateGraph(new float[][] { new float[] { 1, 2 }, new float[] { -2, -1 } }, "9) Q4", "x", "y"));
        carousel.AddNode(CreateGraph(new float[][] {
                new float[] { -10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } },
                "10) y = 1", "x", "y", true, false));
        carousel.AddNode(CreateGraph(new float[][] {
                new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
                new float[] { -10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } },
                "11) x = 1", "x", "y", false));
        carousel.AddNode(CreateGraph(new float[][] {
                new float[] { -10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 },
                new float[] { -10, -9, -8, -7, -6, -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 } },
                "12) y = x", "x", "y"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(0, 100, 0.01), "13) v0 = 0, a = 100, d = 0.01"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(0, -100, 0.01), "14) v0 = 0, a = -100, d = 0.01"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(200, 100, 0.01), "15) v0 = 200, a = 100, d = 0.01"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(200, -100, 0.01), "16) v0 = 200, a = -100, d = 0.01"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(-200, 100, 0.01), "17) v0 = -200, a = 100, d = 0.01"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(-200, -100, 0.01), "18) v0 = -200, a = -100, d = 0.01"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(0, 100), "19) v0 = 0, a = 100"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(0, -100), "20) v0 = 0, a = -100"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(15, 10), "21) v0 = 15, a = 10"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(15, -10), "22) v0 = 15, a = -10"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(-15, 10), "23) v0 = -15, a = 10"));
        carousel.AddNode(CreateGraph(GenerateVelocityData(-15, -10), "24) v0 = -15, a = -10"));
        carousel.SetActiveNode(0);

        //GD.Print("Goodbye");
        //GetTree().Quit();
    }

    private string Summarize(string description, bool isPass)
    {
        if (isPass) return String.Format("Pass {0}", description);
        return String.Format("FAIL {0}", description);
    }

    private string Summarize(string description, double actual, double expected, int precision = 10)
    {
        if (Math.Round(actual, precision) == Math.Round(expected, precision))
        {
            return String.Format("Pass {0}: {1}", description, actual);
        }

        return String.Format("FAIL {0}: actual = {1}, expected = {2}", description, actual, expected);
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
            vs[i] = (float)Kinematics.Velocity(ts[i], v0, a, d);
        }

        return new float[][] { ts, vs };
    }

    private Graph CreateGraph(float[] xSeries, float[] ySeries, string title, string xLabel = "Time",
            string yLabel = "Velocity", bool xSnap = true, bool ySnap = true)
    {
        Graph graph = (Graph)graphScene.Instance();
        int seriesLen = xSeries.Length;
        Vector2[] series = new Vector2[seriesLen];

        for (int i = 0; i < seriesLen; i++) series[i] = new Vector2(xSeries[i], ySeries[i]);

        graph.Series = series;
        graph.Title = title;
        graph.XAxisLabel = xLabel;
        graph.YAxisLabel = yLabel;
        graph.XSnap = xSnap;
        graph.YSnap = ySnap;

        return graph;
    }

    private Graph CreateGraph(float[][] data, string title, string xLabel = "Time", string yLabel = "Velocity",
            bool xSnap = true, bool ySnap = true)
    {
        if (data.Length < 2) throw new ArgumentOutOfRangeException("data must contain 2 array elements.");
        return CreateGraph(data[0], data[1], title, xLabel, yLabel, xSnap, ySnap);
    }
}
