using Godot;
using System;
using System.Runtime.CompilerServices;
using static Com.NarvinSingh.Physics.Kinematics;
using static Com.NarvinSingh.Utility.Adjustment;

public class PlayerTopDown : KinematicBody2D
{
    private const float DEFAULT_SPEED = 400.0F;
    private const float DEFAULT_ACCEL = 2 * DEFAULT_SPEED;
    private const float DEFAULT_FRIC = 4 * DEFAULT_SPEED;
    private const float ROTATION_OFFSET = (float)(Math.PI / 2.0);

    //private delegate float AccelerateFn(float s0, float input, float dt);
    private delegate Vector2 AccelerateFn(Vector2 v0, Vector2 input, float dt);

    public enum AccelMode { IntExtDrag, ExtDrag, NoDrag }

    private AccelerateFn Accelerate;
    private readonly AccelerateFn[] accelerateFns;
    private AccelMode accelerationMode;
    private float speed;
    private float acceleration;
    private float friction;
    private float drag;
    private float internalDrag;
    private Label info;
    private Camera2D camera;
    private Sprite sprite;
    private Vector2 origPos;
    private Vector2 v0;

    public PlayerTopDown()
    {
        accelerateFns = new AccelerateFn[] { AccelerateIntExtDrag, AccelerateExtDrag, AccelerateNoDrag };
    }

    [Export]
    public AccelMode AccelerationMode
    {
        get
        {
            return accelerationMode;
        }
        set
        {
            accelerationMode = value;
            Accelerate = accelerateFns[(int)value];
        }
    }

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
        get { return acceleration; }
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException("Acceleration", "Acceleration must be greater than zero.");
            }

            acceleration = value;
            UpdateInternalDrag();
        }
    }

    [Export]
    public float Friction
    {
        get { return friction; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("Friction", "Friction must be greater than or equal to zero.");
            }

            friction = value;
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

        Accelerate = accelerateFns[(int)AccelerationMode];
        if (Speed <= 0) Speed = DEFAULT_SPEED;
        if (Acceleration <= 0) Acceleration = DEFAULT_ACCEL;
        if (Friction < 0) Friction = DEFAULT_FRIC;

        Friction = 400;
        Friction = 0;
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
            sprite.Rotation = 0;
            Position = origPos;
            v0 = Vector2.Zero;
            camera.Align();
            UpdateInfo();
        }
    }

    public override void _PhysicsProcess(float dt)
    {
        Vector2 input = GetInputVector();

        if (v0 == Vector2.Zero && input == Vector2.Zero) return; // Stopped and no input, so bail

        v0 = Accelerate(v0, input, dt);
        if (input != Vector2.Zero) sprite.Rotation = v0.Angle() + ROTATION_OFFSET;
        MoveAndCollide(v0 * dt);
        UpdateInfo();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateInternalDrag()
    {
        // Top speed is modeled as terminal velocity (vt) under an internal drag force: 0 = a - d * vt^2
        internalDrag = Acceleration / (Speed * Speed);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2 GetInputVector()
    {
        return new Vector2(
            Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
            Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
        ).Clamped(1);
    }

    private Vector2 AdjustInputVector(Vector2 input, float dt)
    {
        if (Friction != 0) return input;

        if (v0.x != 0 && input.x == 0 && Absolute(input.y) == 1)
        {
            int sign = Sign(v0.x);
            float t = (float)TimeToStop(
                    v0.x, -sign * 0.2 * Acceleration, AccelerationMode != AccelMode.NoDrag ? Drag : 0);

            if (t >= dt) return new Vector2(sign * 0.2F, 0.8F * v0.y);

            v0.x = 0;
            v0.y = (float)Velocity(t, v0.y, 0.8F * input.y, Drag);
            // dt = dt - t
            return input; // 
        }

        if (v0.y != 0 && input.y == 0 && Absolute(input.x) == 1)
        {
        }

        return new Vector2();
    }

    // Calculate speed with drag along one dimension
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float Accelerate1D(float s0, float input, float intDrag, float extDrag, float dt)
    {
        if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        if (intDrag == 0 && extDrag == 0) return Accelerate1D(s0, input, dt); // No drag, so return simple case
        
        float totDrag = intDrag + extDrag;

        // Accelerating
        // When |i| = 1, the maximum top speed s_t is given by:
        //     s_t = sqrt(a / d)
        // When |i| < 1, the maximum top speed should be |i| * s_t and is given by:
        //     |i| * s_t = sqrt(a_eff / d)
        // solving for a_eff:
        //     a = s_t^2 * d
        //     a_eff = i^2 * s_t^2 * d
        //           = i^2 * a
        if (Phase(input, s0) == 1) return (float)Velocity(dt, s0, Sign(input) * input * input * Acceleration, totDrag);

        // Decelerating
        float friction;

        // Due to friction (and drag)
        if (Friction != 0) friction = -Sign(s0) * Friction;
        // Due to acceleration in the opposite direction (and drag)
        else friction = input * Acceleration;

        float timeToStop = (float)TimeToStop(s0, friction, extDrag);

        // Not enough time to stop, so apply friction and drag for dt
        if (timeToStop > dt) return (float)Velocity(dt, s0, friction, extDrag);

        // Enough time to stop, so apply acceleration and drag for remaining time
        return (float)Velocity(dt - timeToStop, 0, Sign(input) * input * input * Acceleration, totDrag);
    }

    // Calculate speed with no drag along one dimension
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private float Accelerate1D(float s0, float input, float dt)
    {
        if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        // Coasting
        if (Friction == 0 && input == 0) return s0;

        // Accelerating
        if (Phase(input, s0) == 1) return (float)Velocity(dt, s0, input * Acceleration);

        // Decelerating
        float friction;

        // Due to friction
        if (Friction != 0) friction = -Sign(s0) * Friction;
        // Due to acceleration in the opposite direction
        else friction = input * Acceleration;

        float timeToStop = (float)TimeToStop(s0, friction);

        // Not enough time to stop, so apply friction and drag for dt
        if (timeToStop > dt) return (float)Velocity(dt, s0, friction);

        // Enough time to stop, so apply acceleration and drag for remaining time
        return (float)Velocity(dt - timeToStop, 0, input * Acceleration);
    }

    // Calculate velocity using both external and internal drag to model top speed
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2 AccelerateIntExtDrag(Vector2 v0, Vector2 input, float dt)
    {
        return new Vector2(Accelerate1D(v0.x, input.x, internalDrag, Drag, dt),
                Accelerate1D(v0.y, input.y, internalDrag, Drag, dt));
    }

    // Calculate velocity using only external drag to model top speed
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2 AccelerateExtDrag(Vector2 v0, Vector2 input, float dt)
    {
        return new Vector2(Accelerate1D(v0.x, input.x, 0, Drag, dt), Accelerate1D(v0.y, input.y, 0, Drag, dt));
    }

    // Calculate velocity with no drag and clamped to top speed
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Vector2 AccelerateNoDrag(Vector2 v0, Vector2 input, float dt)
    {
        return new Vector2(Accelerate1D(v0.x, input.x, dt), Accelerate1D(v0.y, input.y, dt)).Clamped(Speed);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void UpdateInfo()
    {
        info.Text = String.Format("    mode: {0}\n" +
                "position: {1,8:F2}, {2,8:F2}\n" +
                "velocity: {3,8:F2}, {4,8:F2}\n" +
                "   speed: {5,8:F2}\n" +
                "a={6}, f={7}, d={8}+{9}",
                AccelerationMode, Position.x, Position.y, v0.x, v0.y, v0.Length(),
                Acceleration, Friction, Drag, internalDrag);
    }
}
