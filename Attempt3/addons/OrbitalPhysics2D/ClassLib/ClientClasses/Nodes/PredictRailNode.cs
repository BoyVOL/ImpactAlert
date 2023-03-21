using Godot;
using System;
using System.Collections.Generic;

public partial class PredictRailNode: PhysRailNode{

	public Collider Collider = null;

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
	
	[Export]
	public Color PredictionColor;

	/// <summary>
	/// List af all points that predict movement of this object for certain period of time
	/// </summary>
	/// <returns></returns>
	public RailPointList PredictionRail;

    public PredictRailNode():base(){
		PredictionRail = new RailPointList(this);
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

	public override void _EnterTree(){
		base._EnterTree();
		PhysNode = GetNode<PhysicsControlNode>("/root/Autoload/PhysicsControlNode");
	}

	public override void LoadObject(){
		base.LoadObject();
		PhysNode.PredictRail.Add(PredictionRail);
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		#if DEBUG
		QueueRedraw();
		#endif
	}

	public override void _Draw()
	{
		base._Draw();
		#if DEBUG
		DrawCollider();
		DrawCollisions();
		DrawPred();
		#endif
	}
}