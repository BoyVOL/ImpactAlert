using Godot;

/// <summary>
/// Base class for rail influencers that change rail's motion
/// </summary>
public partial class ObjectInfluencer:ObjectRailAddon{


	/// <summary>
	/// Metod for infController to combine acceleration 
	/// provided by this specific one with others
	/// </summary>
	/// <param name="Target"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public virtual Vector2 GetAccel(RailPoint Target, int id){
		return Vector2.Zero;
	}

	
	[Export]
	/// <summary>
	/// Max radius in which phys object with this influencer can influence others
	/// </summary>
	public float InfRad;

	[Export]
	public Color DebugColor;

	public override void AddToRail(){
		Rail.Influencers.Add(this);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
