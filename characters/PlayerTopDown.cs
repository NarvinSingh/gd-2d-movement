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

        //v0 = new Vector2(Accelerate(v0.x, input.x, dt), Accelerate(v0.y, input.y, dt));
        v0 = Accelerate(v0, input, dt);
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

    private float RealAccelerate(float s0, float input, float totalDrag, float externalDrag, float dt)
    {
        if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        // Accelerating
        if (Phase(input, s0) == 1)
        {
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
            return (float)Velocity(dt - timeToStop, 0, Sign(input) * input * input * Acceleration, totalDrag);
        }

        // Coasting (no friction)
        return s0;
    }

    // Calculate speed using both external and internal drag to model top speed
    private Vector2 AccelerateIntExtDrag(Vector2 v0, Vector2 input, float dt)
    {
        Vector2 v = new Vector2();

        // Stopped and no input, so bail
        if (v0.x == 0 && input.x == 0) v.x = 0;
        // Accelerating
        else if (Phase(input.x, v0.x) == 1)
        {
            v.x = (float)Velocity(dt, v0.x, Sign(input.x) * input.x * input.x * Acceleration, internalDrag + Drag);
        }
        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input.x != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(v0.x) * Friction;
            else friction = input.x * Acceleration;

            float timeToStop = (float)TimeToStop(v0.x, friction, Drag);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) v.x = (float)Velocity(dt, v0.x, friction, Drag);
            // Enough time to stop, so apply acceleration and drag for remaining time
            else
            {
                v.x = (float)Velocity(
                        dt - timeToStop, 0, Sign(input.x) * input.x * input.x * Acceleration, internalDrag + Drag);
            }
        }
        // Coasting (no friction)
        else v.x = v0.x;

        // Repeat the above calculations for the y component. We're sacrificing DRY for performance.
        if (v0.y == 0 && input.y == 0) v.y = 0;
        else if (Phase(input.y, v0.y) == 1)
        {
            v.y = (float)Velocity(dt, v0.y, Sign(input.y) * input.y * input.y * Acceleration, internalDrag + Drag);
        }
        else if (Friction != 0 || input.y != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(v0.y) * Friction;
            else friction = input.y * Acceleration;

            float timeToStop = (float)TimeToStop(v0.y, friction, Drag);

            if (timeToStop > dt) v.y = (float)Velocity(dt, v0.y, friction, Drag);
            else
            {
                v.y = (float)Velocity(
                        dt - timeToStop, 0, Sign(input.y) * input.y * input.y * Acceleration, internalDrag + Drag);
            }
        }
        else v.y = v0.y;

        return v;
    }

    // Calculate speed using only external drag to model top speed
    private Vector2 AccelerateExtDrag(Vector2 v0, Vector2 input, float dt)
    {
        Vector2 v = new Vector2();

        // Stopped and no input, so bail
        if (v0.x == 0 && input.x == 0) v.x = 0;
        // Accelerating
        else if (Phase(input.x, v0.x) == 1)
        {
            v.x = (float)Velocity(dt, v0.x, Sign(input.x) * input.x * input.x * Acceleration, Drag);
        }
        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input.x != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(v0.x) * Friction;
            else friction = input.x * Acceleration;

            float timeToStop = (float)TimeToStop(v0.x, friction, Drag);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) v.x = (float)Velocity(dt, v0.x, friction, Drag);
            // Enough time to stop, so apply acceleration and drag for remaining time
            else v.x = (float)Velocity(dt - timeToStop, 0, Sign(input.x) * input.x * input.x * Acceleration, Drag);
        }
        // Coasting (no friction)
        else v.x = v0.x;

        // Repeat the above calculations for the y component. We're sacrificing DRY for performance.
        if (v0.y == 0 && input.y == 0) v.y = 0;
        else if (Phase(input.y, v0.y) == 1)
        {
            v.y = (float)Velocity(dt, v0.y, Sign(input.y) * input.y * input.y * Acceleration, Drag);
        }
        else if (Friction != 0 || input.y != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(v0.y) * Friction;
            else friction = input.y * Acceleration;

            float timeToStop = (float)TimeToStop(v0.y, friction, Drag);

            if (timeToStop > dt) v.y = (float)Velocity(dt, v0.y, friction, Drag);
            else v.y = (float)Velocity(dt - timeToStop, 0, Sign(input.y) * input.y * input.y * Acceleration, Drag);
        }
        else v.y = v0.y;

        return v;
    }

    // Calculate speed with no drag and clamped to top speed
    private Vector2 AccelerateNoDrag(Vector2 v0, Vector2 input, float dt)
    {
        Vector2 v = new Vector2();

        // Stopped and no input, so bail
        if (v0.x == 0 && input.x == 0) v.x = 0;
        // Accelerating
        else if (Phase(input.x, v0.x) == 1) v.x = (float)Velocity(dt, v0.x, input.x * Acceleration);
        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input.x != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(v0.x) * Friction;
            else friction = input.x * Acceleration;

            float timeToStop = (float)TimeToStop(v0.x, friction);

            // Not enough time to stop, so apply friction for dt
            if (timeToStop > dt) v.x = (float)Velocity(dt, v0.x, friction);
            // Enough time to stop, so apply acceleration for remaining time
            else v.x = (float)Velocity(dt - timeToStop, 0, input.x * Acceleration);
        }
        // Coasting (no friction)
        else v.x = v0.x;

        // Repeat the above calculations for the y component. We're sacrificing DRY for performance.
        if (v0.y == 0 && input.y == 0) v.y = 0;
        // Accelerating
        else if (Phase(input.y, v0.y) == 1) v.y = (float)Velocity(dt, v0.y, input.y * Acceleration);
        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input.y != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(v0.y) * Friction;
            else friction = input.y * Acceleration;

            float timeToStop = (float)TimeToStop(v0.y, friction);

            // Not enough time to stop, so apply friction for dt
            if (timeToStop > dt) v.y = (float)Velocity(dt, v0.y, friction);
            // Enough time to stop, so apply acceleration for remaining time
            else v.y = (float)Velocity(dt - timeToStop, 0, input.y * Acceleration);
        }
        // Coasting (no friction)
        else v.y = v0.y;

        return v.Clamped(Speed);
    }

    // Calculate speed using both external and internal drag to model top speed
    private float AccelerateIntExtDrag(float s0, float input, float dt)
    {
        //return RealAccelerate(s0, input, Drag + internalDrag, Drag, dt);

        if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        // Accelerating
        if (Phase(input, s0) == 1)
        {
            return (float)Velocity(dt, s0, Sign(input) * input * input * Acceleration, internalDrag + Drag);
        }

        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(s0) * Friction;
            else friction = input * Acceleration;

            float timeToStop = (float)TimeToStop(s0, friction, Drag);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) return (float)Velocity(dt, s0, friction, Drag);

            // Enough time to stop, so apply acceleration and drag for remaining time
            return (float)Velocity(dt - timeToStop, 0, Sign(input) * input * input * Acceleration, internalDrag + Drag);
        }

        // Coasting (no friction)
        return s0;
    }

    // Calculate speed using only external drag to model top speed
    private float AccelerateExtDrag(float s0, float input, float dt)
    {
        //return RealAccelerate(s0, input, Drag, Drag, dt);

        if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        // Accelerating
        if (Phase(input, s0) == 1)
        {
            return (float)Velocity(dt, s0, Sign(input) * input * input * Acceleration, Drag);
        }

        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(s0) * Friction;
            else friction = input * Acceleration;

            float timeToStop = (float)TimeToStop(s0, friction, Drag);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) return (float)Velocity(dt, s0, friction, Drag);

            // Enough time to stop, so apply acceleration and drag for remaining time
            return (float)Velocity(dt - timeToStop, 0, Sign(input) * input * input * Acceleration, Drag);
        }

        // Coasting (no friction)
        return s0;
    }

    // Calculate speed with no drag and clamped to top speed
    private float AccelerateNoDrag(float s0, float input, float dt)
    {
        //return Clamp(RealAccelerate(s0, input, 0, 0, dt), input * Speed);

        if (s0 == 0 && input == 0) return 0; // Stopped and no input, so bail

        // Accelerating
        if (Phase(input, s0) == 1)
        {
            return Clamp((float)Velocity(dt, s0, Sign(input) * input * input * Acceleration), Speed);
        }

        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input != 0)
        {
            float friction;

            if (Friction != 0) friction = -Sign(s0) * Friction;
            else friction = input * Acceleration;

            float timeToStop = (float)TimeToStop(s0, friction);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) return (float)Velocity(dt, s0, friction);

            // Enough time to stop, so apply acceleration and drag for remaining time
            return (float)Velocity(dt - timeToStop, 0, Sign(input) * input * input * Acceleration);
        }

        // Coasting (no friction)
        return s0;
    }

    private void UpdateInfo()
    {
        info.Text = String.Format("position: {0,8:F2}, {1,8:F2}\nvelocity: {2,8:F2}, {3,8:F2}\n   speed: {4,8:F2}\n" +
                "a={5}, f={6}, d={7}+{8}\nmode: {9}",
                Position.x, Position.y, v0.x, v0.y, v0.Length(), Acceleration, Friction, Drag, internalDrag,
                AccelerationMode);
    }
}
