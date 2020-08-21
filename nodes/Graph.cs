using Com.NarvinSingh.Graphing;
using Godot;
using System;
using System.Collections.Generic;

public class Graph : Area2D
{
    private const string DEFAULT_POINT_HIGHLIGHT_COLOR = "6680FF";
    private const float POINT_SCALE = 1.2F;
    private const float CHI_OFFSET = 6;

    private bool isReady;
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
    private Node2D yAxis;
    private Line2D yLine;
    private Label yLabel;
    private Label yMinLabel;
    private Label yMaxLabel;
    private float yAxisLen;
    private Vector2 origin;
    private Node2D crosshair;
    private Line2D xCrosshair;
    private Line2D yCrosshair;
    private Label crosshairInfo;
    private float chiExpMgnTop;
    private float chiExpMgnLeft;
    private float chiExpMgnBtm;
    private float chiExpMgnRight;
    private bool isCrosshairActive;
    private PackedScene point;
    private Color pointColor;
    private List<Sprite> points;
    private Vector2 pointScale;
    private Sprite activePoint;

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

    [Export] public float[] XSeries { get; set; }

    [Export] public float[] YSeries { get; set; }

    [Export] public Color HighlightColor { get; set; } = new Color(DEFAULT_POINT_HIGHLIGHT_COLOR);

    public override void _Ready()
    {
        isReady = true;

        // Get child nodes
        titleLabel = (Label)GetNode("Title");
        xAxis = (Node2D)GetNode("XAxis");
        xLine = (Line2D)GetNode("XAxis/Axis");
        xLabel = (Label)GetNode("XAxis/Label");
        xMinLabel = (Label)GetNode("XAxis/Min");
        xMaxLabel = (Label)GetNode("XAxis/Max");
        yAxis = (Node2D)GetNode("YAxis");
        yLine = (Line2D)GetNode("YAxis/Axis");
        yLabel = (Label)GetNode("YAxis/Label");
        yMinLabel = (Label)GetNode("YAxis/Min");
        yMaxLabel = (Label)GetNode("YAxis/Max");
        crosshair = (Node2D)GetNode("Crosshair");
        xCrosshair = (Line2D)GetNode("Crosshair/XCrosshair");
        yCrosshair = (Line2D)GetNode("Crosshair/YCrosshair");
        crosshairInfo = (Label)GetNode("Crosshair/Info");
        StyleBoxFlat chiStyleBox = (StyleBoxFlat)crosshairInfo.GetStylebox("normal");

        // Initialize class variables
        origin = xLine.Points[0];
        xAxisLen = origin.DistanceTo(xLine.Points[1]);
        yAxisLen = origin.DistanceTo(yLine.Points[1]);
        chiExpMgnTop = chiStyleBox.ExpandMarginTop;
        chiExpMgnLeft = chiStyleBox.ExpandMarginLeft;
        chiExpMgnBtm = chiStyleBox.ExpandMarginBottom;
        chiExpMgnRight = chiStyleBox.ExpandMarginRight;
        point = (PackedScene)ResourceLoader.Load("res://nodes/Point.tscn");
        Sprite tempPoint = (Sprite)point.Instance();
        pointColor = tempPoint.Modulate;
        tempPoint.QueueFree();
        points = new List<Sprite>();
        pointScale = new Vector2(POINT_SCALE, POINT_SCALE);

        // Initialize child nodes
        titleLabel.Text = title;
        xLabel.Text = xAxisLabel;
        yLabel.Text = yAxisLabel;

        Draw();
    }

