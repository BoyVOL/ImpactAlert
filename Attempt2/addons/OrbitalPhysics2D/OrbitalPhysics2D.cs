using Godot;
using System;

[Tool]
public class OrbitalPhysics2D : EditorPlugin
{
    string LibDir = "res://addons/OrbitalPhysics2D/";

    public void AddCustomType(string Type, string parent){
        AddCustomType(Type,parent,GD.Load<Script>(LibDir+Type+"/"+Type+".cs"),
        GD.Load<Texture>(LibDir+"/"+Type+"/icon.png"));
    }

    public override void _EnterTree(){
        base._EnterTree();
        GD.Print("Plugin ready");
        AddCustomType("PhysicsControlNode","Node");
        AddCustomType("GravityInfluencer","Node2D");
        AddCustomType("GravityObject","Node2D");
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        RemoveCustomType("PhysicsControlNode");
        RemoveCustomType("GravityInfluencer");
        RemoveCustomType("GravityObject");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }
}
