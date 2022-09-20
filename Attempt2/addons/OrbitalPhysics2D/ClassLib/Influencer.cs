using Godot;

[Tool]
public abstract class Influencer: SelfUnloadingNode{

    [Export]
    /// <summary>
    /// Max radius in which object can spread influence
    /// </summary>
    public float InfRad;

    [Export]
    public Color DebugColor;

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.InfList.Add(this);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }
}