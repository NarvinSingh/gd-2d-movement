using Com.NarvinSingh.Test;
using Godot;
using System;
using static Com.NarvinSingh.Physics.Kinematics;
using static Com.NarvinSingh.Test.Approximate;

public class PlayerTopDownTest : Node
{
    private const bool VERBOSE = true;
    private const float TOLERANCE = 0.001F;
    private const float FPS = 60;
    private const float DT = 1.0F / FPS;

    private static PlayerTopDown player;
    private static Vector2 origPos;

    private static Vector2 V0
    {
        get { return (Vector2)PrivateAccess.Get("v0", player); }
        set { PrivateAccess.Set("v0", player, value); }
    }

    private static float InternalDrag { get { return (float)PrivateAccess.Get("internalDrag", player); } }

    public override void _Ready()
    {
        player = (PlayerTopDown)((PackedScene)ResourceLoader.Load("res://characters/PlayerTopDown.tscn")).Instance();
        origPos = player.Position;
        AddChild(player);
        player.SetPhysicsProcess(false);

        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithIntExtDrag", ItAcceleratesWithIntExtDrag()));
        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithExtDrag", ItAcceleratesWithExtDrag()));
        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithNoDrag", ItAcceleratesWithNoDrag()));

        player.SetPhysicsProcess(true);
    }

    private string Summarize(string description, bool isPass)
    {
        if (isPass) return String.Format("Pass {0}", description);
        return String.Format("FAIL {0}", description);
    }

    private static void ResetPlayer(PlayerTopDown.AccelMode accelMode)
    {
        player.Position = origPos;
        player.AccelerationMode = accelMode;
        player.Speed = 400;
        player.Acceleration = 800;
        player.Friction = 1600;
        player.Drag = 0.01F;
        V0 = Vector2.Zero;
    }

    private static void ApplyInput(string[] actions, float[] strengths, float duration = DT)
    {
        float frames = FPS * duration;
        int fullFrames = (int)frames;
        float lastFrameDuration = duration - fullFrames * DT;

        for (int i = 0; i < actions.Length; i++) Input.ActionPress(actions[i], strengths[i]);
        for (int i = 0; i < fullFrames; i++) player._PhysicsProcess(DT);
        if (lastFrameDuration > 0) player._PhysicsProcess(lastFrameDuration);
        for (int i = 0; i < actions.Length; i++) Input.ActionRelease(actions[i]);
    }

    private static void ApplyInput(string action, float strength = 1, float duration = DT)
    {
        ApplyInput(new string[] { action }, new float[] { strength }, duration);
    }

    private static bool ItAcceleratesWithIntExtDrag()
    {
        Vector2 expected;
        float t;

        ResetPlayer(PlayerTopDown.AccelMode.IntExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag), 0);
        ApplyInput("ui_right", 1, 5);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        t = (float)TimeToStop(V0.x, -player.Friction, player.Drag);
        ApplyInput(null, 0, t);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, 0));
        if (!IsEqual(V0.x, 0, TOLERANCE)) return false;

        ResetPlayer(PlayerTopDown.AccelMode.IntExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag), 0);
        t = (float)TimeToStop(expected.x, -player.Friction, player.Drag);
        expected = new Vector2((float)Velocity(1 - t, 0, -player.Acceleration, player.Drag + InternalDrag), 0);
        ApplyInput("ui_right", 1, 5);
        ApplyInput("ui_left", 1, 1);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        return true;
    }

    private static bool ItAcceleratesWithExtDrag()
    {
        Vector2 expected;
        float t;

        ResetPlayer(PlayerTopDown.AccelMode.ExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag), 0);
        ApplyInput("ui_right", 1, 5);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        t = (float)TimeToStop(V0.x, -player.Friction, player.Drag);
        ApplyInput(null, 0, t);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, 0));
        if (!IsEqual(V0.x, 0, TOLERANCE)) return false;

        ResetPlayer(PlayerTopDown.AccelMode.ExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag), 0);
        t = (float)TimeToStop(expected.x, -player.Friction, player.Drag);
        expected = new Vector2((float)Velocity(1 - t, 0, -player.Acceleration, player.Drag), 0);
        ApplyInput("ui_right", 1, 5);
        ApplyInput("ui_left", 1, 1);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        return true;
    }

    private static bool ItAcceleratesWithNoDrag()
    {
        Vector2 expected;
        float t;

        ResetPlayer(PlayerTopDown.AccelMode.NoDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration), 0).Clamped(player.Speed);
        ApplyInput("ui_right", 1, 5);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        t = (float)TimeToStop(V0.x, -player.Friction);
        ApplyInput(null, 0, t);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, 0));
        if (!IsEqual(V0.x, 0, TOLERANCE)) return false;

        ResetPlayer(PlayerTopDown.AccelMode.NoDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration), 0).Clamped(player.Speed);
        t = (float)TimeToStop(expected.x, -player.Friction);
        expected = new Vector2((float)Velocity(0.4 - t, 0, -player.Acceleration), 0).Clamped(player.Speed);
        ApplyInput("ui_right", 1, 5);
        ApplyInput("ui_left", 1, 0.4F);
        if (VERBOSE) GD.Print(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        return true;
    }
}
