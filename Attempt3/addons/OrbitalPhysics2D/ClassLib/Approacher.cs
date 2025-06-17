using Godot;
using System.Collections.Generic;

/// <summary>
/// Class that searches for all approaches to other Approachers
/// </summary>
public partial class Approacher:SelfUnloadingNode{

	public struct Approach{
		public float time;

		public Approacher Approacher;

		public Approach(float Time, Approacher approacher){
			time = Time;
			Approacher = approacher;
		}
	}

	[Export]
	public float Radius = 1;

	public List<Approach> Approaches = new List<Approach>();

	[Export]
	public Color RadiusColor;

	[Export]
	public Color ApproachColor;

	public void AddApproach(Approach Coll){
		Approaches.Add(Coll);
	}

	public void ClearApproaches(){
		Approaches.Clear();
	}
}
