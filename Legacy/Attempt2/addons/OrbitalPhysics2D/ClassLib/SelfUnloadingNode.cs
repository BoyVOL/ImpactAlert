using Godot;

public partial class SelfUnloadingNode: Node{

    public CustomPhysObject Parent;

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent = GetParent<CustomPhysObject>();
    }

    public override void _Ready()
    {
        base._Ready();
        Parent.CallDeferred("remove_child",this);
    }
}