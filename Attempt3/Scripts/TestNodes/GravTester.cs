using Godot;
using System;
using PhysRails2D;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using System.Drawing;

public partial class GravTester : Node2D, IINfluencer
{
    [Export]
    float Mass = 1000;

    [Export]
    RailNode Rail;

    public Vector2 GetGravAccel(Vector2 AtPoint, double shift = 0)
    {
        Vector2 Pos = Rail.Items[Rail.Items.GetBeforeTime((float)shift)].Position;
        float RSqr = (Pos - AtPoint).LengthSquared();
        double Module = (Mass * PhysConst.GRAV) / RSqr;
        return (Pos - AtPoint).Normalized() * (float)Module;
    }

    /// <summary>
    /// Method that adds gravitational influence to point
    /// </summary>
    /// <param name="point"></param>
    /// <param name="step"></param>
    /// <param name="shift"></param>
    public void Modify(RailPoint point, double step, double shift = 0)
    {
        point.Acceleration += GetGravAccel(point.Position,shift);
    }
}
