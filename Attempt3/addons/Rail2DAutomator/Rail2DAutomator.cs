#if TOOLS
using Godot;
using System;

[Tool]
public partial class Rail2DAutomator : EditorPlugin
{
	String GlobalPath;
	public override void _EnterTree()
	{
		// Initialization of the plugin goes here
		GlobalPath = ((Resource)GetScript()).ResourcePath.GetBaseDir();
		AddCustomType("RailAutomator", "Node", GD.Load<Script>(GlobalPath+"/Nodes/RailAutomator/RailAutomator.cs"),
		GD.Load<Texture2D>(GlobalPath+"/Nodes/RailAutomator//icon.png"));
		AddCustomType("AutoRailNode", "Node2D", GD.Load<Script>(GlobalPath+"/Nodes/AutoRailNode/AutoRailNode.cs"),
		GD.Load<Texture2D>(GlobalPath+"/Nodes/AutoRailNode//icon.png"));
        AddAutoloadSingleton("RailAutomator", GlobalPath+"/Scenes/AutomatorSingleton.tscn");
		GD.Print("Rail2DAutomator loaded");
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
