using Godot;

public class TopDown : Node2D
{
    [Export] public float friction { get; set; } = 800;
    [Export] public float drag { get; set; } = 0.1F;
}
