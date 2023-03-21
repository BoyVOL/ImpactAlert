using Godot;
using System.Collections.Generic;

public partial class PredictionRailController: PhysRailNode{
    
    CustomPhysObject Parent;

    public PredictionRailController(): base(){
        
    }

    public override void _EnterTree(){
        base._EnterTree();
        Parent = GetParent<CustomPhysObject>();
    }
        
}
