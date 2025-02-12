using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// A class that represents entity able to move with orbital physics
/// </summary>
public partial class PhysRailNode: Node2D{
	
	[Export]
	public Color PredictionColor;

	[Export]
	public float mass = 1;

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
	/// List of all simulation point lists of this object
	/// </summary>
	/// <typeparam name="PhysRail"></typeparam>
	/// <returns></returns>
	public RailPointList PhysRail;

	/// <summary>
	/// List of all approachers
	/// </summary>
	public Approacher Approacher = null;

	public PhysRailNode():base(){
	}

	/// <summary>
	/// Method for updating node pos according to simulation
	/// </summary>
	public void UpdatePos(){
		if(PhysRail!=null){	
			Position = PhysRail[0].Position;
		}
	}

	public void DrawPhys(){
		if(PhysRail != null){
			Vector2[] Points = new Vector2[PhysRail.Count];
			for (int i = 0; i < PhysRail.Count; i++)
			{
				Points[i] = PhysRail[i].Position-PhysRail[0].Position;
			}
			if(Points.Length > 1) DrawPolyline(Points,PhysRailColor,2);
		}
	}

	public void DrawApproaches(){
		if(Approacher != null){
			foreach (var approach in Approacher.Approaches)
			{
				Vector2 Pos;
				Pos = PredictionRail.InterpolatePos(approach.time);
				DrawCircle(Pos-Position,3,approach.Approacher.ApproachColor);
			}
		}
	}

	public void DrawApproacher(){
		if(Approacher != null){
			DrawArc(Vector2.Zero,Approacher.Radius,0,(float)Math.PI*2,100,Approacher.RadiusColor);
		}
	}

	public void DrawPred(){
		if(PredictionRail != null){
			Vector2[] Points = new Vector2[PredictionRail.Count];
			for (int i = 0; i < PredictionRail.Count; i++)
			{
				Points[i] = PredictionRail[i].Position-PredictionRail[0].Position;
				Points[i] = Points[i].Rotated(-Rotation);
			}
			if(Points.Length > 1) DrawPolyline(Points,PredictionColor,2);
		}
	}

	public void DrawInfluencers(){
		if(PhysRail!=null){	
			foreach (var inf in PhysRail.Influencers)
			{
				DrawArc(Vector2.Zero,inf.InfRad,0,(float)Math.PI*2,100,inf.DebugColor);
			}
		}
		if(PredictionRail != null){
			foreach (var inf in PredictionRail.Influencers)
			{
				DrawArc(Vector2.Zero,inf.InfRad,0,(float)Math.PI*2,100,inf.DebugColor);
			}
		}
	}

	public override void _EnterTree(){
		base._EnterTree();
		PhysNode = GetNode<PhysicsControlNode>("/root/Autoload/PhysicsControlNode");
		PhysNode.Add(this);
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		PhysNode.Remove(this);
	}

	public virtual void LoadObject(){
		if(PhysRail != null){	
			PhysNode.PhysRail.Add(PhysRail);
		}
		if(PredictionRail != null){
			PhysNode.PredictRail.Add(PredictionRail);
		}
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
		DrawApproacher();
		DrawApproaches();
		DrawPred();
		DrawInfluencers();
		#endif
	}
}