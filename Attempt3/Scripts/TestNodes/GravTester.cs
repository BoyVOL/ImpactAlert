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

    public Vector2 GetGravAccel(Vector2 AtPoint)
    {
        Vector2 Pos = this.GetParent<Node2D>().Position;
        float RSqr = (Pos - AtPoint).LengthSquared();
        double Module = (Mass * PhysConst.GRAV) / RSqr;
        return (Pos - AtPoint).Normalized()*(float)Module;
    }

    public void Modify(RailPoint point, double step)
    {
        point.Acceleration += GetGravAccel(point.Position);
    }
}
