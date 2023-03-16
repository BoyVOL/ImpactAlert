using Godot;
using System;
using System.Collections.Generic;

public partial class InfluencedRailNode: PredictRailNode{

	public List<Influencer> InfList = new List<Influencer>();

	public void DrawInfluencers(){
		foreach (var inf in InfList)
		{
			DrawArc(Vector2.Zero,inf.InfRad,0,(float)Math.PI*2,100,inf.DebugColor);
		}
	}

	public override void _Draw()
	{
		base._Draw();
		#if DEBUG
		DrawInfluencers();
		#endif
	}
}