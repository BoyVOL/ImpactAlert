#if TOOLS
using Godot;

[Tool]
public partial class OrbitalPhysics2D : EditorPlugin
{
	public string LibDir = "res://addons/OrbitalPhysics2D/";

	public void AddCustomType(string Type, string parent){
		AddCustomType(Type,parent,GD.Load<Script>(LibDir+Type+"/"+Type+".cs"),
		GD.Load<Texture2D>(LibDir+"/"+Type+"/icon.png"));
	}

	public void AddAutoloadSingleton(string Name){
		AddAutoloadSingleton(Name,LibDir+"/AutoloadScenes/"+Name+".tscn");
	}

	public override void _EnterTree(){
		base._EnterTree();
		GD.Print("Plugin loaded");
		AddCustomType("PhysicsControlNode","Node");
		AddCustomType("GravityInfluencer","Node");
		AddCustomType("Collider","Node");
		AddCustomType("CollisionPredictor","Node");
		AddCustomType("CustomPhysObject","Node2D");
		AddCustomType("RailInterpolator","Node2D");
		AddCustomType("PredictionRailController","Node2D");
		AddAutoloadSingleton("Autoload");
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		RemoveCustomType("GravityInfluencer");
		RemoveCustomType("CustomPhysObject");
		RemoveCustomType("RailInterpolator");
		RemoveCustomType("Collider");
		RemoveCustomType("CollisionPredictor");
		RemoveCustomType("PhysicsControlNode");
		RemoveCustomType("PredictionRailController");
		RemoveAutoloadSingleton("Autoload");
		GD.Print("Plugin unloaded");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
#endif
