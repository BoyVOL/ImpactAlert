using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Structure for transferring interpolation states to this module
/// </summary>
public struct InterpolatePosition
{
    public InterpolatePosition(Vector2 pos, float rot)
    {
        Position = pos;
        Rotation = rot;
    }

    public Vector2 Position;

    public float Rotation;
}

public partial class InterpolatorNode : Node2D
{
    /// <summary>
    /// Structure for storing interpolation speed inside this class
    /// </summary>
    struct InterpolateSpeed
    {
        public InterpolateSpeed(Vector2 sp, float rot)
        {
            Speed = sp;
            RotSpeed = rot;
        }

        public Vector2 Speed;

        public float RotSpeed;
    }

    public ITwoTransforms Parent = null;

    /// <summary>
    /// Time offset im ms
    /// </summary>
    public float Offset = 0;

    public float PhysDelta = 0;

    /// <summary>
    /// Snaps node pos to interpolated state
    /// </summary>
    public void SwitchToInterState()
    {
        if (Parent != null){    
            InterpolateSpeed Speed = CalcInterpolSpeed(Parent.StartPosition, Parent.EndPosition);
            Position = Parent.StartPosition.Position+Speed.Speed*Offset-((Node2D)Parent).Position;
            Rotation = Parent.StartPosition.Rotation+Speed.RotSpeed*Offset-((Node2D)Parent).Rotation; 
        }
    }

    /// <summary>
    /// Method for calculating interpolation speed between two points.
    /// </summary>
    /// <param name="Point1">Start point of interpolation</param>
    /// <param name="Point2">End point of interpolation</param>
    /// <returns>structure with both vector and angular speed</returns>
    InterpolateSpeed CalcInterpolSpeed(InterpolatePosition Point1, InterpolatePosition Point2)
    {
        return new InterpolateSpeed(
            (Point2.Position - Point1.Position) / PhysDelta,
            (Point2.Rotation - Point1 .Rotation) / PhysDelta); 
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent = GetParent<ITwoTransforms>();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        PhysDelta = (float)delta;
        Offset = 0;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        Offset += (float)delta;
        SwitchToInterState();
    }

}

public interface ITwoTransforms
{
    public InterpolatePosition StartPosition{ get; }
    public InterpolatePosition EndPosition{ get; }

}
