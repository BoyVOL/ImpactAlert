using Godot;
using System.Collections.Generic;

public class Collider:Rangefinder{

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.PhysNode.CollContr.Add(this);
        Parent.Collider = this;
    }
}