using Com.NarvinSingh.Graphing;
using Godot;
using System;
using System.Collections.Generic;

public class Graph : Node2D
{
    private const string DEFAULT_POINT_HIGHLIGHT_COLOR = "6680FF";
    private const float POINT_SCALE = 1.2F;
    private const float CHI_OFFSET = 6;

    private static readonly Comparer<Vector2> seriesComparer = Comparer<Vector2>.Create((a, b) => a.x.CompareTo(b.x));

    private string title;
    private string xAxisLabel;
    private string yAxisLabel;
    private Vector2[] series;
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
        get { return title; }
        set
        {
            title = value;
            if (!(titleLabel is null)) titleLabel.Text = title;
        }
    }

    [Export]
    public string XAxisLabel
    {
        get { return xAxisLabel; }
        set
        {
            xAxisLabel = value;
            if (!(xLabel is null)) xLabel.Text = xAxisLabel;
        }
    }

    [Export]
    public string YAxisLabel
    {
        get { return yAxisLabel; }
        set
        {
            yAxisLabel = value;
            if (!(yLabel is null)) yLabel.Text = yAxisLabel;
        }
    }

    [Export]
    public Vector2[] Series
    {
        get { return series; }
        set
        {
            series = (Vector2[])value.Clone();
            Array.Sort(Series, seriesComparer);
        }
    }

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
        if (!isReady || Series is null) return;

        // Destroy the current points
        if (!(points is null))
        {
            points.ForEach((point) => point.QueueFree());
            points.Clear();
        }

        // Graph has data
        if (Series.Length > 0)
        {
            int lastIndex = Series.Length - 1;

            axisX = new Axis(0, xAxisLen, Series[0].x, Series[lastIndex].x);
            axisY = new Axis(0, yAxisLen, Series[0].y, Series[lastIndex].y, true);
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

        int numPoints = Series.Length;

        for (int i = 0; i < numPoints; i++) points.Add(CreatePoint(axisX.Map(Series[i].x), axisY.Map(Series[i].y)));
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

    // x values are sorted so we can use a binary search
    private int GetXSnapIndex(float xPosition)
    {
        float value = axisX.Unmap(xPosition);

        int index = Array.BinarySearch(Series, new Vector2(value, 0), seriesComparer);

        if (index >= 0) return index; // Exact value found

        index = ~index; // Index of first item that is greater than value

        if (index >= Series.Length) return Series.Length - 1; // Value is greater than all items, so return last item

        // Value is between two items so pick the closer item
        return value - Series[index - 1].x <= Series[index].x - value ? index - 1 : index;
    }

    // y values are unsorted so we have to run through all of them
    private int GetYSnapIndex(float yPosition)
    {
        float value = axisY.Unmap(yPosition);
        float minDistance = float.PositiveInfinity;
        int seriesLength = Series.Length;
        int index = 0;

        for (int i = 0; i < seriesLength; i++)
        {
            float dist = Series[i].y - value;

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
        int xIndex = GetXSnapIndex(position.x); // Closest x
        int index = xIndex;
        // Distance^2 to point at closest x
        float radiusSq = position.DistanceSquaredTo(
                new Vector2(axisX.Map(Series[xIndex].x), axisY.Map(Series[xIndex].y)));
        float radius = (float)Math.Sqrt(radiusSq);
        int xRadius = (int)(radius * axisX.InverseUnitLength); // Neighboring x's where closer point could be
        int xIndexMax = Math.Min(xIndex + xRadius, Series.Length - 1);
        int xIndexMin = Math.Max(0, xIndex - xRadius);

        // Search the neighboring x's to the left for a closer point
        for (int i = xIndex - 1; i >= xIndexMin; i--)
        {
            float nextRadSq = position.DistanceSquaredTo(new Vector2(axisX.Map(Series[i].x), axisY.Map(Series[i].y)));

            // Closer point found so record it and contract the radius of neighboring x's
            if (nextRadSq < radiusSq)
            {
                radiusSq = nextRadSq;

                float nextXRad = (int)(Math.Sqrt(radiusSq) * axisX.InverseUnitLength);

                if (nextXRad < xRadius)
                {
                    xIndexMax = Math.Min(xIndex + xRadius, Series.Length - 1);
                    xIndexMin = Math.Max(0, xIndex - xRadius);
                }

                index = i;
            }
        }

        // Search the neighboring x's to the right for a closer point
        for (int i = xIndex + 1; i <= xIndexMax; i++)
        {
            float nextRadSq = position.DistanceSquaredTo(new Vector2(axisX.Map(Series[i].x), axisY.Map(Series[i].y)));

            // Closer point found so record it and contract the radius of neighboring x's
            if (nextRadSq < radiusSq)
            {
                radiusSq = nextRadSq;

                float nextXRad = (int)(Math.Sqrt(radiusSq) * axisX.InverseUnitLength);

                if (nextXRad < xRadius) xIndexMax = Math.Min(xIndex + xRadius, Series.Length - 1);

                index = i;
            }
        }

        return index;
    }

    private void PlaceCrosshair(Vector2 position)
    {
        Vector2 plotPos = position - origin; // translate position relative to window back to plot origin

        // Graph has data
        if (Series.Length > 0)
        {
            int i;

            // Graph has more than one point, so get the point from the cursor x position and snap
            if (Series.Length > 1)
            {
                if (XSnap && !YSnap) i = GetXSnapIndex(axisX.Unmap(plotPos.x));
                else if (!XSnap && YSnap) i = GetYSnapIndex(axisY.Unmap(plotPos.y));
                else i = GetSnapIndex(plotPos);
            }
            // Graph has only one point in the middle of itself
            else i = 0;

            // Set crosshair info text to info about the point, set the crosshair position and active point
            crosshairInfo.Text = String.Format("{0,8}: {1,6}\n{2,8}: {3,6}",
                    xAxisLabel, String.Format("{0:F2}", Series[i].x), yAxisLabel, String.Format("{0:F2}", Series[i].y));
            xCrosshair.Position = new Vector2(xCrosshair.Position.x, YSnap ? axisY.Map(Series[i].y) : plotPos.y);
            yCrosshair.Position = new Vector2(XSnap ? axisX.Map(Series[i].x) : plotPos.x, yCrosshair.Position.y);
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
            float[] xSeries = new float[] { -5, -4, -3, -2, -1, 0, 1, 2, 3, 4, 5 };
            float[] ySeries = new float[] { 25, 16, 9, 4, 1, 0, 1, 4, 9, 16, 25 };
            Vector2[] newSeries = new Vector2[xSeries.Length];
            for (int i = 0; i < xSeries.Length; i++) newSeries[i] = new Vector2(xSeries[i], ySeries[i]);
            Draw();
        }
    }

    private void HandleMouseExited()
    {
        ToggleCrosshair(false);
    }
}
