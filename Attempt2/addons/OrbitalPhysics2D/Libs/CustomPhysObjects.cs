using Godot;
using System;
using System.Collections.Generic;

public class CustomPhysObject: Node2D{

    /// <summary>
    /// Ref to controlling object
    /// </summary>
    protected PhysicsControlNode PhysNode = null;

    /// <summary>
    /// List of all simulation points of this object
    /// </summary>
    /// <typeparam name="RailPoint"></typeparam>
    /// <returns></returns>
    public RailPointList RailPoints = new RailPointList();

    [Export]
    public Vector2 FirstPointSpeed = Vector2.Zero;

    [Export]
    public float FirstPointRotSpeed = 0;

    [Export]
    public Vector2 FirstPointAccel = Vector2.Zero;

    [Export]
    public float FirstPointRotAccel = 0;

    /// <summary>
    /// Method for setting up first point of this rail
    /// </summary>
    public void SetFirstPoint(){
        RailPoint Point = new RailPoint();
        Point.Position = Position;
        Point.Rotation = Rotation;
        Point.Speed = FirstPointSpeed;
        Point.RotSpeed = FirstPointRotSpeed;
        Point.Acceleration = FirstPointAccel;
        Point.RotAccel = FirstPointRotAccel;
        RailPoints.SetFirstPoint(Point);
    }

    /// <summary>
    /// Method for updating node pos according to simulation
    /// </summary>
    public void UpdatePos(){
        Position = RailPoints[0].Position;
        Rotation = RailPoints[0].Rotation;
    }

    public void DrawPath(){
        Vector2[] Points = new Vector2[RailPoints.Count()];
        for (int i = 0; i < RailPoints.Count(); i++)
        {
            Points[i] = RailPoints[i].Position-RailPoints[0].Position;
        }
        DrawMultiline(Points,Colors.Green,10);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        SetFirstPoint();
        PhysNode = GetNode<PhysicsControlNode>("/root/Autoload/PhysicsControlNode");
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        Update();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        UpdatePos();
    }

    public override void _Draw()
    {
        base._Draw();
        DrawPath();
    }
}
