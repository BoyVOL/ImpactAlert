using Godot;

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