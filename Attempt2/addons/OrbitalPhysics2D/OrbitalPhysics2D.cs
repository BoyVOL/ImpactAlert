using Godot;

[Tool]
public class OrbitalPhysics2D : EditorPlugin
{
    public string LibDir = "res://addons/OrbitalPhysics2D/";

    public void AddCustomType(string Type, string parent){
        AddCustomType(Type,parent,GD.Load<Script>(LibDir+Type+"/"+Type+".cs"),
        GD.Load<Texture>(LibDir+"/"+Type+"/icon.png"));
    }

    public void AddAutoloadSingleton(string Name){
        AddAutoloadSingleton(Name,LibDir+"/AutoloadScenes/"+Name+".tscn");
    }

    public override void _EnterTree(){
        base._EnterTree();
        GD.Print("Plugin ready");
        AddCustomType("PhysicsControlNode","Node");
        AddCustomType("GravityInfluencer","Node2D");
        AddCustomType("GravityObject","Node2D");
        AddCustomType("RailInterpolator","Node2D");
        AddAutoloadSingleton("Autoload");
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        RemoveCustomType("GravityInfluencer");
        RemoveCustomType("GravityObject");
        RemoveCustomType("PhysicsControlNode");
        RemoveCustomType("RailInterpolator");
        RemoveAutoloadSingleton("Autoload");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }
}
