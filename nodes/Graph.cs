using Godot;
using System;
using Graphing;

public class Graph : Control
{
    private string title;
    private string xAxisLabel;
    private string yAxisLabel;
    private Label titleLabel;
    private Node2D xAxis;
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

    [Export] public string XAxisLabel
    {
        get
        {
            return xAxisLabel;
        }
        set
        {
            xAxisLabel = value;
            if (!(xLabel is null)) xLabel.Text = xAxisLabel;
        }
    }

    [Export] public string YAxisLabel
    {
        get
        {
            return yAxisLabel;
        }
        set
        {
            yAxisLabel = value;
            if (!(yLabel is null)) yLabel.Text = yAxisLabel;
        }
    }

    [Export] public float[] XSeries { get; set; }
    [Export] public float[] YSeries { get; set; }

    public override void _Ready()
    {
        // Child nodes
        titleLabel = (Label)GetNode("Title");
        xAxis = (Node2D)GetNode("XAxis");
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
        xLabel.Text = xAxisLabel;
        yLabel.Text = yAxisLabel;
        xMinLabel.Visible = false;

        Plot();
    }

    private void CreatePoint(float x, float y)
    {
            Sprite newPoint = (Sprite)point.Instance();
            newPoint.Position = new Vector2(x, y);
            AddChild(newPoint);
    }

    private void Plot()
    {
        int frames = XSeries.Length;
        Plot.MinMax xMinMax = new Plot.MinMax(XSeries[0], XSeries[XSeries.Length - 1]);
        Plot.MinMax yMinMax = Graphing.Plot.GetMinMax(YSeries);
        float yLowerExtent;
        float yUpperExtent;

        if (yMinMax.Min != yMinMax.Max)
        {
            yLowerExtent = Math.Min((float)Graphing.Plot.GetAxisLowerExtent(yMinMax.Min), 0);
            yUpperExtent = Math.Max((float)Graphing.Plot.GetAxisUpperExtent(yMinMax.Max), 0);
        }
        else
        {
            if (yMinMax.Min == 0)
            {
                yLowerExtent = 0;
                yUpperExtent = 1;
            }
            else if (yMinMax.Min > 0)
            {
                yLowerExtent = (float)yMinMax.Min * 2;
                yUpperExtent = 0;
            }
            else
            {
                yLowerExtent = 0;
                yUpperExtent = (float)yMinMax.Min * 2;
            }
        }

        xMaxLabel.Text = String.Format("{0:F2}", xMinMax.Max);
        yMinLabel.Text = String.Format("{0:F2}", yLowerExtent);
        yMaxLabel.Text = String.Format("{0:F2}", yUpperExtent);

        if (yLowerExtent != 0) xAxis.Translate(new Vector2(0, yLowerExtent * yAxisLen / (yUpperExtent - yLowerExtent)));

        for (int i = 0; i < frames; i++)
        {
            CreatePoint((float)Graphing.Plot.TranslateX(XSeries[i], origin.x, xAxisLen, 0, 5),
                    (float)Graphing.Plot.TranslateY(YSeries[i], origin.y, yAxisLen, yLowerExtent, yUpperExtent));
        }
    }
}
