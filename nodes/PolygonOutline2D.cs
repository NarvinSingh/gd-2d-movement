using Godot;

public class PolygonOutline2D : Polygon2D
{
    [Export] public Color StrokeColor { get; set; } = new Color(0, 0, 0);
    [Export] public float StrokeWidth { get; set; } = 1.0F;
    [Export] public bool Closed { get; set; } = true;

    public override void _Draw()
    {
        int numVertices = Polygon.Length;

        for (int i = 1; i < numVertices; i++)
        {
            DrawLine(Polygon[i - 1] + Offset, Polygon[i] + Offset, StrokeColor, StrokeWidth, Antialiased);
        }

        if (Closed) DrawLine(Polygon[numVertices - 1], Polygon[0], StrokeColor, StrokeWidth, Antialiased);
    }
}
