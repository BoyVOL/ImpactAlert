using Godot;

public abstract partial class ObjectRailAddon: SelfUnloadingNode{

	public RailPointList Rail;

    //Method, which sets up current rail addon to it's rail 
    public abstract void AddToRail();

	public override void _EnterTree()
	{
		base._EnterTree();
        if(Parent.PredictionRail != null){  
		    Rail = Parent.PredictionRail;
            AddToRail();
            if (Parent.PhysRail != null)
            {                
                ObjectRailAddon copy = (ObjectRailAddon)Duplicate();
                copy.Rail = Parent.PhysRail;
                copy.Parent = Parent;
                copy.AddToRail();
            }
        } else {
            if (Parent.PhysRail != null)
            {
                Rail = Parent.PhysRail;
                AddToRail();
            }
        }
	}
}