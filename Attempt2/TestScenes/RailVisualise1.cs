using Godot;
using System;
using CustomPhysics;

public class RailVisualise1 : Node2D
{

    MainRailArray Test = new MainRailArray(10,1);
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta){
        Test.Update();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
