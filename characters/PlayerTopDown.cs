using Godot;
using System;
using static Com.NarvinSingh.Physics.Kinematics;

public class PlayerTopDown : KinematicBody2D
{
    private const float DEFAULT_SPEED = 400.0F;
    private const float DEFAULT_ACCEL = 2 * DEFAULT_SPEED;
    private const float DEFAULT_FRIC = 4 * DEFAULT_SPEED;

    private bool isDebug = false;
    private int debugLevel = 0;
    private float speed;
    private float accel;
    private float fric;
    private float drag;
    private float effFric;
    private float intDrag;
    private Label info;
    private Vector2 v0;

    public Vector2 DbgVelocity
    {
        get { return v0; }
        set { v0 = value; }
    }

    public float DbgTotalDrag { get { return Drag/* + intDrag*/; } }

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
        v0 = Vector2.Zero;

        if (Speed <= 0) Speed = DEFAULT_SPEED;
        if (Acceleration <= 0) Acceleration = DEFAULT_ACCEL;
        if (Friction < 0) Friction = DEFAULT_FRIC;
        UpdateInfo();
    }

    public override void _PhysicsProcess(float dt)
    {
        // X/Y style
        Vector2 input = GetInputVector();

        if (v0 == Vector2.Zero && input == Vector2.Zero) return; // Stopped and no input, so bail

        // DEBUG
         if (Input.IsActionJustPressed("ui_accept"))
        {
            GD.Print("Breakpoint 1");
            // isDebug = !isDebug;
        }

        if (input.y != 0)
        {
            if (debugLevel == 0) 
            {
                // GD.Print("Breakpoint 2");
                debugLevel = 1;
            }
        }

        if (input.y == 0)
        {
            if (debugLevel == 1) 
            {
                // GD.Print("Breakpoint 3");
                debugLevel = 2;
            }
        }

        v0 = new Vector2(Accelerate(v0.x, input.x, dt), Accelerate(v0.y, input.y, dt)).Clamped(Speed);

        // // Parallel/Orthogonal style
        // Vector2 input = GetInputVector();
        // Vector2 inputPara;
        // Vector2 inputOrth;
        // Vector2 vHat;
        // Vector2 v;
        // float s0;

        // if (v0 == Vector2.Zero && input == Vector2.Zero) return; // Stopped and no input, so bail

        // // DEBUG
        //  if (Input.IsActionJustPressed("ui_accept"))
        // {
        //     GD.Print("Breakpoint 1");
        //     // isDebug = !isDebug;
        // }

        // if (input.y != 0)
        // {
        //     if (debugLevel == 0) 
        //     {
        //         // GD.Print("Breakpoint 2");
        //         debugLevel = 1;
        //     }
        // }

        // if (input.y == 0)
        // {
        //     if (debugLevel == 1) 
        //     {
        //         // GD.Print("Breakpoint 3");
        //         debugLevel = 2;
        //     }
        // }

        // if (v0 != Vector2.Zero)
        // {
        //     vHat = v0.Normalized();
        //     inputPara = input.Project(v0);
        //     inputOrth = input.Project(v0.Tangent());
        // }
        // else
        // {
        //     vHat = input.Normalized();
        //     inputPara = input;
        //     inputOrth = Vector2.Zero;
        // }

        // s0 = v0.Length();

        // float angleToV0 = inputPara.AngleTo(v0);
        // bool isPara = Math.Round(inputPara.AngleTo(v0)) == 0;

        // // Accelerating
        // if (inputPara != Vector2.Zero && (isPara || v0 == Vector2.Zero))
        // {
        //     // v = (float)Velocity(dt, s0, Acceleration * inputPara.Length(), Drag + intDrag) * vHat;
        //     v = (float)Velocity(dt, s0, Acceleration * inputPara.Length(), Drag) * vHat;
        // }
        // // Decelerating (either due to friction or accelerating in the other direction)
        // else if (Friction != 0 || inputPara != Vector2.Zero)
        // {
        //     float timeToStop = (float)TimeToStop(s0, -effFric, Drag);

        //     // Not enough time to stop, so apply friction and drag for dt
        //     if (timeToStop > dt) v = (float)Velocity(dt, s0, -effFric, Drag) * vHat;
        //     // Enough time to stop, so apply acceleration and drag in reverse dirction for remaining time
        //     // else v = -(float)Velocity(dt - timeToStop, 0, Acceleration * inputPara.Length(), Drag + intDrag) * vHat;
        //     else v = -(float)Velocity(dt - timeToStop, 0, Acceleration * inputPara.Length(), Drag) * vHat;
        // }
        // // Coasting (no friction)
        // else v = v0;

        // // Changing direction
        // if (inputOrth != Vector2.Zero)
        // {
        //     // v += (float)Velocity(dt, 0, Acceleration * inputOrth.Length(), Drag + intDrag) * inputOrth.Normalized();
        //     v += (float)Velocity(dt, 0, Acceleration * inputOrth.Length(), Drag) * inputOrth.Normalized();
        // }

        // v0 = v.Clamped(Speed);
        // Vector2 unclampedV0 = v;
        // v0 = v.Clamped(Speed);
        // if (isDebug) GD.Print(String.Format("{0},{1},{2},{3},{4},{5}", unclampedV0.x, unclampedV0.y, unclampedV0.Length(), v0.x, v0.y, v0.Length()));
        MoveAndCollide(v0 * dt);
        UpdateInfo();
    }

    public void DbgUpdateInfo()
    {
        UpdateInfo();
    }

    private float Accelerate(float v0, float input, float dt)
    {
        // Accelerating
        if (input > 0 && v0 >= 0 || input < 0 && v0 <= 0) return (float)Velocity(dt, v0, Acceleration * input, Drag);

        // Decelerating (either due to friction or accelerating in the other direction)
        else if (Friction != 0 || input != 0)
        {
            float friction = v0 >= 0 ? -effFric : effFric;
            float timeToStop = (float)TimeToStop(v0, friction, Drag);

            // Not enough time to stop, so apply friction and drag for dt
            if (timeToStop > dt) return (float)Velocity(dt, v0, friction, Drag);

            // Enough time to stop, so apply acceleration and drag for remaining time
            return (float)Velocity(dt - timeToStop, 0, Acceleration * input, Drag);
        }

        // Coasting (no friction)
        return v0;
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
                "a={5}, f={6}, d={7}+{8}",
                Position.x, Position.y, v0.x, v0.y, v0.Length(), Acceleration, Friction, Drag, intDrag);
    }
}
