#if TOOLS
using Godot;
using System;

[Tool]
public partial class Transform2DInterpolation : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		GD.Print("Transform2DInterpolation loaded");
		AddCustomType("InterpolatorNode","Node2D",GD.Load<Script>("res://addons/Transform2DInterpolation/Nodes/InterpolatorNode/InterpolatorNode.cs"),
		GD.Load<Texture2D>("res://addons/Transform2DInterpolation/Nodes/InterpolatorNode//icon.png"));
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveCustomType("RailInterpolator");
		GD.Print("Transform2DInterpolation unloaded");
	}
}
#endif
