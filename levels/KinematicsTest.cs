using Godot;
using System;
using Com.NarvinSingh.Physics;
using Com.NarvinSingh.Graphing;
using Com.NarvinSingh.Test;

public class KinematicsTest : Carousel
{
    private readonly PackedScene graphScene;

    private Approximate.Fxy VelocityFn(double a, double d)
    {
        return (double dt, double v0) =>
        {
            int phase = (v0 >= 0 && d >= 0) || (v0 < 0 && d < 0) ? 1 : -1;
            return v0 + a * dt - phase * d * v0 * v0 * dt;
        };
    }

    public override void _Ready()
    {
        // long t;
        // int i;
        // double d;
        // float f;
        // Stopwatch w = new Stopwatch();

        // w.Start();
        // for (i = 0; i < 1000000; i++) d = Math.Tan(1.5);
        // w.Stop();
        // t = w.ElapsedTicks;
        // GD.Print(String.Format("Math.Tan {0}", t));

        // w.Start();
        // for (i = 0; i < 1000000; i++) f = (float)Math.Tan(1.5);
        // w.Stop();
        // t = w.ElapsedTicks;
        // GD.Print(String.Format("Math.Tan {0}", t));

        // w.Start();
        // for (i = 0; i < 1000000; i++) f = Mathf.Tan(1.5F);
        // w.Stop();
        // t = w.ElapsedTicks;
        // GD.Print(String.Format("Mathf.Tan {0}", t));

        // GD.Print(Summarize("ApproximateTest.ItCalculatesIsEqual", ApproximateTest.ItCalculatesIsEqual()));
        // GD.Print(Summarize("ApproximateTest.ItReimannSums", ApproximateTest.ItReimannSums()));
        // GD.Print(Summarize("ApproximateTest.ItReimannSumsTo", ApproximateTest.ItReimannSumsTo()));

        //double v0, a, d, t;

        // v0 = 0; a = 100; d = 0.01; t = 1;
        // GD.Print(Approximate.Iterate(VelocityFn(a, d), 0, t, 1.0 / 1000000, v0));
        // v0 = 0; a = -100; d = 0.01; t = 1;
        // GD.Print(Approximate.Iterate(VelocityFn(a, d), 0, t, 1.0 / 1000000, v0));
        // v0 = 50; a = 100; d = 0.01; t = 1;
        // GD.Print(Approximate.Iterate(VelocityFn(a, d), 0, t, 1.0 / 1000000, v0));
        // v0 = -50; a = -100; d = 0.01; t = 1;
        // GD.Print(Approximate.Iterate(VelocityFn(a, d), 0, t, 1.0 / 1000000, v0));
        // v0 = 200; a = -100; d = 0.01; t = 1;
        // GD.Print(Approximate.Iterate(VelocityFn(a, d), 0, t, 1.0 / 1000000, v0));
        // v0 = -200; a = 100; d = 0.01; t = 1;
        // GD.Print(Approximate.Iterate(VelocityFn(a, d), 0, t, 1.0 / 1000000, v0));
        // v0 = 100; a = -100; d = 0.01; t = 1;
        // GD.Print(Approximate.Iterate(VelocityFn(a, d), 0, t, 1.0 / 1000000, v0));
        // v0 = -100; a = 100; d = 0.01; t = 1;
        // GD.Print(Approximate.Iterate(VelocityFn(a, d), 0, t, 1.0 / 1000000, v0));

        //v0 = 100; a = -100; d = 0; t = 5;
        //GD.Print(Approximate.IterateUntil(VelocityFn(a, d), 0, t, v0));
        //v0 = 100; a = -200; d = 0; t = 5;
        //GD.Print(Approximate.IterateUntil(VelocityFn(a, d), 0, t, v0));
        //v0 = 100; a = -10; d = 0; t = 5;
        //GD.Print(Approximate.IterateUntil(VelocityFn(a, d), 0, t, v0));
        //v0 = 100; a = -100; d = 0.01; t = 5;
        //GD.Print(Approximate.IterateUntil(VelocityFn(a, d), 0, t, v0));

        GD.Print(Summarize("KinematicsTest.ItCalculatesTimeToStop", Test.KinematicsTest.ItCalculatesTimeToStop()));
        GD.Print(Summarize("KinematicsTest.ItCalculatesVelocity", Test.KinematicsTest.ItCalculatesVelocity()));

        // GD.Print(Summarize("RangeTest.ItInstantiates", RangeTest.ItInstantiates()));
        // GD.Print(Summarize("RangeTest.ItIncludes", RangeTest.ItIncludes()));

        // GD.Print(Summarize("AxisTest.ItCalcsExtents", AxisTest.ItCalcsExtents()));
        // GD.Print(Summarize("AxisTest.ItInstantiates", AxisTest.ItInstantiates()));
        // GD.Print(Summarize("AxisTest.ItMaps", AxisTest.ItMaps()));
        // GD.Print(Summarize("AxisTest.ItUnmaps", AxisTest.ItUnmaps()));

        // graphScene = (PackedScene)ResourceLoader.Load("res://nodes/Graph.tscn");
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(0, 100, 0.01), "1) v0 = 0, a = 100, d = 0.01"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(0, -100, 0.01), "2) v0 = 0, a = -100, d = 0.01"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(200, 100, 0.01), "3) v0 = 200, a = 100, d = 0.01"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(200, -100, 0.01), "4) v0 = 200, a = -100, d = 0.01"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(-200, 100, 0.01), "5) v0 = -200, a = 100, d = 0.01"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(-200, -100, 0.01), "6) v0 = -200, a = -100, d = 0.01"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(0, 100), "7) v0 = 0, a = 100"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(0, -100), "8) v0 = 0, a = -100"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(15, 10), "9) v0 = 15, a = 10"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(15, -10), "10) v0 = 15, a = -10"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(-15, 10), "11) v0 = -15, a = 10"));
        // carouselNodes.Add(CreateGraph(GenerateVelocityData(-15, -10), "12) v0 = -15, a = -10"));
        // SetActiveNode(0);
        GD.Print("Goodbye");
        GetTree().Quit();
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
            vs[i] = (float)Kinematics.Velocity(v0, ts[i], a, d);
        }

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
