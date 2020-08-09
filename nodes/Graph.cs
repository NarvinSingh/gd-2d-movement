using Godot;
using System;
using Physics;

public class Graph : Control
{
    private String title;
    private String xAxis;
    private String yAxis;
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

    [Export] public String Title
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

    [Export] public String XAxis
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

    [Export] public String YAxis
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
        // Run(0, 100);
        // Run("Negative Acceleration, No Drag", 0, -100);
        // Run("Positive Velocity, Positive Acceleration, No Drag", 250, 100);
        // Run(250, -100);
        // Run("Negative Velocity, Positive Acceleration, No Drag", -600, 100);
        // Run(100);
        // Run(0);
        // Run(-100);
        // Run(-250, 100);
        // Run(250, -100);
        // Run(0, 100, 0.01);
        // Run(200, 100, 0.01);
        // Run(200, -100, 0.01, Kinematics.StopTime(200, -100, 0.01));
        // Run(200, -100, 0.01, 2);
    }

    private Vector2 Map(float x, float y, float xScale, float yScale, float yOffset)
    {
        return new Vector2(origin.x + x * xScale, origin.y - (y - yOffset) * yScale);
    }

    private void Plot(float x, float y, float xScale, float yScale, float yOffset)
    {
        Sprite newPoint = (Sprite)point.Instance();
        newPoint.Position = Map(x, y, xScale, yScale, yOffset);
        AddChild(newPoint);
    }

    private void Plot(float x, float y)
    {
        Sprite newPoint = (Sprite)point.Instance();
        newPoint.Position = new Vector2(x, y);
        AddChild(newPoint);
    }

    private void Run1(double v0, double a = 0, double d = 0, double t = 5.0, double dt = 1.0 / 60)
    {
        int frames = (int)(t / dt) + 1;
        double v = v0;
        double vMin = v0;
        double vMax = v0;
        double[] vs = new double[frames];

        vs[0] = v0;

        for (int i = 1; i < frames; i++)
        {
            v = Kinematics.Velocity(v, dt, a, d);
            vs[i] = v;
            if (v < vMin) vMin = v;
            else if (v > vMax) vMax = v;
        }

        float xScale = xAxisLen / frames;
        float yScale = vMax > vMin ? yAxisLen / (float)(vMax - vMin) : yAxisLen / (float)vMax;
        float yOffset = vMin != vMax ? (float)vMin : 0;

        if (vMin < 0)
        {
            xLine.Translate(new Vector2(0, (float)vMin));
        }

        xMaxLabel.Text = String.Format("{0:F2}", t);
        yMinLabel.Text = String.Format("{0:F2}", vMin != vMax ? vMin : 0);
        yMaxLabel.Text = String.Format("{0:F2}", vMax);

        for (int i = 0; i < frames; i++)
        {
            Plot(i, (float)vs[i], xScale, yScale, yOffset);
        }
    }

    private void Run2(double v0, double a = 0, double d = 0, double t = 5.0, double dt = 1.0 / 60)
    {
        int frames = (int)(t / dt) + 1;
        double v = v0;
        double vMin = v0;
        double vMax = v0;
        double[] vs = new double[frames];

        vs[0] = v0;

        for (int i = 1; i < frames; i++)
        {
            v = Kinematics.Velocity(v, dt, a, d);
            vs[i] = v;
            if (v < vMin) vMin = v;
            else if (v > vMax) vMax = v;
        }

        float xScale = xAxisLen / frames;
        float yScale;
        float yOffset;
        float yMinValue;
        float yMaxValue;

        if (vMax != vMin)
        {
            yScale = yAxisLen / (float)Math.Abs(vMax - vMin);
            yOffset = (float)vMin;
            yMinValue = (float)vMin;
            yMaxValue = (float)vMax;
        }
        else
        {
            if (vMax != 0)
            {
                yScale = yAxisLen / (float)Math.Abs(vMax);
                yMaxValue = (float)vMax;
            }
            else
            {
                yScale = 1;
                yMaxValue = 1;
            }

            yOffset = 0;
            yMinValue = 0;
        }

        if (vMin < 0)
        {
            xLine.Translate(new Vector2(0, (float)vMin));
        }

        xMaxLabel.Text = String.Format("{0:F2}", t);
        yMinLabel.Text = String.Format("{0:F2}", yMinValue);
        yMaxLabel.Text = String.Format("{0:F2}", yMaxValue);

        for (int i = 0; i < frames; i++)
        {
            Plot(i, (float)vs[i], xScale, yScale, yOffset);
        }
    }

    private void Run(
            double v0 = 0, double a = 0, double d = 0, double t = 5.0, double dt = 1.0 / 60, String title = null)
    {
        int frames = (int)(t / dt) + 1;
        double v = v0;
        double vMin = v0;
        double vMax = v0;
        double[] vs = new double[frames];

        vs[0] = v0;

        for (int i = 1; i < frames; i++)
        {
            v = Kinematics.Velocity(v, dt, a, d);
            vs[i] = v;
            if (v < vMin) vMin = v;
            else if (v > vMax) vMax = v;
        }

        float xScale = xAxisLen / frames;
        float yScale;
        float yOffset;
        float yMinValue;
        float yMaxValue;

        if (vMax != vMin)
        {
            yScale = yAxisLen / (float)Math.Abs(vMax - vMin);
            yOffset = (float)vMin;
            yMinValue = (float)vMin;
            yMaxValue = (float)vMax;
        }
        else
        {
            if (vMax != 0)
            {
                yScale = yAxisLen / (float)Math.Abs(vMax);
                yMaxValue = (float)vMax;
            }
            else
            {
                yScale = 1;
                yMaxValue = 1;
            }

            yOffset = 0;
            yMinValue = 0;
        }

        if (vMin < 0)
        {
            // xAxis.Translate(new Vector2(0, (float)vMin));
        }

        // this.title.Text = title ?? String.Format("v0 = {0:F2}, a = {1:F2}, d = {2:F2}", v0, a, d);
        xMaxLabel.Text = String.Format("{0:F2}", t);
        yMinLabel.Text = String.Format("{0:F2}", vMin);
        yMaxLabel.Text = String.Format("{0:F2}", vMax);

        if (vMin == vMax && vMin < 0) xLine.Translate(new Vector2(0, -yAxisLen));
        else if (vMax < 0) xLine.Visible = false;
        else if (vMin < 0) xLine.Translate(new Vector2(0, (float)vMin));
        else if (vMin > 0) xLine.Visible = false;

        for (int i = 0; i < frames; i++)
        {
            if (vMax != vMin)
            {
                Plot(
                    origin.x + (float)(i * dt * xAxisLen / t),
                    origin.y - (float)((vs[i] - vMin) * yAxisLen / (vMax - vMin)));
            }
            else if (vMax > 0)
            {
                Plot(
                    origin.x + (float)(i * dt * xAxisLen / t),
                    origin.y - yAxisLen);
            }
            else
            {
                Plot(
                    origin.x + (float)(i * dt * xAxisLen / t),
                    origin.y);
            }
        }
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
