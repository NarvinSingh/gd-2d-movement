#define VERBOSE

using Com.NarvinSingh.Test;
using Godot;
using System;
using static Com.NarvinSingh.Physics.Kinematics;
using static Com.NarvinSingh.Test.Approximate;

public class PlayerTopDownTest : Node
{
    private const float TOLERANCE = 0.001F;
    private const float FPS = 60;
    private const float DT = 1.0F / FPS;

    private static PlayerTopDown player;
    private static Sprite sprite;
    private static Vector2 origPos;

    private static Vector2 V0
    {
        get { return (Vector2)PrivateAccess.Get("v0", player); }
        set { PrivateAccess.Set("v0", player, value); }
    }

    private static float InternalDrag { get { return (float)PrivateAccess.Get("internalDrag", player); } }

    private static void PrintVerbose(string msg)
    {
#if (VERBOSE)
        GD.Print(msg);
#endif
    }

    public override void _Ready()
    {
        player = (PlayerTopDown)((PackedScene)ResourceLoader.Load("res://characters/PlayerTopDown.tscn")).Instance();
        sprite = (Sprite)player.GetNode("Sprite");
        origPos = player.Position;
        AddChild(player);
        player.SetPhysicsProcess(false);

        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithIntExtDrag", ItAcceleratesWithIntExtDrag()));
        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithExtDrag", ItAcceleratesWithExtDrag()));
        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithNoDrag", ItAcceleratesWithNoDrag()));

        ResetPlayer(PlayerTopDown.AccelMode.IntExtDrag);
        player.SetPhysicsProcess(true);
    }

    private string Summarize(string description, bool isPass)
    {
        if (isPass) return String.Format("Pass {0}", description);
        return String.Format("FAIL {0}", description);
    }

