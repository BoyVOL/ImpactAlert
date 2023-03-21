using Godot;

[Tool]
public abstract partial class Influencer: SelfUnloadingNode{

	[Export]
	/// <summary>
	/// Max radius in which phys object with this influencer can influence others
	/// </summary>
	public float InfRad;

	public RailPointList Rail;

	[Export]
	public Color DebugColor;

	public override void _EnterTree()
	{
		base._EnterTree();
		Parent.PhysRail.Influencers.Add(this);
		Parent.PredictionRail.Influencers.Add(this);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}