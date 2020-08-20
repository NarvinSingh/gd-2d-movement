using Godot;

public class TopDown : Node2D
{
    [Export] public float FrictionCoeff { get; set; } = 800;
    [Export] public float DragCoeff { get; set; } = 0.01F;
}
