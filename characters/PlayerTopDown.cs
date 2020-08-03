using Godot;

public class PlayerTopDown : KinematicBody2D
{
    public float speed = 200;
    private Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        GD.Print("PlayerTopDown ready");
    }

    public override void _PhysicsProcess(float dt)
    {
        velocity = speed * GetInputDirection();
        MoveAndCollide(velocity * dt);
    }

    protected Vector2 GetInputDirection()
    {
        return new Vector2(
            Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
            Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
        ).Clamped(1);
    }
}
