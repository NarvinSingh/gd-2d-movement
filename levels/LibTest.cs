using Com.NarvinSingh.UnitTest;
using Godot;
using System;

public class LibTest : Node
{
    public override void _Ready()
    {
        GD.Print(Summarize("AdjustmentTest.ItGetsSign", AdjustmentTest.ItGetsSign()));
        GD.Print(Summarize("AdjustmentTest.ItAbsolutes", AdjustmentTest.ItAbsolutes()));
        GD.Print(Summarize("AdjustmentTest.ItClamps", AdjustmentTest.ItClamps()));

        GD.Print(Summarize("ApproximateTest.ItCalculatesIsEqual", ApproximateTest.ItCalculatesIsEqual()));
        GD.Print(Summarize("ApproximateTest.ItReimannSums", ApproximateTest.ItReimannSums()));
        GD.Print(Summarize("ApproximateTest.ItReimannSumsTo", ApproximateTest.ItReimannSumsTo()));

        GD.Print(Summarize("KinematicsTest.ItCalculatesTimeToStop", KinematicsTest.ItCalculatesTimeToStop()));
        GD.Print(Summarize("KinematicsTest.ItCalculatesVelocity", KinematicsTest.ItCalculatesVelocity()));
        GD.Print(Summarize(
                "KinematicsTest.ItCalculatesTerminalVelocity", KinematicsTest.ItCalculatesTerminalVelocity()));

        GD.Print(Summarize("RangeTest.ItInstantiates", RangeTest.ItInstantiates()));
        GD.Print(Summarize("RangeTest.ItIncludes", RangeTest.ItIncludes()));

        GD.Print(Summarize("AxisTest.ItCalcsExtents", AxisTest.ItCalcsExtents()));
        GD.Print(Summarize("AxisTest.ItInstantiates", AxisTest.ItInstantiates()));
        GD.Print(Summarize("AxisTest.ItMaps", AxisTest.ItMaps()));
        GD.Print(Summarize("AxisTest.ItUnmaps", AxisTest.ItUnmaps()));

        GD.Print(Summarize("PrivateAccessTest.ItGets", PrivateAccessTest.ItGets()));
        GD.Print(Summarize("PrivateAccessTest.ItSets", PrivateAccessTest.ItSets()));
        GD.Print(Summarize("PrivateAccessTest.ItCalls", PrivateAccessTest.ItCalls()));

        GD.Print("Goodbye");
        GetTree().Quit();
    }

    private string Summarize(string description, bool isPass)
    {
        if (isPass) return String.Format("Pass {0}", description);
        return String.Format("FAIL {0}", description);
    }
}
