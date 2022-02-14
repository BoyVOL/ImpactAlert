using Godot;
using System;
using CustomPhysics;

public class RailArrayTest : Node2D
{

    RailArray Test = new RailArray(10,1);
    
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("RailArrayTest");
        RailPoint Start1 = new RailPoint();
        Start1.Position = new Vector2(10,10);
        Start1.Acceleration = new Vector2(1,0);
        Guid ID = Test.AddRail(Start1);
        GD.Print(Test.StringifyRail(ID));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
