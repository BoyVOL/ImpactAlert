using Godot;
using System;
using System.Collections.Generic;

public partial class CustomPhysObject: PhysRailNode{

	[Export]
	private Vector2 FirstPointSpeed;

	[Export]
	private float FirstPointRotSpeed;

	[Export]
	private Vector2 FirstPointAccel;

	[Export]
	private float FirstPointRotAccel;

	public CustomPhysObject(): base(){		
		PredictionRail = new RailPointList(this);
		PhysRail = new RailPointList(this);
	}

	/// <summary>
	/// Method for setting up first point of this rail
	/// </summary>
	public void SetFirstPoint(){
		RailPoint Point = new RailPoint();
		Point.Position = Position;
		Point.Rotation = Rotation;
		Point.Speed = FirstPointSpeed;
		Point.RotSpeed = FirstPointRotSpeed;
		Point.Acceleration = FirstPointAccel;
		Point.RotAccel = FirstPointRotAccel;
		PhysRail.SetFirstPoint(Point);
		PredictionRail.SetFirstPoint(Point);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
        //GD.Print("PHYS RAIL = "+PhysRail.Stringify(5));
	}

	public override void _EnterTree()
	{
		SetFirstPoint();
		base._EnterTree();
	}

	public override void _Draw()
	{
		base._Draw();
		#if DEBUG
		#endif
	}
}
