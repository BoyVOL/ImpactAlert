using Godot;
using System.Collections.Generic;

public class CollisionController: PhysicsControlAddon{
    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.CollContr = this;
    }
}