    private static void ResetPlayer(PlayerTopDown.AccelMode accelMode)
    {
        sprite.Rotation = 0;
        player.Position = origPos;
        player.AccelerationMode = accelMode;
        player.Speed = 400;
        player.Acceleration = 800;
        player.Friction = 1600;
        player.Drag = 0.01F;
        V0 = Vector2.Zero;
        PrivateAccess.Call("UpdateInfo", player);
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

        // Accelerate in one direction then come to a stop by braking passively
        ResetPlayer(PlayerTopDown.AccelMode.IntExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag), 0);
        ApplyInput("ui_right", 1, 5);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        t = (float)TimeToStop(V0.x, -player.Friction, player.Drag);
        ApplyInput(null, 0, t);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, 0));
        if (!IsEqual(V0.x, 0, TOLERANCE)) return false;

        // Accelerate in one direction, then brake to a stop and accelerate in the opposite direction
        ResetPlayer(PlayerTopDown.AccelMode.IntExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag), 0);
        t = (float)TimeToStop(expected.x, -player.Friction, player.Drag);
        expected = new Vector2((float)Velocity(1 - t, 0, -player.Acceleration, player.Drag + InternalDrag), 0);
        ApplyInput("ui_right", 1, 5);
        ApplyInput("ui_left", 1, 1);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        // Accelerate up and right, right, down and right, down, down and left, left, up and left, and up
        ResetPlayer(PlayerTopDown.AccelMode.IntExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag),
                (float)Velocity(5, 0, -player.Acceleration, player.Drag + InternalDrag))
                .Clamped((float)TerminalVelocity(player.Acceleration, player.Drag + InternalDrag));
        ApplyInput(new string[] { "ui_up", "ui_right" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag), 0);
        ApplyInput(new string[] { "ui_right" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag),
                (float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag))
                .Clamped((float)TerminalVelocity(player.Acceleration, player.Drag + InternalDrag));
        ApplyInput(new string[] { "ui_down", "ui_right" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2(0, (float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag));
        ApplyInput(new string[] { "ui_down", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, -player.Acceleration, player.Drag + InternalDrag),
                (float)Velocity(5, 0, player.Acceleration, player.Drag + InternalDrag))
                .Clamped((float)TerminalVelocity(player.Acceleration, player.Drag + InternalDrag));
        ApplyInput(new string[] { "ui_down", "ui_left" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, -player.Acceleration, player.Drag + InternalDrag), 0);
        ApplyInput(new string[] { "ui_left", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, -player.Acceleration, player.Drag + InternalDrag),
                (float)Velocity(5, 0, -player.Acceleration, player.Drag + InternalDrag))
                .Clamped((float)TerminalVelocity(player.Acceleration, player.Drag + InternalDrag));
        ApplyInput(new string[] { "ui_up", "ui_left" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2(0, (float)Velocity(5, 0, -player.Acceleration, player.Drag + InternalDrag));
        ApplyInput(new string[] { "ui_up", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        return true;
    }

    private static bool ItAcceleratesWithExtDrag()
    {
        Vector2 expected;
        float t;

        // Accelerate in one direction then come to a stop by braking passively
        ResetPlayer(PlayerTopDown.AccelMode.ExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag), 0);
        ApplyInput("ui_right", 1, 5);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        t = (float)TimeToStop(V0.x, -player.Friction, player.Drag);
        ApplyInput(null, 0, t);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, 0));
        if (!IsEqual(V0.x, 0, TOLERANCE)) return false;

        // Accelerate in one direction, then brake to a stop and accelerate in the opposite direction
        ResetPlayer(PlayerTopDown.AccelMode.ExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag), 0);
        t = (float)TimeToStop(expected.x, -player.Friction, player.Drag);
        expected = new Vector2((float)Velocity(1 - t, 0, -player.Acceleration, player.Drag), 0);
        ApplyInput("ui_right", 1, 5);
        ApplyInput("ui_left", 1, 1);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        // Accelerate up and right, right, down and right, down, down and left, left, up and left, and up
        ResetPlayer(PlayerTopDown.AccelMode.ExtDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag),
                (float)Velocity(5, 0, -player.Acceleration, player.Drag))
                .Clamped((float)TerminalVelocity(player.Acceleration, player.Drag));
        ApplyInput(new string[] { "ui_up", "ui_right" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag), 0);
        ApplyInput(new string[] { "ui_right" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration, player.Drag),
                (float)Velocity(5, 0, player.Acceleration, player.Drag))
                .Clamped((float)TerminalVelocity(player.Acceleration, player.Drag));
        ApplyInput(new string[] { "ui_down", "ui_right" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2(0, (float)Velocity(5, 0, player.Acceleration, player.Drag));
        ApplyInput(new string[] { "ui_down", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, -player.Acceleration, player.Drag),
                (float)Velocity(5, 0, player.Acceleration, player.Drag))
                .Clamped((float)TerminalVelocity(player.Acceleration, player.Drag));
        ApplyInput(new string[] { "ui_down", "ui_left" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, -player.Acceleration, player.Drag), 0);
        ApplyInput(new string[] { "ui_left", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, -player.Acceleration, player.Drag),
                (float)Velocity(5, 0, -player.Acceleration, player.Drag))
                .Clamped((float)TerminalVelocity(player.Acceleration, player.Drag));
        ApplyInput(new string[] { "ui_up", "ui_left" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2(0, (float)Velocity(5, 0, -player.Acceleration, player.Drag));
        ApplyInput(new string[] { "ui_up", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        return true;
    }

    private static bool ItAcceleratesWithNoDrag()
    {
        Vector2 expected;
        float t;

        // Accelerate in one direction then come to a stop by braking passively
        ResetPlayer(PlayerTopDown.AccelMode.NoDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration), 0).Clamped(player.Speed);
        ApplyInput("ui_right", 1, 5);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        t = (float)TimeToStop(V0.x, -player.Friction);
        ApplyInput(null, 0, t);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, 0));
        if (!IsEqual(V0.x, 0, TOLERANCE)) return false;

        // Accelerate in one direction, then brake to a stop and accelerate in the opposite direction
        ResetPlayer(PlayerTopDown.AccelMode.NoDrag);

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration), 0).Clamped(player.Speed);
        t = (float)TimeToStop(expected.x, -player.Friction);
        expected = new Vector2((float)Velocity(0.4 - t, 0, -player.Acceleration), 0).Clamped(player.Speed);
        ApplyInput("ui_right", 1, 5);
        ApplyInput("ui_left", 1, 0.4F);
        PrintVerbose(String.Format("  /{0:F4}\n  \\{1:F4}", V0.x, expected.x));
        if (!IsEqual(V0.x, expected.x, TOLERANCE)) return false;

        // Accelerate up and right, right, down and right, down, down and left, left, up and left, and up
        ResetPlayer(PlayerTopDown.AccelMode.NoDrag);

        expected = new Vector2((float)Velocity(8, 0, player.Acceleration), (float)Velocity(8, 0, -player.Acceleration))
                .Clamped(player.Speed);
        ApplyInput(new string[] { "ui_up", "ui_right" }, new float[] { 1, 1 }, 8);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, player.Acceleration), 0).Clamped(player.Speed);
        ApplyInput(new string[] { "ui_right" }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(8, 0, player.Acceleration), (float)Velocity(8, 0, player.Acceleration))
                .Clamped(player.Speed);
        ApplyInput(new string[] { "ui_down", "ui_right" }, new float[] { 1, 1 }, 8);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2(0, (float)Velocity(5, 0, player.Acceleration)).Clamped(player.Speed);
        ApplyInput(new string[] { "ui_down", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(8, 0, -player.Acceleration), (float)Velocity(8, 0, player.Acceleration))
                .Clamped(player.Speed);
        ApplyInput(new string[] { "ui_down", "ui_left" }, new float[] { 1, 1 }, 8);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(5, 0, -player.Acceleration), 0).Clamped(player.Speed);
        ApplyInput(new string[] { "ui_left", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2((float)Velocity(8, 0, -player.Acceleration), (float)Velocity(8, 0, -player.Acceleration))
                .Clamped(player.Speed);
        ApplyInput(new string[] { "ui_up", "ui_left" }, new float[] { 1, 1 }, 8);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        expected = new Vector2(0, (float)Velocity(5, 0, -player.Acceleration)).Clamped(player.Speed);
        ApplyInput(new string[] { "ui_up", }, new float[] { 1, 1 }, 5);
        PrintVerbose(String.Format("  /{0:F4}, {1:F4}\n  \\{2:F4}, {3:F4}", V0.x, V0.y, expected.x, expected.y));
        if (!IsEqual(V0.x, expected.x, TOLERANCE) || !IsEqual(V0.y, expected.y, TOLERANCE)) return false;

        return true;
    }
}
