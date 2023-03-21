using Godot;
using System.Collections.Generic;

public partial class GravityInfluencer: ObjectInfluencer{

    [Export]
    public float massMultiplier=1;

    public override Vector2 GetAccel(RailPoint target, int id)
    {
        float M = Parent.mass*massMultiplier;
        float R2 = Rail[id].Position.DistanceSquaredTo(target.Position);
        Vector2 Dir = target.Position.DirectionTo(Rail[id].Position);
        float Module = (float)(PhysConst.GRAV*(M/R2));
        return Module*Dir;
    }
}
