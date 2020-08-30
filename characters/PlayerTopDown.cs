using Godot;
using System;
using static Com.NarvinSingh.Physics.Kinematics;
using static Com.NarvinSingh.Utility.Adjustment;

public class PlayerTopDown : KinematicBody2D
{
    private const float DEFAULT_SPEED = 400.0F;
    private const float DEFAULT_ACCEL = 2 * DEFAULT_SPEED;
    private const float DEFAULT_FRIC = 4 * DEFAULT_SPEED;
    private const float ROTATION_OFFSET = (float)(Math.PI / 2.0);

    private delegate float AccelerateFn(float s0, float input, float dt);

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

        v0 = new Vector2(Accelerate(v0.x, input.x, dt), Accelerate(v0.y, input.y, dt)).Clamped(Speed);
        if (input != Vector2.Zero) sprite.Rotation = v0.Angle() + ROTATION_OFFSET;
        MoveAndCollide(v0 * dt);
        UpdateInfo();
    }

    private void UpdateInternalDrag()
    {
        // Top speed is modeled as terminal velocity (vt) under an internal drag force: 0 = a - d * vt^2
        internalDrag = Acceleration / (Speed * Speed);
    }

    private Vector2 GetInputVector()
    {
        return new Vector2(
            Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
            Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
        ).Clamped(1);
    }

    private float GetEffectiveFriction(float input)
    {
        // if Friction -> Friction * s0Hat (regardless of input--no input will slow bc of friction)
        // if !Friction -> input * Acceleration (no input will coast)

        if (Friction != 0) return input > 0 ? Friction : -Friction;
        // No friction, so player has to use its acceleration to stop, so input should be opposing velocity
        return input * Acceleration;
    }

    private float RealAccelerate(float s0, float input, float totalDrag, float externalDrag, float sMax, float dt)
    {
        if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        // Accelerating
        if ((input > 0 && s0 >= 0) || (input < 0 && s0 <= 0))
        {
            //return Clamp((float)Velocity(dt, s0, Acceleration * input, totalDrag), input * sMax);
            return (float)Velocity(dt, s0, Sign(input) * input * input * Acceleration, totalDrag);
        }

        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(s0) * Friction;
            else friction = input * Acceleration;
            
            float timeToStop = (float)TimeToStop(s0, friction, externalDrag);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) return (float)Velocity(dt, s0, friction, externalDrag);

            // Enough time to stop, so apply acceleration and drag for remaining time
            //return Clamp((float)Velocity(dt - timeToStop, 0, Acceleration * input, totalDrag), input * sMax);
            return (float)Velocity(dt - timeToStop, 0, Sign(input) * input * input * Acceleration, totalDrag);
        }

        // Coasting (no friction)
        return s0;
    }

    // Calculate speed using both external and internal drag to model top speed
    private float AccelerateIntExtDrag(float s0, float input, float dt)
    {
        return RealAccelerate(s0, input, Drag + internalDrag, Drag, (float)Math.Sqrt(Acceleration / (Drag + internalDrag)), dt);
        //if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        //// Accelerating
        //if ((input > 0 && s0 >= 0) || (input < 0 && s0 <= 0))
        //{
        //    return (float)Velocity(dt, s0, Acceleration * input, Drag + internalDrag);
        //}

        //// Decelerating (either due to friction or accelerating in the other direction)
        //else if (Friction != 0 || input != 0)
        //{
        //    float friction = GetEffectiveFriction(input);
        //    float timeToStop = (float)TimeToStop(s0, friction, Drag);

        //    // Not enough time to stop, so apply friction and drag for dt
        //    if (timeToStop > dt) return (float)Velocity(dt, s0, friction, Drag);

        //    // Enough time to stop, so apply acceleration and drag for remaining time
        //    return (float)Velocity(dt - timeToStop, 0, Acceleration * input, Drag + internalDrag);
        //}

        //// Coasting (no friction)
        //return s0;
    }

    // Calculate speed using only external drag to model top speed
    private float AccelerateExtDrag(float s0, float input, float dt)
    {
        return RealAccelerate(s0, input, Drag, Drag, (float)Math.Sqrt(Acceleration / Drag), dt);
        //if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        //// Accelerating
        //if ((input > 0 && s0 >= 0) || (input < 0 && s0 <= 0))
        //{
        //    return (float)Velocity(dt, s0, Acceleration * input, Drag);
        //}

        //// Decelerating (either due to friction or accelerating in the other direction)
        //else if (Friction != 0 || input != 0)
        //{
        //    float friction = GetEffectiveFriction(input);
        //    float timeToStop = (float)TimeToStop(s0, friction, Drag);

        //    // Not enough time to stop, so apply friction and drag for dt
        //    if (timeToStop > dt) return (float)Velocity(dt, s0, friction, Drag);

        //    // Enough time to stop, so apply acceleration and drag for remaining time
        //    return (float)Velocity(dt - timeToStop, 0, Acceleration * input, Drag);
        //}

        //// Coasting (no friction)
        //return s0;
    }

    // Calculate speed with no drag and clamped to top speed
    private float AccelerateNoDrag(float s0, float input, float dt)
    {
        return RealAccelerate(s0, input, 0, 0, Speed, dt);
        //if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail
        //if ((input > 0 && s0 >= 0) || (input < 0 && s0 <= 0)) return Clamp(s0 + input * Acceleration * dt, Speed);
        //if (s0 > 0) return Math.Max(0, s0 + GetEffectiveFriction(input) * dt);
        //return Math.Min(0, s0 - GetEffectiveFriction(input) * dt);
    }

    private void UpdateInfo()
    {
        info.Text = String.Format("position: {0,8:F2}, {1,8:F2}\nvelocity: {2,8:F2}, {3,8:F2}\n   speed: {4,8:F2}\n" +
                "a={5}, f={6}, d={7}+{8}\nmode: {9}",
                Position.x, Position.y, v0.x, v0.y, v0.Length(), Acceleration, Friction, Drag, internalDrag,
                AccelerationMode);
    }
}
