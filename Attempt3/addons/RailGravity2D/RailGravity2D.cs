#if TOOLS
using Godot;
using System;

[Tool]
public partial class RailGravity2D : EditorPlugin
{
	String GlobalPath = "res://addons/RailGravity2D/";
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		AddCustomType("GravNode", "Node", GD.Load<Script>(GlobalPath+"Nodes/GravNode/GravNode.cs"),
		GD.Load<Texture2D>(GlobalPath+"Nodes/GravNode//icon.png"));
		GD.Print("gravitator2d loaded");
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		GD.Print("gravitator2d unloaded");
	}
}
#endif
