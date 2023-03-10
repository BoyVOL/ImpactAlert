using Godot;
using System;
using System.Collections.Generic;

public partial class PredictRailNode: PhysRailNode{
	
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

	public override void _Draw()
	{
		base._Draw();
		#if DEBUG
		DrawPred();
		#endif
	}
}