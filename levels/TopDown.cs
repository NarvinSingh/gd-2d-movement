using Godot;

public class TopDown : Node2D
{
    [Export] public float Friction { get; set; } = 800;
    [Export] public float Drag { get; set; } = 0.1F;
}
