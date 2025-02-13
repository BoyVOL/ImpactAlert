using Godot;
using System.Collections.Generic;

public partial class PhysInfController: InfController<PhysInfluencer>{

    public PhysInfController(PhysicsControlNode parent):base(parent){

    }

    /// <summary>
    /// Method for combining acceleration at specific id of the rail.
    /// </summary>
    /// <param name="target">target rail point</param>
    /// <param name="physRail">Prediction or physical rail</param>
    /// <param name="id"></param>
    /// <returns></returns>
    public Vector2 CombineAccels(RailPoint target,bool physRail, int id, RailPointList Exclude){
        Vector2 result = Vector2.Zero;
        foreach (var item in Items)
        {   
            if((physRail && item.Parent.PhysRail != Exclude) || (!physRail && item.Parent.PredictionRail != Exclude)){  
                result += item.GetAccel(target,physRail, id);
            }
        }
        return result;
    }
}
