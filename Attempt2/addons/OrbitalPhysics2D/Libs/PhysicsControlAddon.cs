using Godot;

public class PhysicsControlAddon:Node{
    public PhysicsControlNode Parent;

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent = GetParent<PhysicsControlNode>();
    }
}