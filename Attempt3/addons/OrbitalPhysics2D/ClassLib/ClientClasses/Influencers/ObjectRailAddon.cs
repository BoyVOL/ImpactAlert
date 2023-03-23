using Godot;

public abstract partial class ObjectRailAddon: SelfUnloadingNode{

	public RailPointList Rail;

    //Method, which sets up current rail addon to it's rail 
    public abstract void AddToRail();

	public override void _EnterTree()
	{
		base._EnterTree();
		Rail = Parent.PredictionRail;
        AddToRail();
		ObjectRailAddon copy = (ObjectRailAddon)Duplicate();
		copy.Rail = Parent.PhysRail;
		copy.Parent = Parent;
        copy.AddToRail();
	}
}