using Godot;
using System.Collections.Generic;

public class GravityInfluencer: PhysInfluencer{

    [Export]
    public float massMultiplier=1;

    public override Vector2 GetAccel(RailPoint target, bool physRail, int id)
    {
        RailPointList List;
        if(physRail){
            List = Parent.PhysRail;
        } else {
            List = Parent.PredictionRail;
        }
        float M = Parent.mass*massMultiplier;
        float R2 = List[id].Position.DistanceSquaredTo(target.Position);
        Vector2 Dir = target.Position.DirectionTo(List[id].Position);
        float Module = (float)(PhysConst.GRAV*(M/R2));
        return Module*Dir;
    }
}
