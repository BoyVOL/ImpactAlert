using Godot;
using System;
using System.Collections.Generic;

public partial class PhysRailNode: Node2D{

	
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
}