using Godot;
using System.Collections.Generic;

public class GravityInfluencer: CustomPhysObject{

    public override void _EnterTree(){
        base._EnterTree();
        PhysNode.AddInfluencer(this);
    }

    public override void _ExitTree(){
        PhysNode.RemoveInfluencer(this);
    }
        
}
