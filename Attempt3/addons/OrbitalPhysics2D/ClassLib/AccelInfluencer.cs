using Godot;

public abstract partial class AccelInfluencer:Influencer{

	public override void _EnterTree()
	{
		base._EnterTree();
		Parent.PhysNode.InfContr.Add(this);
	}

	/// <summary>
	/// Metod for infController to combine acceleration 
	/// provided by this specific one with others
	/// </summary>
	/// <param name="Target"></param>
	/// <param name="PhysRail"></param>
	/// <param name="id"></param>
	/// <returns></returns>
	public abstract Vector2 GetAccel(RailPoint Target, bool PhysRail, int id);

}
