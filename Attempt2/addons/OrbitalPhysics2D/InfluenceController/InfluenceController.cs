using Godot;

public class InfluenceController: PhysicsControlAddon{
    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.InfContr = this;
    }
}
