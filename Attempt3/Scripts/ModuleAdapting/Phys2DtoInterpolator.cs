using Godot;
using System;

public partial class CustomPhysObject : ITwoTransforms
{
    public InterpolatePosition StartPosition
    {
        get { return new InterpolatePosition(
            PhysRail[0].Position, PhysRail[0].Rotation); }
    }
    
    public InterpolatePosition EndPosition
    {
        get { return new InterpolatePosition(
            PhysRail[1].Position, PhysRail[1].Rotation); }
    }
}
