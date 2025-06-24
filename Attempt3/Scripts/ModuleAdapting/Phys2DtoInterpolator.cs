using Godot;
using System;

namespace PhysicsRails2D;

public partial class RailNode : ITwoTransforms
{
    public InterpolatePosition StartPosition
    {
        get
        {
            return new InterpolatePosition(
            Items[0].Position, Items[0].Rotation);
        }
    }

    public InterpolatePosition EndPosition
    {
        get
        {
            return new InterpolatePosition(
            Items[1].Position, Items[1].Rotation);
        }
    }
}
