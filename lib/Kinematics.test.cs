using Physics;
using System;
using System.Linq;

public static class KinematicsTest
{
    public static void WriteData()
    {
        Run("C:\\Temp\\Kinematics\\AccelerateNoDrag.csv", 0, 100, 0);
        Run("C:\\Temp\\Kinematics\\DeccelerateNoDrag.csv", 100, -100, 0);
        Run("C:\\Temp\\Kinematics\\DragNoAcceleration.csv", 50, 0, 0.01);
        Run("C:\\Temp\\Kinematics\\AccelerateAndDrag.csv", 0, 100, 0.01);
        Run("C:\\Temp\\Kinematics\\DeccelerateAndDrag.csv", 100, -100, 0.01);
 
        Run2("C:\\Temp\\Kinematics\\AccelerateNoDrag2.csv", 0, 100, 0);
        Run2("C:\\Temp\\Kinematics\\DeccelerateNoDrag2.csv", 100, -100, 0);
        Run2("C:\\Temp\\Kinematics\\DragNoAcceleration2.csv", 50, 0, 0.01);
        Run2("C:\\Temp\\Kinematics\\AccelerateAndDrag2.csv", 0, 100, 0.01);
        Run2("C:\\Temp\\Kinematics\\DeccelerateAndDrag2.csv", 100, -100, 0.01);
   }

    public static bool ItAcceleratesNoDrag()
    {
        return new double[] {
            Kinematics.Velocity(0, 1, 100),
            Kinematics.Velocity(50, 1, 100),
            Kinematics.Velocity(0, 1, -100),
            Kinematics.Velocity(50, 1, -100),
            Kinematics.Velocity(-50, 1, 100),
            Kinematics.Velocity(-50, 1, -100),
        }.SequenceEqual(new double[] {
            100.0,
            150.0,
            -100.0,
            -50.0,
            50.0,
            -150.0,
        });
    }

    public static bool ItDragsNoAcceleration()
    {
        return new double[] {
            Kinematics.Velocity(0, 1, 0, 0.01),
            Kinematics.Velocity(50, 1, 0, 0.01),
        }.SequenceEqual(new double[] {
            0,
            50.0 / (0.01 * 50 * 1 - 1),
        });
    }

    private static void Run(string path, double v0, double a, double d, double t = 3.0, double dt = 1.0 / 60)
    {
        int frames = (int)(t / dt);
        String[] lines = new String[frames];
        double v = v0;

        for (int i = 1; i <= frames; i++)
        {
            v = Kinematics.Velocity(v, dt, a, d);
            lines[i - 1] =  String.Format("{0},{1}", i * dt, v);
        }

        System.IO.File.WriteAllLines(path, lines);
    }

    private static void Run2(string path, double v0, double a, double d, double t = 3.0, double dt = 1.0 / 60)
    {
        int frames = (int)(t / dt);
        String[] lines = new String[frames];
        double v = v0;

        for (int i = 1; i <= frames; i++)
        {
            v = Kinematics.Velocity2(v, dt, a, d);
            lines[i - 1] =  String.Format("{0},{1}", i * dt, v);
        }

        System.IO.File.WriteAllLines(path, lines);
    }
}
