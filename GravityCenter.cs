using Godot;
using RailSystem;
using ForceProjection;
using System;

public class GravityCenter : Node2D
{
    Rail Rail = new Rail();

    GravityRailProjector Projector;

    RailFollower Follower;
    
    TestScene Scene;

    GlobalPhysUpdater Updater;

    void RailSetup(){
            Random Rnd = new Random();
            Rail.SetFirstPoint(new KineticPoint(Vector2.Zero,0));
            Updater.RailController.AddRail(Rail);
            Follower = Updater.RailController.GetRailFollower(Rail);
            Follower.Shift = Updater.Watcher.Shift;
    } 

    void ForceSetup(){
        Projector = new GravityRailProjector(Rail,1000000);
        Updater.ForceHandler.AddProjector(Projector);
    }
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Scene = GetParent<TestScene>();
        Updater = Scene.Updater;
        ForceSetup();
        RailSetup();
    }
    public override void _Process(float delta)
    {
        Follower.Shift += delta;
        GlobalPosition = Follower.GetInterpolation().Position;
        GlobalRotation = Follower.GetInterpolation().Rotation;
    }
}
