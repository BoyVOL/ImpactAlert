using Godot;
using System;
using CustomPhysics;

public class RailVisualise1 : Node2D
{

    MainRailArray Test = new MainRailArray(10,1);

    int MainRailID = 0;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RailPoint Start1 = new RailPoint();
        Start1.Position = new Vector2(14,10);
        Start1.Acceleration = new Vector2(-1,0);
        MainRailID = Test.Edit.AddRail(Start1);
        Sprite ShipSprite = GetChild<Sprite>(0);
    }

    public override void _PhysicsProcess(float delta){
        GD.Print("PhysUpdate");
        Test.Update();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