    private Sprite CreatePoint(float x, float y)
    {
        Sprite newPoint = (Sprite)point.Instance();
        newPoint.Position = new Vector2(x, y);
        AddChild(newPoint);
        return newPoint;
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
        Axis x;
        Axis y;

        // Graph has data
        if (XSeries.Length > 0)
        {
            x = new Axis(origin.x, xAxisLen, XSeries);
            y = new Axis(origin.y - yAxisLen, yAxisLen, YSeries, true);
        }
        // Graph has no data so use default extents
        else
        {
            x = new Axis(origin.x, xAxisLen, 0, 1);
            y = new Axis(origin.y - yAxisLen, yAxisLen, 0, 1, true);
        }

        xMinLabel.Text = String.Format("{0:F2}", x.LowerExtent);
        xMaxLabel.Text = String.Format("{0:F2}", x.UpperExtent);
        yMinLabel.Text = String.Format("{0:F2}", y.LowerExtent);
        yMaxLabel.Text = String.Format("{0:F2}", y.UpperExtent);

        // Translate the x-axis to 0 on the y-axis
        if (y.LowerExtent != 0)
        {
            // The x-axis would be below the lower extent or above the upper extent of the y-axis, so hide it
            if (y.LowerExtent > 0 || y.UpperExtent < 0) xAxis.Visible = false;
            // The x-axis starts off at origin.y so translate it back to screen 0, then translate it to the screen
            // coordinates of 0 on the y-axis.
            else xAxis.Translate(new Vector2(0, -origin.y + y.Map(0)));
        }

        // Translate the y-axis to 0 on the x-axis
        if (x.LowerExtent != 0)
        {
            // The y-axis would be below the lower extent or above the upper extent of the x-axis, so hide it
            if (x.LowerExtent > 0 || x.UpperExtent < 0) yAxis.Visible = false;
            // The y-axis starts off at origin.x so translate it back to screen 0, then translate it to the screen
            // coordinates of 0 on the x-axis.
            else yAxis.Translate(new Vector2(-origin.x + x.Map(0), 0));
        }
        else xMinLabel.Visible = false;

        for (int i = 0; i < frames; i++) points.Add(CreatePoint(x.Map(XSeries[i]), y.Map(YSeries[i])));
    }

    private void SetActivePoint(int i)
    {
        Sprite point = i >= 0 && i < points.Count ? points[i] : null;

        if (activePoint == point) return;

        if (!(activePoint is null))
        {
            activePoint.Scale = Vector2.One;
            activePoint.Modulate = pointColor;
            activePoint.ZIndex = 0;
        }

        activePoint = point;

        if (!(activePoint is null))
        {
            activePoint.Scale = pointScale;
            activePoint.Modulate = HighlightColor;
            activePoint.ZIndex = 1;
        }
    }

    private void ToggleCrosshair(bool? isActive = null)
    {
        bool isActiveFixed = isActive is null ? !isCrosshairActive : (bool)isActive;
        isCrosshairActive = isActiveFixed;
        crosshair.Visible = isActiveFixed;
        if (!isCrosshairActive) SetActivePoint(-1);

    }

    private void PlaceCrosshair(Vector2 position)
    {
        Vector2 chiPosition = position;

        // Graph has data
        if (XSeries.Length > 0)
        {
            int i;
            float yCrosshairPosX;

            // Graph has more than one point, so get the point from the cursor x position and snap
            if (XSeries.Length > 1)
            {
                i = Math.Min(
                    Math.Max(Convert.ToInt32((XSeries.Length - 1) * (position.x - origin.x) / xAxisLen), 0),
                    XSeries.Length - 1);
                yCrosshairPosX = i * xAxisLen / (XSeries.Length - 1);
            }
            // Graph has only one point in the middle of itself
            else
            {
                i = 0;
                yCrosshairPosX = xAxisLen / 2;
            }

            // Set crosshair info text to info about the point, set the crosshair position and active point
            crosshairInfo.Text = String.Format("{0,8}: {1,6}\n{2,8}: {3,6}",
                    xAxisLabel, i >= 0 && i < XSeries.Length ? String.Format("{0:F2}", XSeries[i]) : "N/A",
                    yAxisLabel, i >= 0 && i < YSeries.Length ? String.Format("{0:F2}", YSeries[i]) : "N/A");
            xCrosshair.Position = new Vector2(xCrosshair.Position.x, position.y - origin.y);
            yCrosshair.Position = new Vector2(yCrosshairPosX, yCrosshair.Position.y);
            SetActivePoint(i);
        }
        // Graph has no data
        else
        {
            // Set crosshair info text to info about the cursor, set the crosshair position and active point
            xCrosshair.Position = new Vector2(xCrosshair.Position.x, position.y - origin.y);
            yCrosshair.Position = new Vector2(position.x - origin.x, yCrosshair.Position.y);
            crosshairInfo.Text = String.Format("Cursor: {0}", position);
        }

        // Crosshair info position is set now because it depends on the text that was set above
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
    }

    private void HandleMouseExited()
    {
        ToggleCrosshair(false);
    }
}
