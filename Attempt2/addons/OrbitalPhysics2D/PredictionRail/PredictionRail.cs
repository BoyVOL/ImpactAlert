using Godot;
using System.Collections.Generic;

public class PredictionRail: Node2D{

    public Node2D Parent;

    RailPointList RailPoints = new RailPointList();

    public override void _EnterTree(){
        base._EnterTree();
        Parent = GetParent<CustomPhysObject>();
    }
        
}
