using Godot;
using System.Collections.Generic;
using System;

/// <summary>
/// Класс, отвечающий за обработку физических взаимодействий 
/// </summary>
public partial class PhysicsControlNode: Node{

	private List<PhysRailNode> Objects = new List<PhysRailNode>();

	public RPListController PhysRail;

	public RPListController PredictRail;

	public RPListController FramePredictRail;

	[Export]
	public float MaxPredictionRange = 50;

	public List<PhysRailNode> NotLoaded = new List<PhysRailNode>();

	public PhysicsControlNode():base(){
		PhysRail = new RPListController(this);
		PredictRail = new RPListController(this);
		FramePredictRail = new RPListController(this);
	}

	public void Add(PhysRailNode Object){
		NotLoaded.Add(Object);
	}

	public void LoadAll(){
		foreach (PhysRailNode item in NotLoaded)
		{
			Objects.Add(item);
			item.LoadObject();
		}
		NotLoaded.Clear();
	}
	
	public void Remove(PhysRailNode Object){
		Objects.Remove(Object);
		if(Object.PhysRail != null) PhysRail.Remove(Object.PhysRail);
		if(Object.PredictionRail != null) PredictRail.Remove(Object.PredictionRail);
	}

	public override void _EnterTree()
	{
		base._EnterTree();
	}

	public void PhysRailUpdate(double delta){
		PhysRail.Reset();
		PhysRail.UpdateAccel(0);
		PhysRail.AppendPoint((float)delta,1);
		PhysRail.UpdateAccel(1);
		PhysRail.LeapFrogAdjust(1,(float)delta);
	}

	public int CalcStepCount(float delta){
		return (int)(MaxPredictionRange/delta);
	}

	public void PredictRailUpdate(float delta){
		PredictRail.LoadFromPhys();
		for (int i = 0; i < CalcStepCount(delta); i++)
		{
			PredictRail.UpdateAccel(i);
			PredictRail.AppendPoint(delta,i+1);
			PredictRail.UpdateAccel(i+1);
			PredictRail.LeapFrogAdjust(i+1,(float)delta);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess((float)delta);
		LoadAll();
		PhysRailUpdate((float)delta);
		PredictRailUpdate((float)delta);
	}

}
