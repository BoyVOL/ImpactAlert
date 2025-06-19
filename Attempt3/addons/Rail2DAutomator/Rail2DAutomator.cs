#if TOOLS
using Godot;
using System;

[Tool]
public partial class Rail2DAutomator : EditorPlugin
{
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here.
		GD.Print("Rail2DAutomator loaded");
		AddCustomType("RailAutomator", "Node", GD.Load<Script>("res://addons/Rail2DAutomator/Nodes/RailAutomator/RailAutomator.cs"),
		GD.Load<Texture2D>("res://addons/Rail2DAutomator/Nodes/RailAutomator//icon.png"));
		AddCustomType("AutoRailNode", "Node2D", GD.Load<Script>("res://addons/Rail2DAutomator/Nodes/AutoRailNode/AutoRailNode.cs"),
		GD.Load<Texture2D>("res://addons/Rail2DAutomator/Nodes/AutoRailNode//icon.png"));
        AddAutoloadSingleton("RailAutomator", "res://addons/Rail2DAutomator/Scenes/AutomatorSingleton.tscn");
		// Initialization of the plugin goes here.
	}

	public override void _ExitTree()
	{
        RemoveAutoloadSingleton("RailAutomator");
		RemoveCustomType("RailAutomator");
		RemoveCustomType("AutoRailNode");
		GD.Print("Rail2DAutomator unloaded");
		// Clean-up of the plugin goes here.
	}
}
#endif
