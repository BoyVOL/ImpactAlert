#if TOOLS
using Godot;
using System;

[Tool]
public partial class Transform2DInterpolation : EditorPlugin
{
	String GlobalPath = "res://addons/Transform2DInterpolation/";
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		AddCustomType("InterpolatorNode", "Node2D", GD.Load<Script>(GlobalPath+"Nodes/InterpolatorNode/InterpolatorNode.cs"),
		GD.Load<Texture2D>(GlobalPath+"Nodes/InterpolatorNode//icon.png"));
		GD.Print("Transform2DInterpolation loaded");
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveCustomType("RailInterpolator");
		GD.Print("Transform2DInterpolation unloaded");
	}
}
#endif
