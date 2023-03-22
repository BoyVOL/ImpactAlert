using Godot;
using System;
using System.Collections.Generic;

public partial class PhysRailNode: Node2D{
	
	[Export]
	public Color PredictionColor;

	/// <summary>
	/// List af all points that predict movement of this object for certain period of time
	/// </summary>
	/// <returns></returns>
	public RailPointList PredictionRail;

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

	public Collider Collider = null;

	public PhysRailNode():base(){
		PredictionRail = new RailPointList(this);
		PhysRail = new RailPointList(this);
	}

	/// <summary>
	/// Method for updating node pos according to simulation
	/// </summary>
	public void UpdatePos(){
		Position = PhysRail[0].Position;
	}

	public void DrawPhys(){
			Vector2[] Points = new Vector2[PhysRail.Count];
			for (int i = 0; i < PhysRail.Count; i++)
			{
				Points[i] = PhysRail[i].Position-PhysRail[0].Position;
			}
			if(Points.Length > 1) DrawPolyline(Points,PhysRailColor,2);
	}

	public void DrawCollisions(){
		if(Collider != null){
			foreach (var collision in Collider.Collisions)
			{
				Vector2 Pos;
				Pos = PredictionRail.InterpolatePos(collision.time);
				DrawCircle(Pos-Position,3,collision.Approacher.CollisionColor);
			}
		}
	}

	public void DrawCollider(){
		if(Collider != null){
			DrawArc(Vector2.Zero,Collider.Radius,0,(float)Math.PI*2,100,Collider.RadiusColor);
		}
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
		foreach (var inf in PhysRail.Influencers)
		{
			DrawArc(Vector2.Zero,inf.InfRad,0,(float)Math.PI*2,100,inf.DebugColor);
		}
		foreach (var inf in PredictionRail.Influencers)
		{
			DrawArc(Vector2.Zero,inf.InfRad,0,(float)Math.PI*2,100,inf.DebugColor);
		}
	}

	public override void _EnterTree(){
		base._EnterTree();
		PhysNode = GetNode<PhysicsControlNode>("/root/Autoload/PhysicsControlNode");
	}

	public virtual void LoadObject(){
		PhysNode.PhysRail.Add(PhysRail);
		PhysNode.PredictRail.Add(PredictionRail);
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
		DrawPhys();
		DrawCollider();
		DrawCollisions();
		DrawPred();
		DrawInfluencers();
		#endif
	}
}