using Godot;

public abstract partial class PhysInfluencer:Influencer{

	public override void _EnterTree()
	{
		base._EnterTree();
		Parent.PhysNode.InfContr.Add(this);
	}

	public abstract Vector2 GetAccel(RailPoint Target, bool PhysRail, int id);

}
