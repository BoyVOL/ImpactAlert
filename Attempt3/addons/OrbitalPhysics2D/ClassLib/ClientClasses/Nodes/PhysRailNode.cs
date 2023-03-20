using Godot;
using System;
using System.Collections.Generic;

public partial class PhysRailNode: Node2D{

	/// <summary>
	/// Ref to controlling object
	/// </summary>
	public PhysicsControlNode PhysNode = null;

	
	[Export]
	public Color PhysRailColor;

	/// <summary>
	/// List of all simulation points of this object
	/// </summary>
	/// <typeparam name="PhysRail"></typeparam>
	/// <returns></returns>
	public RailPointList PhysRail;

	public PhysRailNode():base(){
		PhysRail = new RailPointList(this);
	}

	/// <summary>
	/// Method for updating node pos according to simulation
	/// </summary>
	public void UpdatePos(){
		Position = PhysRail[0].Position;
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdatePos();
		base._PhysicsProcess(delta);
	}

	public virtual void LoadObject(){
		PhysNode.PhysRail.Add(PhysRail);
	}

	public void DrawPhys(){
			Vector2[] Points = new Vector2[PhysRail.Count];
			for (int i = 0; i < PhysRail.Count; i++)
			{
				Points[i] = PhysRail[i].Position-PhysRail[0].Position;
			}
			if(Points.Length > 1) DrawPolyline(Points,PhysRailColor,2);
	}

	public override void _Draw()
	{
		base._Draw();
		#if DEBUG
		DrawPhys();
		#endif
	}
}