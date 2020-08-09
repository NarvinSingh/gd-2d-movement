using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public class PlayerTopDown : KinematicBody2D
{
    private const float DT = 1.0F / 10000;
    private const float DT_TOLERANCE = DT * 2;

    private Label info;
    private float frictionCoeff;
    private float dragCoeff;
    private Vector2 velocity = Vector2.Zero;

    [Export] public float Mass { get; set; } = 10;
    [Export] public float Speed { get; set; } = 200;
    [Export] public float Acceleration { get; set; } = 400;

    private float InternalDragCoeff
    {
        get
        {
            return Acceleration / Speed / Speed;
        }
    }

    public override void _Ready()
    {
        TopDown level = (TopDown)GetParent();
        info = (Label)GetNode("Info");
        frictionCoeff = level.FrictionCoeff;
        dragCoeff = level.DragCoeff;
        GD.Print("PlayerTopDown ready");

        Vector2 velocityExact = new Vector2(200, 0);

        for (int i = 1; i <= 0; i++)
        {
            ulong t0;
            ulong t;
            const float DT = 1.0F / 60;

            // t0 = OS.GetTicksUsec();
            // velocity = CalcVelocity(DT, velocity, Vector2.Right, Speed, Acceleration, frictionCoeff, dragCoeff);
            // t = OS.GetTicksUsec();
            // GD.Print(String.Format("{0,4} ({1,6:F2}, {2,6:F2}) {3,6:F2} {4,4}us", i, velocity.x, velocity.y, velocity.Length(), t - t0));

            t0 = OS.GetTicksUsec();
            velocityExact = CalcVelocityExact(DT, velocityExact, Vector2.Zero, 200, 400, 800, 400.0F / 200 / 200);
            t = OS.GetTicksUsec();
            // GD.Print(String.Format("{0,4} ({1,6:F2}, {2,6:F2}) {3,6:F2} {4,4}us delta: {5:F2}\n", i, velocityExact.x, velocityExact.y, velocityExact.Length(), t - t0, velocity.Length() - velocityExact.Length()));
            GD.Print(String.Format("{0},{1}", i * DT, velocityExact.x));
        }

        velocity = Vector2.Zero;
        // GD.Print("KinematicsTest.ItAcceleratesNoDrag()", KinematicsTest.ItAcceleratesNoDrag());
        // GD.Print("KinematicsTest.ItDragsNoAcceleration()", KinematicsTest.ItDragsNoAcceleration());
    }

    public override void _PhysicsProcess(float dt)
    {
        velocity = CalcVelocity(dt, velocity);
        MoveAndCollide(velocity * dt);
        info.Text = String.Format("position: {0,8:F2}, {1,8:F2}\nvelocity: {2,8:F2}, {3,8:F2}\n   speed: {4,8:F2}",
                Position.x, Position.y, velocity.x, velocity.y, velocity.Length());
    }

    protected Vector2 GetInputVector()
    {
        return new Vector2(
            Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left"),
            Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up")
        ).Clamped(1);
    }

    protected Vector2 CalcVelocityExact(float dt, Vector2 v0, Vector2 i, float sMax, float a,
            float f, float d)
    {
        Vector2 vHat;
        Vector2 iPara;
        Vector2 iOrth;
        Vector2 v;
        float s0 = v0.Length();

        if (v0 != Vector2.Zero)
        {
            vHat = v0.Normalized();
            iPara = i.Project(v0);
            iOrth = i.Project(v0.Tangent());
        }
        else
        {
            vHat = i.Normalized();
            iPara = i;
            iOrth = Vector2.Zero;
        }

        if (iPara != Vector2.Zero && iPara.AngleTo(v0) == 0) v = CalcSpeed(s0, dt, d, a * iPara.Length()) * vHat;
        else
        {
			float tStop = CalcStopTime(s0, d, f);

			if (tStop > dt) v = CalcSpeed(s0, dt, d, f) * vHat;
			else v = -CalcSpeed(0, dt - tStop, d, a * iPara.Length()) * vHat;
        }

		v += CalcSpeed(0, dt, d, a * iOrth.Length()) * iOrth.Normalized();

        return v;
    }

    protected Vector2 CalcVelocityExact(float dt, Vector2 v0)
    {
        return CalcVelocityExact(dt, v0, GetInputVector(), Speed, Acceleration, frictionCoeff, dragCoeff);
    }

    protected Vector2 CalcVelocity(float dt, Vector2 v0, Vector2 i, float sMax, float a,
            float f, float d)
    {
        if (d != 0 && dt > DT_TOLERANCE)
        {
            return CalcVelocity(dt - DT, CalcVelocity(DT, v0, i, sMax, a, f, d), i, sMax, a, f, d);
        }

        Vector2 vHat;
        Vector2 iPara;
        Vector2 iOrth;
        Vector2 v;
        float s0 = v0.Length();

        if (v0 != Vector2.Zero)
        {
            vHat = v0.Normalized();
            iPara = i.Project(v0);
            iOrth = i.Project(v0.Tangent());
        }
        else
        {
            vHat = i.Normalized();
            iPara = i;
            iOrth = Vector2.Zero;
        }

        if (iPara != Vector2.Zero && iPara.AngleTo(v0) == 0)
        {
            v = v0 + (a * iPara - d * s0 * s0 * vHat) * dt;
        }
        else
        {
            float sMed = s0 / 2;
            float tFriction = s0 / (f + d * sMed * sMed);

            if (tFriction > dt)
            {
                v = v0 - (f * d * s0 * s0) * vHat * dt;
            }
            else
            {
                v = a * iPara * (dt - tFriction);
            }
        }

        v += a * iOrth * dt;

        return v;
    }

    protected Vector2 CalcVelocity(float dt, Vector2 v0)
    {
        return CalcVelocity(dt, v0, GetInputVector(), Speed, Acceleration, frictionCoeff, dragCoeff);
    }

	private float CalcSpeed(float s0, float dt, float d = 0, float a = 0)
	{
		if (d == 0) return s0 + a * dt;

		if (a > 0)
		{
			Complex c1 = new Complex(0, Math.Sqrt(a / d));
			Complex c2 = new Complex(0, Math.Sqrt(d / a));
			Complex c3 = new Complex(0, Math.Sqrt(a * d));
			Complex s = -c1 * Complex.Tan(Complex.Atan(c2 * s0) + c3 * dt);
			return (float)s.Real;
		}
		
		if (a < 0) return Mathf.Sqrt(a / d) * Mathf.Tan(Mathf.Atan(Mathf.Sqrt(d / a) * s0) + Mathf.Sqrt(a * d) * dt);
		
		return s0 / (d * s0 * dt + 1);
	}

	private float CalcStopTime(float s0, float d = 0, float f = 0)
	{
		if (f <= 0) return float.PositiveInfinity;

		if (d == 0) return s0 / f;

		return Mathf.Atan(Mathf.Sqrt(f / d) * s0) / Mathf.Sqrt(f * d);
	}
}
