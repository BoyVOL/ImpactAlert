using Godot;

/// <summary>
/// Base class for physic control node
/// </summary>
public partial class PhysicsControlAddon{
    public PhysicsControlNode Parent;

    public PhysicsControlAddon(PhysicsControlNode parent){
        Parent = parent;
    }
}