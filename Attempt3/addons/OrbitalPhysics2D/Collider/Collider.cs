using Godot;
using System.Collections.Generic;

public partial class Collider:Approacher{

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.PhysNode.CollContr.Add(this);
        Parent.Collider = this;
    }
}