using Godot;
using System;
using System.Collections.Generic;

namespace PhysicsRails2D;

public partial class RailNode : Node2D
{

	[Export]
	public Color RailColor;

	/// <summary>
	/// List af all points that predict movement of this object for certain period of time
	/// </summary>
	/// <returns></returns>
	public RailPointList Items;

	[Export]
	private Vector2 FirstPointSpeed;

	[Export]
	private float FirstPointRotSpeed;

	[Export]
	private Vector2 FirstPointAccel;

	[Export]
	private float FirstPointRotAccel;

	public RailNode() : base()
	{
		Items = new RailPointList();
	}

	/// <summary>
	/// Method for updating node pos according to simulation
	/// </summary>
	public void UpdatePos()
	{
		if (Items != null)
		{
			Position = Items[0].Position;
		}
	}

	public void DrawRail()
	{
		if (Items != null)
		{
			Vector2[] Points = new Vector2[Items.Count];
			for (int i = 0; i < Items.Count; i++)
			{
				Points[i] = Items[i].Position - Items[0].Position;
				Points[i] = Points[i].Rotated(-Rotation);
			}
			if (Points.Length > 1) DrawPolyline(Points, RailColor, 2);
		}
	}

	public override void _EnterTree()
	{
		SetFirstPoint();
		base._EnterTree();
	}

	public override void _ExitTree()
	{
		base._ExitTree();
	}

	public override void _PhysicsProcess(double delta)
	{
		UpdatePos();
		base._PhysicsProcess(delta);
#if DEBUG
		QueueRedraw();
#endif
	}

	public override void _Draw()
	{
		base._Draw();
#if DEBUG
		DrawRail();
#endif
	}

	/// <summary>
	/// Method for setting up first point of this rail
	/// </summary>
	public void SetFirstPoint()
	{
		RailPoint Point = new RailPoint();
		Point.Position = Position;
		Point.Rotation = Rotation;
		Point.Speed = FirstPointSpeed;
		Point.RotSpeed = FirstPointRotSpeed;
		Point.Acceleration = FirstPointAccel;
		Point.RotAccel = FirstPointRotAccel;
		Items.SetFirstPoint(Point);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

}

public interface IAccelerator
{

	/// <summary>
	/// Method for accelerating a specific rail point
	/// </summary>
	/// <param name="railPoint"></param>
	/// <param name="time">параметр для </param>
	public void Accelerate(RailPoint railPoint, double time);

}
