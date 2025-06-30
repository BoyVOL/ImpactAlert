using Godot;
using PhysRails2D;
using System;

public partial class GravNode : Node, IINfluencer
{
    [Export]
    float Mass = 1000;
    
    RailNode Rail;

    public override void _EnterTree()
    {
        base._EnterTree();
        Rail = this.GetParent<RailNode>();
    }

    public Vector2 GetGravAccel(Vector2 AtPoint, Vector2 FromPoit)
    {
        Vector2 Pos = FromPoit;
        float RSqr = (Pos - AtPoint).LengthSquared();
        double Module = (Mass * PhysConst.GRAV) / RSqr;
        return (Pos - AtPoint).Normalized() * (float)Module;
    }


    public void Modify(RailPoint Point, double step)
    {
        Point.Acceleration += GetGravAccel(Point.Position,Rail.Items[Rail.Items.GetBeforeTime(Point.time)].Position);
    }
}
