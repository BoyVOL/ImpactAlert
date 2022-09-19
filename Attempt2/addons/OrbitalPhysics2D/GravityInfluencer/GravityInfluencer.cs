using Godot;
using System.Collections.Generic;

public class GravityInfluencer: PhysInfluencer{

    [Export]
    public float massMultiplier=1;

    public override Vector2 GetAccel(RailPointList ownRail, RailPointList targetRail, int id)
    {
        float M = Parent.mass*massMultiplier;
        float R2 = ownRail[id].Position.DistanceSquaredTo(targetRail[id].Position);
        Vector2 Dir = targetRail[id].Position.DirectionTo(ownRail[id].Position);
        float Module = (float)(PhysConst.GRAV*(M/R2));
        return Module*Dir;
    }
}
