using Godot;
using System;
using PhysRails2D;

public partial class AutoRailNode : RailNode
{
    RailAutomator Automator;

    public override void _EnterTree()
    {
        base._EnterTree();
        Automator = GetNode<RailAutomator>("/root/RailAutomator");
        Automator.rails.Add(Items);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Automator.rails.Remove(Items);
    }


}
