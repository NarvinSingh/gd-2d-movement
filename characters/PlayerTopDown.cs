using Godot;
using System;
using static Com.NarvinSingh.Physics.Kinematics;

public class PlayerTopDown : KinematicBody2D
{
    private const float DEFAULT_SPEED = 400.0F;
    private const float DEFAULT_ACCEL = 2 * DEFAULT_SPEED;
    private const float DEFAULT_FRIC = 4 * DEFAULT_SPEED;
    private const float ROTATION_OFFSET = (float)(Math.PI / 2.0);

    private float speed;
    private float accel;
    private float fric;
    private float drag;
    private float effFric;
    private float intDrag;
    private Label info;
    private Camera2D camera;
    private Sprite sprite;
    private Vector2 origPos;
    private Vector2 v0;

    public enum AccelMode { IntExtDrag, ExtDrag, NoDrag }

    [Export] public AccelMode AccelerationMode { get; set; } = AccelMode.IntExtDrag;

    [Export]
    public float Speed
    {
        get { return speed; }
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException("Speed", "Speed must be greater than zero.");

            speed = value;
            UpdateInternalDrag();
        }
    }

    [Export]
    public float Acceleration
    {
        get { return accel; }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException("Acceleration", "Acceleration must be greater than zero.");
            }

            accel = value;
            UpdateEffectiveFriction();
            UpdateInternalDrag();
        }
    }

    [Export]
    public float Friction
    {
        get { return fric; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("Friction", "Friction must be greater than or equal to zero.");
            }

            fric = value;
            UpdateEffectiveFriction();
        }
    }

    [Export]
    public float Drag
    {
        get { return drag; }
        set
        {
            if (value < 0) throw new ArgumentOutOfRangeException("Drag", "Drag must be greater than or equal to zero.");
            drag = value;
        }
    }

    public override void _Ready()
    {
        info = (Label)GetNode("Info");
        camera = (Camera2D)GetNode("Camera");
        sprite = (Sprite)GetNode("Sprite");
        origPos = Position;
        v0 = Vector2.Zero;

        if (Speed <= 0) Speed = DEFAULT_SPEED;
        if (Acceleration <= 0) Acceleration = DEFAULT_ACCEL;
        if (Friction < 0) Friction = DEFAULT_FRIC;

        Friction = 400;
        Drag = 0.01F;

        UpdateInfo();
    }

    public override void _Input(InputEvent @event)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            AccelerationMode = (AccelMode)((int)(AccelerationMode + 1) % 3);
            UpdateInfo();
        }

        if (Input.IsActionJustPressed("ui_cancel"))
        {
            Position = origPos;
            camera.Align();
            v0 = Vector2.Zero;
            UpdateInfo();
        }
    }

    public override void _PhysicsProcess(float dt)
    {
        Vector2 input = GetInputVector();

        if (v0 == Vector2.Zero && input == Vector2.Zero) return; // Stopped and no input, so bail

        switch (AccelerationMode)
        {
            case AccelMode.NoDrag:
                v0 = AccelerateNoDrag(v0, input, dt);
                break;

            case AccelMode.ExtDrag:
                v0 = new Vector2(AccelerateExtDrag(v0.x, input.x, dt),
                        AccelerateExtDrag(v0.y, input.y, dt)).Clamped(Speed);
                break;

            default:
                v0 = new Vector2(AccelerateIntExtDrag(v0.x, input.x, dt),
                        AccelerateIntExtDrag(v0.y, input.y, dt)).Clamped(Speed);
                break;
        }

        if (v0 != Vector2.Zero) sprite.Rotation = v0.Angle() + ROTATION_OFFSET;
        MoveAndCollide(v0 * dt);
        UpdateInfo();
    }

    // Calculate speed using both external and internal drag to model top speed
    private float AccelerateIntExtDrag(float s0, float input, float dt)
    {
        // Accelerating
        if (input > 0 && s0 >= 0 || input < 0 && s0 <= 0)
        {
            return (float)Velocity(dt, s0, Acceleration * input, Drag + intDrag);
        }

        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input != 0)
        {
            float friction = s0 >= 0 ? -effFric : effFric;
            float timeToStop = (float)TimeToStop(s0, friction, Drag);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) return (float)Velocity(dt, s0, friction, Drag);

            // Enough time to stop, so apply acceleration and drag for remaining time
            return (float)Velocity(dt - timeToStop, 0, Acceleration * input, Drag + intDrag);
        }

        // Coasting (no friction)
        return s0;
    }

    // Calculate speed using only external drag to model top speed
    private float AccelerateExtDrag(float s0, float input, float dt)
    {
        // Accelerating
        if (input > 0 && s0 >= 0 || input < 0 && s0 <= 0) return (float)Velocity(dt, s0, Acceleration * input, Drag);

        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input != 0)
        {
            float friction = s0 >= 0 ? -effFric : effFric;
            float timeToStop = (float)TimeToStop(s0, friction, Drag);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) return (float)Velocity(dt, s0, friction, Drag);

            // Enough time to stop, so apply acceleration and drag for remaining time
            return (float)Velocity(dt - timeToStop, 0, Acceleration * input, Drag);
        }

        // Coasting (no friction)
        return s0;
    }

    // Calculate velocity with no drag and clamped to top speed
    private Vector2 AccelerateNoDrag(Vector2 v0, Vector2 input, float dt)
    {
        if (input != Vector2.Zero) return (v0 + input * Acceleration * dt).Clamped(Speed);
        return v0.MoveToward(Vector2.Zero, Friction * dt);
    }

    private void UpdateEffectiveFriction()
    {
        effFric = Friction != 0 ? Friction : Acceleration;
    }

    private void UpdateInternalDrag()
    {
        // Top speed is modeled as terminal velocity (vt) under an internal drag force: 0 = a - d * vt^2
        intDrag = Acceleration / (Speed * Speed);
    }

    private Vector2 GetInputVector()
    {
        return new Vector2(
            Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
            Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
        ).Clamped(1);
    }

    private void UpdateInfo()
    {
        info.Text = String.Format("position: {0,8:F2}, {1,8:F2}\nvelocity: {2,8:F2}, {3,8:F2}\n   speed: {4,8:F2}\n" +
                "a={5}, f={6}, d={7}+{8}\nmode: {9}",
                Position.x, Position.y, v0.x, v0.y, v0.Length(), Acceleration, Friction, Drag, intDrag,
                AccelerationMode);
    }
}
