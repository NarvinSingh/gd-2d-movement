using Godot;
using System;
using Physics;

public class Graph : Control
{
    private string title;
    private string xAxis;
    private string yAxis;
    private Label titleLabel;
    private Line2D xLine;
    private Label xLabel;
    private Label xMinLabel;
    private Label xMaxLabel;
    private float xAxisLen;
    private Line2D yLine;
    private Label yLabel;
    private Label yMinLabel;
    private Label yMaxLabel;
    private float yAxisLen;
    private Vector2 origin;
    private PackedScene point;

    [Export] public string Title
    {
        get
        {
            return title;
        }
        set
        {
            title = value;
            if (!(titleLabel is null)) titleLabel.Text = title;
        }
    }

    [Export] public string XAxis
    {
        get
        {
            return xAxis;
        }
        set
        {
            xAxis = value;
            if (!(xLabel is null)) xLabel.Text = xAxis;
        }
    }

    [Export] public string YAxis
    {
        get
        {
            return yAxis;
        }
        set
        {
            yAxis = value;
            if (!(yLabel is null)) yLabel.Text = yAxis;
        }
    }

    [Export] public float[] XSeries { get; set; }
    [Export] public float[] YSeries { get; set; }

    public override void _Ready()
    {
        // Child nodes
        titleLabel = (Label)GetNode("Title");
        xLine = (Line2D)GetNode("XAxis/Axis");
        xLabel = (Label)GetNode("XAxis/Label");
        xMinLabel = (Label)GetNode("XAxis/Min");
        xMaxLabel = (Label)GetNode("XAxis/Max");
        yLine = (Line2D)GetNode("YAxis/Axis");
        yLabel = (Label)GetNode("YAxis/Label");
        yMinLabel = (Label)GetNode("YAxis/Min");
        yMaxLabel = (Label)GetNode("YAxis/Max");
        point = (PackedScene)ResourceLoader.Load("res://nodes/Point.tscn");

        // Class variables
        origin = xLine.Points[0];
        xAxisLen = origin.DistanceTo(xLine.Points[1]);
        yAxisLen = origin.DistanceTo(yLine.Points[1]);

        // Initialize child nodes
        titleLabel.Text = title;
        xLabel.Text = xAxis;
        yLabel.Text = yAxis;

        Run();
    }

    private void Plot(float x, float y)
    {
        Sprite newPoint = (Sprite)point.Instance();
        newPoint.Position = new Vector2(x, y);
        AddChild(newPoint);
    }

    private void Run()
    {
        int frames = XSeries.Length;
        float xMin = XSeries[0];
        float xMax = XSeries[XSeries.Length - 1];
        float yMin = YSeries[0];
        float yMax = YSeries[0];

        foreach (float y in YSeries)
        {
            if (y < yMin) yMin = y;
            else if (y > yMax) yMax = y;
        }

        xMinLabel.Text = String.Format("{0:F2}", xMin);
        xMaxLabel.Text = String.Format("{0:F2}", xMax);
        yMinLabel.Text = String.Format("{0:F2}", yMin);
        yMaxLabel.Text = String.Format("{0:F2}", yMax);

        if (yMin == yMax && yMin < 0) xLine.Translate(new Vector2(0, -yAxisLen));
        else if (yMax < 0) xLine.Visible = false;
        else if (yMin < 0) xLine.Translate(new Vector2(0, yMin));
        else if (yMin > 0) xLine.Visible = false;

        for (int i = 0; i < frames; i++)
        {
            if (yMax != yMin)
            {
                Plot(
                    origin.x + (XSeries[i] * xAxisLen / xMax),
                    origin.y - (YSeries[i] - yMin) * yAxisLen / (yMax - yMin));
            }
            else if (yMax > 0)
            {
                Plot(
                    origin.x + (XSeries[i] * xAxisLen / xMax),
                    origin.y - yAxisLen);
            }
            else
            {
                Plot(
                    origin.x + (XSeries[i] * xAxisLen / xMax),
                    origin.y);
            }
        }
    }
}
