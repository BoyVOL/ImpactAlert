#if TOOLS
using Godot;
using System;

[Tool]
public partial class PhysicsRails2D : EditorPlugin
{
	String GlobalPath = "res://addons/PhysicsRails2D/";
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		AddCustomType("RailNode", "Node2D", GD.Load<Script>(GlobalPath+"Nodes/RailNode/RailNode.cs"),
		GD.Load<Texture2D>(GlobalPath+"Nodes/RailNode//icon.png"));
		GD.Print("PhysicsRails2D loaded");
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveCustomType("RailNode");
		GD.Print("PhysicsRails2D unloaded");
	}
}

#endif