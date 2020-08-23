using Com.NarvinSingh.Graphing;
using Godot;
using System;
using System.Collections.Generic;

public class Graph : Node2D
{
    private const string DEFAULT_POINT_HIGHLIGHT_COLOR = "6680FF";
    private const float POINT_SCALE = 1.2F;
    private const float CHI_OFFSET = 6;

    private string title;
    private string xAxisLabel;
    private string yAxisLabel;
    private Label titleLabel;
    private Area2D plot;
    private Node2D xAxis;
    private Line2D xLine;
    private Label xLabel;
    private Label xMinLabel;
    private Label xMaxLabel;
    private Node2D yAxis;
    private Line2D yLine;
    private Label yLabel;
    private Label yMinLabel;
    private Label yMaxLabel;
    private Node2D crosshair;
    private Line2D xCrosshair;
    private Line2D yCrosshair;
    private Label crosshairInfo;
    private Vector2 origin;
    private Axis axisX;
    private Axis axisY;
    private float xAxisLen;
    private float yAxisLen;
    private float chiExpMgnTop;
    private float chiExpMgnLeft;
    private float chiExpMgnBtm;
    private float chiExpMgnRight;
    private bool isCrosshairActive;
    private bool isReady;
    private List<Sprite> points;
    private Sprite activePoint;
    private PackedScene point;
    private Color pointColor;
    private Vector2 pointScale;

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

    [Export] public bool XSnap { get; set; } = false;

    [Export] public bool YSnap { get; set; } = true;

    [Export] public Color HighlightColor { get; set; } = new Color(DEFAULT_POINT_HIGHLIGHT_COLOR);

    public override void _Ready()
    {
        isReady = true;

        //Get child nodes
        titleLabel = (Label)GetNode("Title");
        plot = (Area2D)GetNode("Plot");
        xAxis = (Node2D)GetNode("Plot/XAxis");
        xLine = (Line2D)GetNode("Plot/XAxis/Axis");
        xLabel = (Label)GetNode("Plot/XAxis/Topmost/Label");
        xMinLabel = (Label)GetNode("Plot/XAxis/Topmost/Min");
        xMaxLabel = (Label)GetNode("Plot/XAxis/Topmost/Max");
        yAxis = (Node2D)GetNode("Plot/YAxis");
        yLine = (Line2D)GetNode("Plot/YAxis/Axis");
        yLabel = (Label)GetNode("Plot/YAxis/Topmost/Label");
        yMinLabel = (Label)GetNode("Plot/YAxis/Topmost/Min");
        yMaxLabel = (Label)GetNode("Plot/YAxis/Topmost/Max");
        crosshair = (Node2D)GetNode("Plot/Crosshair");
        xCrosshair = (Line2D)GetNode("Plot/Crosshair/XLine");
        yCrosshair = (Line2D)GetNode("Plot/Crosshair/YLine");
        crosshairInfo = (Label)GetNode("Plot/Crosshair/Info");
        StyleBoxFlat chiStyleBox = (StyleBoxFlat)crosshairInfo.GetStylebox("normal");

        // Initialize class variables
        origin = plot.Position;
        xAxisLen = xLine.Points[0].DistanceTo(xLine.Points[1]);
        yAxisLen = yLine.Points[0].DistanceTo(yLine.Points[1]);
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
        plot.AddChild(newPoint);
        return newPoint;
    }

    private void Draw()
    {
        if (!isReady || XSeries is null || YSeries is null || XSeries.Length != YSeries.Length) return;

        // Destroy the current points
        if (!(points is null))
        {
            points.ForEach((point) => point.QueueFree());
            points.Clear();
        }

        int frames = XSeries.Length;

        // Graph has data
        if (XSeries.Length > 0)
        {
            axisX = new Axis(0, xAxisLen, XSeries);
            axisY = new Axis(0, yAxisLen, YSeries, true);
        }
        // Graph has no data so use default extents
        else
        {
            axisX = new Axis(0, xAxisLen, -1, 1);
            axisY = new Axis(0, yAxisLen, -1, 1, true);
        }

        // Both axes would be scrolled out of view based on their extents, so adjust the x extents to show the y-axis
        if ((axisY.LowerExtent > 0 || axisY.UpperExtent < 0) && (axisX.LowerExtent > 0 || axisX.UpperExtent < 0))
        {
            if (axisX.LowerExtent > 0) axisX.LowerExtent = 0;
            else axisX.UpperExtent = 0;
        }

        // The x-axis would be below the lower extent or above the upper extent of the y-axis, so hide it
        if (axisY.LowerExtent > 0 || axisY.UpperExtent < 0) xAxis.Visible = false;
        // Translate the x-axis to 0 on the y-axis
        else xAxis.Position = new Vector2(xAxis.Position.x, axisY.Map(0));

        // The y-axis would be below the lower extent or above the upper extent of the x-axis, so hide it
        if (axisX.LowerExtent > 0 || axisX.UpperExtent < 0) yAxis.Visible = false;
        // Translate the y-axis to 0 on the x-axis
        else yAxis.Position = new Vector2(axisX.Map(0), yAxis.Position.y);

        // First make sure all the min/max labels are visible
        xMinLabel.Visible = true;
        xMaxLabel.Visible = true;
        yMinLabel.Visible = true;
        yMaxLabel.Visible = true;

        // Now hide a min or max label if the axes intersect on screen so two labels don't butt up against each other
        if (axisX.LowerExtent == 0) xMinLabel.Visible = false;
        else if (axisY.LowerExtent == 0) yMinLabel.Visible = false;
        else if (axisX.UpperExtent == 0 && axisY.UpperExtent == 0) xMaxLabel.Visible = false;

        xMinLabel.Text = String.Format("{0:F2}", axisX.LowerExtent);
        xMaxLabel.Text = String.Format("{0:F2}", axisX.UpperExtent);
        yMinLabel.Text = String.Format("{0:F2}", axisY.LowerExtent);
        yMaxLabel.Text = String.Format("{0:F2}", axisY.UpperExtent);

        for (int i = 0; i < frames; i++) points.Add(CreatePoint(axisX.Map(XSeries[i]), axisY.Map(YSeries[i])));
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

    private int GetSnapIndex(float[] series, float value)
    {
        float minDistance = float.PositiveInfinity;
        int seriesLength = series.Length;
        int index = 0;

        for (int i = 0; i < seriesLength; i++)
        {
            float dist = series[i] - value;

            if (dist < 0) dist = -dist;
            if (dist < minDistance)
            {
                minDistance = dist;
                index = i;
            }
        }

        return index;
    }

    private int GetSnapIndex(Vector2 position)
    {
        float minDistSq = float.PositiveInfinity;
        int seriesLength = XSeries.Length;
        int index = 0;

        for (int i = 0; i < seriesLength; i++)
        {
            float distSq = position.DistanceSquaredTo(new Vector2(axisX.Map(XSeries[i]), axisY.Map(YSeries[i])));

            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                index = i;
            }
        }

        return index;
    }

