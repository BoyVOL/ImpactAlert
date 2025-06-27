using Godot;
using System;

public partial class GravNode : Node
{
    [Export]
    float Mass = 1000;

    public Vector2 GetGravAccel(Vector2 AtPoint, Vector2 FromPoit, double shift = 0)
    {
        Vector2 Pos = FromPoit;
        float RSqr = (Pos - AtPoint).LengthSquared();
        double Module = (Mass * PhysConst.GRAV) / RSqr;
        return (Pos - AtPoint).Normalized() * (float)Module;
    }

    public Vector2 GetGravAccel(Vector2 AtPoint, double shift = 0)
    {
        return GetGravAccel(AtPoint, GetParent<Node2D>().Position, shift);
    }
    
}
