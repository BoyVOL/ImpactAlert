using Godot;

/// <summary>
/// Base class for rail influencers that change rail's motion
/// </summary>
public abstract partial class AccelInfluencer:Influencer{


	public override void _EnterTree()
	{
		base._EnterTree();
		Parent.PhysRail.Influencers.Add(this);
		Rail = Parent.PhysRail;
		Influencer copy = (Influencer)Duplicate();
		Parent.PredictionRail.Influencers.Add(copy);
		copy.Rail = Parent.PredictionRail;
		copy.Parent = Parent;
	}

	/// <summary>
	/// Metod for infController to combine acceleration 
	/// provided by this specific one with others
	/// </summary>
	/// <param name="Target"></param>
	/// <param name="PhysRail"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public abstract Vector2 GetAccel(RailPoint Target, int id);

}
