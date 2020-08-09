using Godot;
using System;
using System.Collections.Generic;
using Physics;

public class KinematicsTest : Node2D
{
    private PackedScene graphScene;
    private readonly List<Graph> graphs = new List<Graph>();
    private int activeGraphIndex;

    public override void _Ready()
    {
        graphScene = (PackedScene)ResourceLoader.Load("res://nodes/Graph.tscn");
        graphs.Add(CreateGraph(RunVelocity(0, 100), "v0 = 0, a = 100"));
        graphs.Add(CreateGraph(RunVelocity(0, -100), "v0 = 0, a = -100"));
        graphs.Add(CreateGraph(RunVelocity(250, 100), "v0 = 250, a = 100"));
        SetActiveGraph(0);
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
            double ti = i * dt;
            ts[i] = (float)ti;
            vs[i] = (float)Kinematics.Velocity(vs[i - 1], dt, a, d);
            //vs[i] = (float)Kinematics.Velocity(v0, ti, a, d);
        }

        return new float[][] { ts, vs };
    }

    private Graph CreateGraph(float[] xSeries, float[] ySeries, String title)
    {
        Graph graph = (Graph)graphScene.Instance();

        graph.Title = title;
        graph.XSeries = xSeries;
        graph.YSeries = ySeries;

        return graph;
    }

    private Graph CreateGraph(float[][] data, String title)
    {
        if (data.Length < 2) throw new ArgumentOutOfRangeException("data must contain 2 array elements.");
        return CreateGraph(data[0], data[1], title);
    }

    private void SetActiveGraph(int index)
    {
        RemoveChild(GetNode("Graph"));
        AddChild(graphs[index]);
        MoveChild(graphs[index], 0);
        activeGraphIndex = index;
    }

    private void HandlePrevButtonPressed()
    {
        if (graphs.Count > 1)
        {
            if (activeGraphIndex > 0) SetActiveGraph(activeGraphIndex - 1);
            else SetActiveGraph(graphs.Count - 1);
        }
    }

    private void HandleNextButtonPressed()
    {
        if (graphs.Count > 1)
        {
            if (activeGraphIndex < graphs.Count - 1) SetActiveGraph(activeGraphIndex + 1);
            else SetActiveGraph(0);
        }
    }
}
