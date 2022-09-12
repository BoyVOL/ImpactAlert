using Godot;

[Tool]
public class Influencer: Node{

    protected CustomPhysObject Parent;

    [Export]
    /// <summary>
    /// MaxRadius which can spread influence
    /// </summary>
    public float InfRad;

    [Export]
    public Color DebugColor;

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent = GetParent<CustomPhysObject>();
        Parent.InfList.Add(this);
    }

    public override void _Ready()
    {
        base._Ready();
        if(!Engine.EditorHint){
            Parent.CallDeferred("remove_child",this);
        }
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }
}