using Godot;
using System.Collections.Generic;

public class PredictionRailController: Node2D{

    public Node2D Parent;

    public override void _EnterTree(){
        base._EnterTree();
        Parent = GetParent<CustomPhysObject>();
    }
        
}
