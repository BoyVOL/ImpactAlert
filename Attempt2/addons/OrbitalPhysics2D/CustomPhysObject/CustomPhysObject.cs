using Godot;
using System;
using System.Collections.Generic;

public class CustomPhysObject: Node2D{

    /// <summary>
    /// Ref to controlling object
    /// </summary>
    protected PhysicsControlNode PhysNode = null;
    
    [Export]
    public Color DebugColor;

    public List<Influencer> InfList = new List<Influencer>();

    /// <summary>
    /// List of all simulation points of this object
    /// </summary>
    /// <typeparam name="PhysRail"></typeparam>
    /// <returns></returns>
    public RailPointList PhysRail = new RailPointList();

    /// <summary>
    /// List af all points that predict movement of this object for certain period of time
    /// </summary>
    /// <returns></returns>
    public RailPointList PredictionRail = new RailPointList();

    [Export]
    private Vector2 FirstPointSpeed = Vector2.Zero;

    [Export]
    private float FirstPointRotSpeed = 0;

    [Export]
    private Vector2 FirstPointAccel = Vector2.Zero;

    [Export]
    private float FirstPointRotAccel = 0;

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
        PhysRail.SetFirstPoint(Point);
    }

    /// <summary>
    /// Method for updating node pos according to simulation
    /// </summary>
    public void UpdatePos(){
        Position = PhysRail[0].Position;
    }

    public void DrawPath(){
            Vector2[] Points = new Vector2[PhysRail.Count()];
            for (int i = 0; i < PhysRail.Count(); i++)
            {
                Points[i] = PhysRail[i].Position-PhysRail[0].Position;
            }
            DrawPolyline(Points,DebugColor,2);
    }

    public void DrawInfluencer(){
        for (int i = 0; i < InfList.Count; i++)
        {
            DrawArc(Vector2.Zero,InfList[i].InfRad,0,(float)Math.PI*2,100,InfList[i].DebugColor);
        }
    }

    public void LoadObject(){
        PhysNode.PhysRail.Add(PhysRail);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        SetFirstPoint();
        PhysNode = GetNode<PhysicsControlNode>("/root/Autoload/PhysicsControlNode");
        PhysNode.Add(this);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        PhysNode.Remove(this);
        PhysNode.PhysRail.Remove(PhysRail);
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        UpdatePos();
        #if DEBUG
        Update();
        #endif
    }

    public override void _Draw()
    {
        base._Draw();
        #if DEBUG
        DrawPath();
        DrawInfluencer();
        #endif
    }
}