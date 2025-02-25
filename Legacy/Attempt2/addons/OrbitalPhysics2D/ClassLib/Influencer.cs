using Godot;

[Tool]
public abstract partial class Influencer: SelfUnloadingNode{

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

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
