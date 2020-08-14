using Godot;
using System;
using System.Collections.Generic;
using Graphing;

public class Graph : Area2D
{
    private const float POINT_RADIUS = 4;
    private const float CHI_OFFSET = 6;
    private readonly Vector2 POINT_SCALE = new Vector2(1.5F, 1.5F);

    private bool isReady = false;
    private string title;
    private string xAxisLabel;
    private string yAxisLabel;
    private float[] xSeries;
    private float[] ySeries;
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
    private float yLowerExtent;
    private float yUpperExtent;
    private Vector2 origin;
    private Node2D crosshair;
    private Line2D xCrosshair;
    private Line2D yCrosshair;
    private Label crosshairInfo;
    private float chiExpMgnTop;
    private float chiExpMgnLeft;
    private float chiExpMgnBtm;
    private float chiExpMgnRight;
    private bool isCrosshairActive = false;
    private PackedScene point;
    private Color pointColor;
    private float pointRadius;
    private List<Sprite> points;
    private Sprite activePoint = null;

    [Export]
    public string Title
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

    [Export]
    public string XAxisLabel
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

    [Export]
    public string YAxisLabel
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

    [Export]
    public float[] XSeries
    {
        get
        {
            return xSeries;
        }
        set
        {
            xSeries = value;
            Draw();
        }
    }
    [Export]
    public float[] YSeries
    {
        get
        {
            return ySeries;
        }
        set
        {
            ySeries = value;
            Draw();
        }
    }

    [Export] public Color HighlightColor { get; set; } = new Color("6680FF");

    public override void _Ready()
    {
        isReady = true;

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
        crosshair = (Node2D)GetNode("Crosshair");
        xCrosshair = (Line2D)GetNode("Crosshair/XCrosshair");
        yCrosshair = (Line2D)GetNode("Crosshair/YCrosshair");
        crosshairInfo = (Label)GetNode("Crosshair/Info");
        StyleBoxFlat chiStyleBox = (StyleBoxFlat)crosshairInfo.GetStylebox("normal");
        chiExpMgnTop = chiStyleBox.ExpandMarginTop;
        chiExpMgnLeft = chiStyleBox.ExpandMarginLeft;
        chiExpMgnBtm = chiStyleBox.ExpandMarginBottom;
        chiExpMgnRight = chiStyleBox.ExpandMarginRight;
        point = (PackedScene)ResourceLoader.Load("res://nodes/Point.tscn");
        Sprite tempPoint = (Sprite)point.Instance();
        pointColor = tempPoint.Modulate;
        tempPoint.QueueFree();
        points = new List<Sprite>();

        // Class variables
        origin = xLine.Points[0];
        xAxisLen = origin.DistanceTo(xLine.Points[1]);
        yAxisLen = origin.DistanceTo(yLine.Points[1]);

        // Initialize child nodes
        titleLabel.Text = title;
        xLabel.Text = xAxisLabel;
        yLabel.Text = yAxisLabel;
        xMinLabel.Visible = false;

        Draw();
    }

    private Sprite CreatePoint(float x, float y)
    {
        Sprite newPoint = (Sprite)point.Instance();
        newPoint.Position = new Vector2(x, y);
        AddChild(newPoint);
        return newPoint;
    }

    private void SetActivePoint(Sprite point = null)
    {
        if (!(activePoint is null))
        {
            activePoint.Scale = Vector2.One;
            activePoint.Modulate = pointColor;
            activePoint.ZIndex = 0;
        }
        activePoint = point;
        if (!(activePoint is null))
        {
            activePoint.Scale = POINT_SCALE;
            activePoint.Modulate = HighlightColor;
            activePoint.ZIndex = 1;
        }
    }

    private void Erase()
    {
        if (points is null) return;
        points.ForEach((point) => point.QueueFree());
        points.Clear();
    }

