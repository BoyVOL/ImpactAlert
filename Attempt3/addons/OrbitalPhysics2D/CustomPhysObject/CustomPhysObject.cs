using Godot;
using System;
using System.Collections.Generic;

public partial class CustomPhysObject: Node2D{

	/// <summary>
	/// Ref to controlling object
	/// </summary>
	public PhysicsControlNode PhysNode = null;

	public Collider Collider = null;
	
	[Export]
	public Color PhysRailColor;
	
	[Export]
	public Color PredictionColor;

	public List<Influencer> InfList = new List<Influencer>();

	/// <summary>
	/// List of all simulation points of this object
	/// </summary>
	/// <typeparam name="PhysRail"></typeparam>
	/// <returns></returns>
	public RailPointList PhysRail;

	/// <summary>
	/// List af all points that predict movement of this object for certain period of time
	/// </summary>
	/// <returns></returns>
	public RailPointList PredictionRail;

	[Export]
	private Vector2 FirstPointSpeed;

	[Export]
	private float FirstPointRotSpeed;

	[Export]
	private Vector2 FirstPointAccel;

	[Export]
	private float FirstPointRotAccel;

	[Export]
	public float mass = 1;

	public CustomPhysObject():base(){
		PhysRail = new RailPointList(this);
		PredictionRail = new RailPointList(this);
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

	/// <summary>
	/// Method for updating node pos according to simulation
	/// </summary>
	public void UpdatePos(){
		Position = PhysRail[0].Position;
		Rotation = PhysRail[0].Rotation;
	}

	public void DrawPhys(){
			Vector2[] Points = new Vector2[PhysRail.Count];
			for (int i = 0; i < PhysRail.Count; i++)
			{
				Points[i] = PhysRail[i].Position-PhysRail[0].Position;
			}
			if(Points.Length > 1) DrawPolyline(Points,PhysRailColor,2);
	}

	public void DrawPred(){
			Vector2[] Points = new Vector2[PredictionRail.Count];
			for (int i = 0; i < PredictionRail.Count; i++)
			{
				Points[i] = PredictionRail[i].Position-PredictionRail[0].Position;
				Points[i] = Points[i].Rotated(-Rotation);
			}
			if(Points.Length > 1) DrawPolyline(Points,PredictionColor,2);
	}

	public void DrawInfluencers(){
		foreach (var inf in InfList)
		{
			DrawArc(Vector2.Zero,inf.InfRad,0,(float)Math.PI*2,100,inf.DebugColor);
		}
	}

	public void DrawCollider(){
		if(Collider != null){
			DrawArc(Vector2.Zero,Collider.Radius,0,(float)Math.PI*2,100,Collider.RadiusColor);
		}
	}    

	public void DrawCollisions(){
		if(Collider != null){
			foreach (var collision in Collider.Collisions)
			{
				Vector2 Pos;
				Pos = PredictionRail.InterpolatePos(collision.time);
				DrawCircle(Pos-Position,3,collision.Rangefinder.CollisionColor);
			}
		}
	}

	public void LoadObject(){
		PhysNode.PhysRail.Add(PhysRail);
		PhysNode.PredictRail.Add(PredictionRail);
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		SetFirstPoint();
		PhysNode = GetNode<PhysicsControlNode>("/root/Autoload/PhysicsControlNode");
		PhysNode.Add(this);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		PhysNode.Remove(this);
		PhysNode.PhysRail.Remove(PhysRail);
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		UpdatePos();
		#if DEBUG
		QueueRedraw();
		#endif
	}

	public override void _Draw()
	{
		base._Draw();
		#if DEBUG
		DrawPred();
		DrawPhys();
		DrawInfluencers();
		DrawCollider();
		DrawCollisions();
		#endif
	}
}
