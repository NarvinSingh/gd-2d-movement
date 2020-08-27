using Godot;
using System;
using static Com.NarvinSingh.Physics.Kinematics;

public class TopDownTest : Node2D
{
    private const float FPS = 60;
    private const float DT = 1.0F / FPS;

    private PlayerTopDown player;

    public override void _Ready()
    {
        Vector2 origPos;
        Vector2 expected;
        float t;

        player = (PlayerTopDown)GetNode("PlayerTopDown");
        origPos = player.Position;
        player.SetPhysicsProcess(false);

        // Run tests
        expected = new Vector2((float)Velocity(5, player.DbgVelocity.x, player.Acceleration, player.DbgTotalDrag), 0)
                .Clamped(player.Speed);
        GD.Print(Summarize("Accelerate from rest for 5s", ApplyInput("ui_right", 1, 5), expected));

        expected = new Vector2((float)Velocity(0.1, player.DbgVelocity.x, -player.Friction, player.Drag), 0);
        GD.Print(Summarize("Then decelerate from for 0.1s", ApplyInput("ui_left", 1, 0.1F), expected));

        t = (float)TimeToStop(player.DbgVelocity.x, -player.Friction, player.Drag);
        GD.Print(Summarize("Continue to decelerate until stopped", ApplyInput("ui_left", 1, t), Vector2.Zero));

        expected = new Vector2((float)Velocity(0.1, player.DbgVelocity.x, player.Acceleration, player.DbgTotalDrag), 0);
        GD.Print(Summarize("Then accelerate for 0.1s", ApplyInput("ui_right", 1, 0.1F), expected));

        t = (float)TimeToStop(player.DbgVelocity.x, -player.Friction, player.Drag);
        expected = new Vector2(-(float)Velocity(0.1 - t, 0, player.Acceleration, player.DbgTotalDrag), 0);
        GD.Print(Summarize("Then switch direction for 0.1s", ApplyInput("ui_left", 1, 0.1F), expected));

        player.DbgVelocity = Vector2.Zero;
        expected = new Vector2(
                (float)Velocity(5, player.DbgVelocity.x, player.Acceleration * Math.Sqrt(2) / 2.0F,
                        player.DbgTotalDrag),
                (float)Velocity(5, player.DbgVelocity.y, player.Acceleration * Math.Sqrt(2) / 2.0F,
                        player.DbgTotalDrag)).Clamped(player.Speed);
        GD.Print(Summarize("Accelerate diagonally from rest for 5s",
                ApplyInput(new string[] { "ui_right", "ui_up" }, new float[] { 1, 1 }, 5), expected));

        // Restore control to player
        player.Position = origPos;
        player.DbgVelocity = Vector2.Zero;
        player.DbgUpdateInfo();
        player.SetPhysicsProcess(true);
    }

    private string Summarize(string description, Vector2 actual, Vector2 expected, int precision = 2)
    {
        if (Math.Round(actual.x, precision) == Math.Round(expected.x, precision)
                && Math.Round(actual.x, precision) == Math.Round(expected.x, precision))
        {
            return String.Format("Pass {0}: {1} {2}", description, actual, expected);
        }

        return String.Format("FAIL {0}: actual = {1}, expected = {2}", description, actual, expected);
    }

    private Vector2 ApplyInput(string[] actions, float[] strengths, float duration = DT)
    {
        int frames = (int)Math.Round(FPS * duration);

        for (int i = 0; i < actions.Length; i++) Input.ActionPress(actions[i], strengths[i]);
        for (int i = 0; i < frames; i++) player._PhysicsProcess(DT);
        for (int i = 0; i < actions.Length; i++) Input.ActionRelease(actions[i]);

        return player.DbgVelocity;
    }

    private Vector2 ApplyInput(string action, float strength = 1, float duration = DT)
    {
        return ApplyInput(new string[] { action }, new float[] { strength }, duration);
    }
}
