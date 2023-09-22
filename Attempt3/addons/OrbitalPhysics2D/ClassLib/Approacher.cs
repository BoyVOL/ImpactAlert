using Godot;
using System.Collections.Generic;

/// <summary>
/// Class that searches for all approaches to other Approachers
/// </summary>
public partial class Approacher:SelfUnloadingNode{

	public struct Approach{
		public float time;

		public Approacher Approacher;

		public Approach(float Time, Approacher approacher){
			time = Time;
			Approacher = approacher;
		}
	}

	[Export]
	public float Radius = 1;

	public List<Approach> Approaches = new List<Approach>();

	[Export]
	public Color RadiusColor;

	[Export]
	public Color ApproachColor;
	
	protected Approach ScanForApproaches(Collider collider, RailPointList OwnRail, RailPointList OtherRail,int id){
		float TimeFrame = OwnRail[id+1].time-OwnRail[id].time;
		float calltime = OwnRail[id].CPA(OtherRail[id],TimeFrame)+OwnRail[id].time;
		Approach Result = new Approach(calltime,collider);
		return Result;
	}
	
    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.Approacher = this;
    }

	public Approach ScanForApproaches(Collider collider, bool Phys,int id){
		if(Phys) {
			return ScanForApproaches(collider,Parent.PhysRail,collider.Parent.PhysRail,id);
		} else {
			return ScanForApproaches(collider,Parent.PredictionRail,collider.Parent.PredictionRail,id);
		}
	}

	public void AddApproach(Approach Coll){
		Approaches.Add(Coll);
	}

	public void ClearApproaches(){
		Approaches.Clear();
	}
}
