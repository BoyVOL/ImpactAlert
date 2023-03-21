using Godot;
using System;
using System.Collections.Generic;

public partial class CollisionRailNode: PredictRailNode{

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

	public override void _Draw()
	{
		base._Draw();
		#if DEBUG
		DrawCollider();
		DrawCollisions();
		#endif
	}
}