    private void Draw()
    {
        if (!isReady || XSeries is null || YSeries is null || XSeries.Length != YSeries.Length) return;

        Erase();

        int frames = XSeries.Length;

        Plot.MinMax xMinMax = new Plot.MinMax(XSeries[0], XSeries[XSeries.Length - 1]);
        Plot.MinMax yMinMax = Plot.GetMinMax(YSeries);

        if (yMinMax.Min != yMinMax.Max)
        {
            yLowerExtent = Math.Min((float)Plot.GetAxisLowerExtent(yMinMax.Min), 0);
            yUpperExtent = Math.Max((float)Plot.GetAxisUpperExtent(yMinMax.Max), 0);
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
            points.Add(CreatePoint((float)Plot.TranslateX(XSeries[i], origin.x, xAxisLen, 0, 5),
                    (float)Plot.TranslateY(YSeries[i], origin.y, yAxisLen, yLowerExtent, yUpperExtent)));
        }
    }

    private void ToggleCrosshair(bool? isActive = null)
    {
        bool isActiveFixed = isActive is null ? !isCrosshairActive : (bool)isActive;
        isCrosshairActive = isActiveFixed;
        crosshair.Visible = isActiveFixed;
        if (!isCrosshairActive) SetActivePoint();

    }

    private void PlaceCrosshair(Vector2 position)
    {
        int i = Math.Min(
                Math.Max(Convert.ToInt32(XSeries.Length * (position.x - origin.x + POINT_RADIUS) / xAxisLen) - 1, 0),
                XSeries.Length - 1);
        string xValue = i >= 0 && i < XSeries.Length ? String.Format("{0:F2}", XSeries[i]) : "N/A";
        string yValue = i >= 0 && i < YSeries.Length ? String.Format("{0:F2}", YSeries[i]) : "N/A";
        Vector2 chiPosition = position;

        crosshairInfo.Text = String.Format("{0,8}: {1,6}\n{2,8}: {3,6}", xAxisLabel, xValue, yAxisLabel, yValue);

        if (position.x + crosshairInfo.RectSize.x + chiExpMgnLeft + chiExpMgnRight + CHI_OFFSET <= origin.x + xAxisLen)
        {
            chiPosition.x += chiExpMgnLeft + CHI_OFFSET;
        }
        else chiPosition.x -= crosshairInfo.RectSize.x + chiExpMgnRight + CHI_OFFSET;

        if (position.y - crosshairInfo.RectSize.y - chiExpMgnTop - chiExpMgnBtm - CHI_OFFSET >= origin.y - yAxisLen)
        {
            chiPosition.y -= crosshairInfo.RectSize.y + chiExpMgnBtm + CHI_OFFSET;
        }
        else chiPosition.y += chiExpMgnTop + CHI_OFFSET;

        crosshairInfo.RectPosition = chiPosition;
        xCrosshair.Position = new Vector2(xCrosshair.Position.x, position.y - origin.y);
        yCrosshair.Position = new Vector2(i * xAxisLen / XSeries.Length, yCrosshair.Position.y);
        SetActivePoint(points[i]);
    }

    private void HandleInputEvent(Node viewport, InputEvent @event, int shapeId)
    {
        if (@event is InputEventMouseButton mouseClick && mouseClick.ButtonIndex == (int)ButtonList.Left
                && mouseClick.Pressed)
        {
            ToggleCrosshair();
            if (isCrosshairActive) PlaceCrosshair(mouseClick.Position);
        }
        else if (@event is InputEventMouseMotion mouseMove && isCrosshairActive) PlaceCrosshair(mouseMove.Position);
        if (@event is InputEventMouseButton mouseClick2 && mouseClick2.ButtonIndex == (int)ButtonList.Right
                && mouseClick2.Pressed)
        {
            XSeries = new float[] { 0, 1, 2, 3, 4 };
            YSeries = new float[] { 0, 1, 4, 9, 16 };
        }
    }

    private void HandleMouseExited()
    {
        ToggleCrosshair(false);
    }
}
