using Godot;
using System.Collections.Generic;

public partial class PredictionRailController: PhysRailNode{
    
    public CustomPhysObject Parent;

    public PredictionRailController(): base(){
        PhysRail=null;		
		PredictionRail = new RailPointList(this);
    }

    public override void _EnterTree(){
        base._EnterTree();
        Parent = GetParent<CustomPhysObject>();
    }

    public override void _PhysicsProcess(double delta){
        base._PhysicsProcess(delta);
        //GD.Print("NEW RAIL = "+PredictionRail.Stringify(5));
    }
        
}