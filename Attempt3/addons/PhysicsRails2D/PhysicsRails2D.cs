#if TOOLS
using Godot;
using System;

[Tool]
public partial class PhysicsRails2D : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		GD.Print("PhysicsRails2D loaded");
		AddCustomType("RailNode","Node2D",GD.Load<Script>("res://addons/PhysicsRails2D/Nodes/RailNode/RailNode.cs"),
		GD.Load<Texture2D>("res://addons/PhysicsRails2D/Nodes/RailNode//icon.png"));
	}

	public override void _ExitTree()
	{
		// Clean-up of the plugin goes here.
		RemoveCustomType("RailNode");
		GD.Print("PhysicsRails2D unloaded");
	}
}
#endif
