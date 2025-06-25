using Godot;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PhysRails2D;

public partial class RailNode : Node2D
{

	[Export]
	public Color RailColor;

	/// <summary>
	/// Поле для связывания нод между собой
	/// </summary>
	[Export]
	public Node[] ExportInfluencers = null;

	public List<IINfluencer> Influencers = new List<IINfluencer>();

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

	/// <summary>
	/// Method for modifing a specific rail with all the attached influencers
	/// </summary>
	/// <param name="point"></param>
	/// <param name="step"></param>
	public void ModifyWithAll(RailPoint point, double step)
	{
		foreach (var item in Influencers)
		{
			item.Modify(point, step);
		}
	}

	/// <summary>
	/// Simulate with all linked influence applied
	/// </summary>
	/// <param name="step"></param>
	public void Simulate(double step)
	{
		Items.Simulate((float)step);
		ModifyWithAll(Items.Last(), step);
	}

	public override void _EnterTree()
	{
		SetFirstPoint();
		base._EnterTree();
		if (ExportInfluencers != null)
		{
			foreach(var item in ExportInfluencers)
			Influencers.Add((IINfluencer)item);
		}
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

public interface IINfluencer
{

	/// <summary>
	/// Method for accelerating a specific rail point
	/// </summary>
	/// <param name="railPoint"></param>
	/// <param name="time">параметр для </param>
	public void Modify(RailPoint railPoint, double time);

}