    private void PlaceCrosshair(Vector2 position)
    {
        Vector2 plotPos = position - origin; // translate position relative to window back to plot origin

        // Graph has data
        if (XSeries.Length > 0)
        {
            int i;
            float yCrosshairPosX;
            float xCrosshairPosY;

            // Graph has more than one point, so get the point from the cursor x position and snap
            if (XSeries.Length > 1)
            {
                // Only x-snap, so get the closest point along x
                if (XSnap && !YSnap) i = GetSnapIndex(XSeries, axisX.Unmap(plotPos.x));
                // Only y-snap, so get the closest point along y
                else if (!XSnap && YSnap) i = GetSnapIndex(YSeries, axisY.Unmap(plotPos.y));
                // x and y snap or no snap, so get the closest point along either direction
                else i = GetSnapIndex(plotPos);

                //i = Math.Min(
                //    Math.Max(Convert.ToInt32((XSeries.Length - 1) * plotPos.x / xAxisLen), 0),
                //    XSeries.Length - 1);
                //yCrosshairPosX = XSnap ? i * xAxisLen / (XSeries.Length - 1) : plotPos.x;
                //xCrosshairPosY = YSnap ? -i * yAxisLen / (YSeries.Length - 1) : cursorPosY;
                //xCrosshairPosY = YSnap ? axisY.Map(YSeries[i]) - origin.y : plotPos.y;
            }
            // Graph has only one point in the middle of itself
            else
            {
                i = 0;
                //yCrosshairPosX = XSnap ? xAxisLen / 2 : plotPos.x;
                //xCrosshairPosY = YSnap ? -yAxisLen / 2 : plotPos.y;
            }

            yCrosshairPosX = XSnap ? axisX.Map(XSeries[i]) : plotPos.x;
            xCrosshairPosY = YSnap ? axisY.Map(YSeries[i]) : plotPos.y;

            // Set crosshair info text to info about the point, set the crosshair position and active point
            crosshairInfo.Text = String.Format("{0,8}: {1,6}\n{2,8}: {3,6}",
                    //xAxisLabel, i >= 0 && i < XSeries.Length ? String.Format("{0:F2}", XSeries[i]) : "N/A",
                    xAxisLabel, String.Format("{0:F2}", XSeries[i]),
                    //yAxisLabel, i >= 0 && i < YSeries.Length ? String.Format("{0:F2}", YSeries[i]) : "N/A");
                    yAxisLabel, String.Format("{0:F2}", YSeries[i]));
            xCrosshair.Position = new Vector2(xCrosshair.Position.x, xCrosshairPosY);
            yCrosshair.Position = new Vector2(yCrosshairPosX, yCrosshair.Position.y);
            SetActivePoint(i);
        }
        // Graph has no data
        else
        {
            // Set crosshair info text to info about the cursor, and set the crosshair position
            xCrosshair.Position = new Vector2(xCrosshair.Position.x, plotPos.y);
            yCrosshair.Position = new Vector2(plotPos.x, yCrosshair.Position.y);
            crosshairInfo.Text = String.Format("Cursor: {0}\nPoint:  ({1:F2}, {2:F2})",
                    plotPos, axisX.Unmap(plotPos.x), axisY.Unmap(plotPos.y));
        }

        crosshairInfo.Text = String.Format("{0}\n-DEBUG-\nWindow Pos: {1}\nPlot Pos:   ({2}, {3})",
                crosshairInfo.Text, position, plotPos.x, plotPos.y);

        // Crosshair info position is set now because it depends on the text that was set above
        //Vector2 chiPosition = position;
        Vector2 chiPosition = new Vector2(yCrosshair.Position.x + origin.x, xCrosshair.Position.y + origin.y);

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
        else if (@event is InputEventMouseButton mouseClick2 && mouseClick2.ButtonIndex == (int)ButtonList.Right && mouseClick2.Pressed)
        {
            //XSnap = !XSnap;
            //YSnap = !YSnap;
            //GD.Print("Toggle snap ", XSnap, YSnap);
            XSeries = new float[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };
            YSeries = new float[] { 25, 16, 9, 4, 1, 0, 1, 4, 9, 16, 25 };
            Draw();
        }
    }

    private void HandleMouseExited()
    {
        ToggleCrosshair(false);
    }
}
