using Godot;

/// <summary>
/// Node class that made to be a parent of PhysRailNode
/// </summary>
public partial class SelfUnloadingNode: Node{

    public PhysRailNode Parent;

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent = GetParent<PhysRailNode>();
    }

    public override void _Ready()
    {
        base._Ready();
        Parent.CallDeferred("remove_child",this);
    }
}