using Godot;
using System;
using PhysRails2D;
using System.Linq;

public partial class AutoRailNode : RailNode
{
    RailAutomator Automator;

    public override void _EnterTree()
    {
        base._EnterTree();
        Automator = GetNode<RailAutomator>("/root/RailAutomator");
        Automator.rails.Add(this);
        Automator.INfluencers.AddRange(OnwInfs);
        Influencers = Automator.INfluencers;
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Automator.rails.Remove(this);
    }


}
