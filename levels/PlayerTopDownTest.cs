using Com.NarvinSingh.Test;
using Godot;
using System;
using static Com.NarvinSingh.Physics.Kinematics;
using static Com.NarvinSingh.Test.Approximate;

public class PlayerTopDownTest : Node
{
    private const float FPS = 60;
    private const float DT = 1.0F / FPS;

    private static PlayerTopDown player;
    private static Vector2 origPos;
    private static Vector2 expected;
    //private static float t;

    private static Vector2 V0
    {
        get { return (Vector2)PrivateAccess.Get("v0", player); }
        set { PrivateAccess.Set("v0", player, value); }
    }

    private static float IntDrag { get { return (float)PrivateAccess.Get("intDrag", player); } }

    public override void _Ready()
    {
        player = (PlayerTopDown)((PackedScene)ResourceLoader.Load("res://characters/PlayerTopDown.tscn")).Instance();
        origPos = player.Position;
        AddChild(player);
        player.SetPhysicsProcess(false);

        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithIntExtDrag", ItAcceleratesWithIntExtDrag()));
        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithExtDrag", ItAcceleratesWithExtDrag()));
        GD.Print(Summarize("PlayerTopDownTest.ItAcceleratesWithNoDrag", ItAcceleratesWithNoDrag()));
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
        V0 = Vector2.Zero;
    }

    private static void ApplyInput(string[] actions, float[] strengths, float duration = DT)
    {
        int frames = (int)Math.Round(FPS * duration);

        for (int i = 0; i < actions.Length; i++) Input.ActionPress(actions[i], strengths[i]);
        for (int i = 0; i < frames; i++) player._PhysicsProcess(DT);
        for (int i = 0; i < actions.Length; i++) Input.ActionRelease(actions[i]);
    }

    private static void ApplyInput(string action, float strength = 1, float duration = DT)
    {
        ApplyInput(new string[] { action }, new float[] { strength }, duration);
    }

    private static bool ItAcceleratesWithIntExtDrag()
    {
        ResetPlayer(PlayerTopDown.AccelMode.IntExtDrag);

        expected = new Vector2((float)Velocity(5, V0.x, player.Acceleration, player.Drag + IntDrag), 0)
              .Clamped(player.Speed);
        ApplyInput("ui_right", 1, 5);
        if (!IsEqual(V0.x, expected.x, 2)) return false;

        return true;
    }

    private static bool ItAcceleratesWithExtDrag()
    {
        ResetPlayer(PlayerTopDown.AccelMode.ExtDrag);

        expected = new Vector2((float)Velocity(5, V0.x, player.Acceleration, player.Drag), 0)
              .Clamped(player.Speed);
        ApplyInput("ui_right", 1, 5);
        if (!IsEqual(V0.x, expected.x, 2)) return false;

        return true;
    }

    private static bool ItAcceleratesWithNoDrag()
    {
        ResetPlayer(PlayerTopDown.AccelMode.NoDrag);

        expected = new Vector2((float)Velocity(5, V0.x, player.Acceleration), 0).Clamped(player.Speed);
        ApplyInput("ui_right", 1, 5);
        if (!IsEqual(V0.x, expected.x, 2)) return false;

        return true;
    }
}
