using Godot;
using System;
using System.Collections.Generic;
using Physics;

public class Test : Node2D
{
    private PackedScene graphScene;
    private List<Graph> graphs = new List<Graph>();
    private int currentGraph;

    public override void _Ready()
    {
        graphScene = (PackedScene)ResourceLoader.Load("res://nodes/Graph.tscn");
        AddGraph(0, 100);
        AddGraph(0, -100);
        AddGraph(250, 100);
        AddChild(graphs[0]);
        MoveChild(graphs[0], 0);
    }

    private float[][] RunVelocity(double v0 = 0, double a = 0, double d = 0, double t = 5.0, double dt = 1.0 / 60)
    {
        int frames = (int)(t / dt) + 1;
        float[] ts = new float[frames];
        float[] vs = new float[frames];

        ts[0] = 0;
        vs[0] = (float)v0;
        for (int i = 1; i < frames; i++)
        {
            ts[i] = i * (float)dt;
            vs[i] = (float)Kinematics.Velocity2(vs[i - 1], dt, a, d);
        }

        return new float[][] { ts, vs };
    }

    private void AddGraph(
            double v0 = 0, double a = 0, double d = 0, double t = 5.0, double dt = 1.0 / 60, String title = null)
    {
        Graph graph = (Graph)graphScene.Instance();
        float[][] data = RunVelocity(v0, a, d, t, dt);
        graph.Title = title is null ? String.Format("v0 = {0:F2}, a = {1:F2}, d = {2:F2}", v0, a, d) : title;
        graph.XSeries = data[0];
        graph.YSeries = data[1];
        graphs.Add(graph);
    }

    private void ReplaceGraph()
    {
        RemoveChild(GetNode("Graph"));
        AddChild(graphs[currentGraph]);
        MoveChild(graphs[currentGraph], 0);
    }

    private void HandlePrevButtonPressed()
    {
        if (graphs.Count > 1)
        {
            if (currentGraph > 0) currentGraph -= 1;
            else currentGraph = graphs.Count - 1;
            ReplaceGraph();
        }
    }

    private void HandleNextButtonPressed()
    {
        if (graphs.Count > 1)
        {
            if (currentGraph < graphs.Count - 1) currentGraph += 1;
            else currentGraph = 0;
            ReplaceGraph();
        }
    }
}
