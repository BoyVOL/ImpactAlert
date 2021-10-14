using Godot;
using System;

public class TestCamera : Camera2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _UnhandledInput(InputEvent @event){
    if (@event is InputEventMouseButton){
        InputEventMouseButton emb = (InputEventMouseButton)@event;
        if (emb.IsPressed()){
            if (emb.ButtonIndex == (int)ButtonList.WheelUp){
                this.Zoom -= Vector2.One;
            }
            if (emb.ButtonIndex == (int)ButtonList.WheelDown){
                this.Zoom += Vector2.One;
                GD.Print(emb.AsText());
            }
        }
    }
}
  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }
}
