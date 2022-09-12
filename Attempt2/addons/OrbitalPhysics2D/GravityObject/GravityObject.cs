using Godot;
using System.Collections.Generic;

public class GravityObject: CustomPhysObject{

    public override void _EnterTree(){
        base._EnterTree();
        PhysNode.AddObject(this);
    }

    public override void _ExitTree(){
        PhysNode.RemoveObject(this);
    }
}
