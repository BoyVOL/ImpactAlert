using Godot;
using System;
using RailSystem;
using ForceProjection;

public class TestObjectRes : Node2D
{
    TestScene Scene;

    GlobalPhysUpdater Updater;

    Line2D DebugPath;

    InfluencedRail Rail = new InfluencedRail();

    ForceFieldInfluencer Influencer = new ForceFieldInfluencer();

    GravityRailProjector Projector;

    TestCollider Collider = new TestCollider();

    RailFollower Follower;

    void InitLine(){
        DebugPath = GetNode<Line2D>("LinePath");
        for (int i = 0; i < 100; i++)
        {
            DebugPath.AddPoint(new Vector2(0,0));
        }
        this.RemoveChild(DebugPath);
        Scene.AddChild(DebugPath);
    }

    void updateLine(){
        for (int i = 0; i < Rail.GetCount(); i++)
        {
            DebugPath.SetPointPosition(i,Rail.GetPoint(i).Position);
        }
    }

    void RailSetup(
        int ArraySize = 100, float posRange = 10000, 
        float SpeedRange = 100, float AccelRange = 100){
            

            Random Rnd = new Random();

            Vector2 newPos = new Vector2((float)Rnd.NextDouble()*posRange*2-posRange,(float)Rnd.NextDouble()*posRange*2-posRange);
            //Vector2 newPos = new Vector2(20,20);
            Vector2 newSpeed = new Vector2((float)(Rnd.NextDouble()*SpeedRange*2-SpeedRange),(float)(Rnd.NextDouble()*SpeedRange*2-SpeedRange));
            Vector2 newAccel = new Vector2((float)Rnd.NextDouble()*AccelRange*2-AccelRange,(float)Rnd.NextDouble()*AccelRange*2-AccelRange);

            Rail.AddInfluencer(Influencer);
            Rail.SetFirstPoint(new AccelPoint(newPos,(float)(Rnd.NextDouble()*Math.PI*2),newSpeed,newAccel,(float)(Rnd.NextDouble()*2-1)));
            Updater.RailController.AddRail(Rail);
            Follower = Updater.RailController.GetRailFollower(Rail);
            Follower.Shift = Updater.Watcher.Shift;
    } 

    void ForceSetup(){
        Projector = new GravityRailProjector(Rail,100000);
        Updater.ForceHandler.AddProjector(Projector);
        Influencer.Handler = Updater.ForceHandler;
        Influencer.Params.Exclude = new ForceProjector[1];
        Influencer.Params.Exclude[0] = Projector;
    }

    void CollisionSetup(float radius = 100){
        Collider.Current = Rail;
        Collider.Radius = radius;
        Updater.Collider.AddCollider(Collider);
    }



    // Called when the node enters the scene tree for the first time.

    public override void _Ready()
    {
        Scene = GetParent<TestScene>();
        Updater = Scene.Updater;
        ForceSetup();
        CollisionSetup();
        RailSetup();
        InitLine();
    }

    public override void _Process(float delta)
    {
        updateLine();
        Follower.Shift += delta;
        GlobalPosition = Follower.GetInterpolation().Position;
        GlobalRotation = Follower.GetInterpolation().Rotation;
    }
}
