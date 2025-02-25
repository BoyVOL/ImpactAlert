using Godot;
using System.Collections.Generic;

public partial class RailInterpolator: Node2D{

    public PhysRailNode Parent;

    /// <summary>
    /// Time offset im ms
    /// </summary>
    public float Offset = 0;

    public float PhysTime = 0;
    
    /// <summary>
    /// Snaps node pos to interpolated state
    /// </summary>
    public void SwitchToInterState(){
        int PrevPointID = Parent.PhysRail.GetBeforeTime(Offset);
        if(PrevPointID >= Parent.PhysRail.Count-1){
            Position = Parent.PhysRail[Parent.PhysRail.Count-1].Position-Parent.Position;
            Rotation = Parent.PhysRail[Parent.PhysRail.Count-1].Rotation-Parent.Rotation;
        } else {
            int NextPointID = PrevPointID + 1;
            float LocalOffset = Offset-Parent.PhysRail[PrevPointID].time;
            float TimeFrame = Parent.PhysRail[PrevPointID+1].time-Parent.PhysRail[PrevPointID].time;
            Position = Parent.PhysRail[PrevPointID].GetInterPos(LocalOffset,TimeFrame)-Parent.Position;
            Rotation = Parent.PhysRail[PrevPointID].GetInterRot(LocalOffset,TimeFrame)-Parent.Rotation;
        }

    }

    /// <summary>
    /// Method for calculating interpolation speed between two points.
    /// </summary>
    /// <param name="Point1">Start point of interpolation</param>
    /// <param name="Point2">End point of interpolation</param>
    /// <returns></returns>
    public Vector2 CalcInterpolSpeed(RailPoint Point1, RailPoint Point2){
        float time = Point2.time - Point1.time;
        return (Point2.Position - Point1.Position)/time;
    }

    /// <summary>
    /// Method for calculating interpolation rotation speed between two points.
    /// </summary>
    /// <param name="Point1">Start point of interpolation</param>
    /// <param name="Point2">End point of interpolation</param>
    /// <returns></returns>
    public float CalcInterpolRot(RailPoint Point1, RailPoint Point2){
        float time = Point2.time - Point1.time;
        return (Point2.Rotation - Point1.Rotation)/time;
    }

    public override void _EnterTree(){
        base._EnterTree();
        Parent = GetParent<CustomPhysObject>();
    }

    public override void _ExitTree(){
        base._ExitTree();
    }

    public override void _PhysicsProcess(double delta){
        base._PhysicsProcess(delta);
        PhysTime = (float)delta;
        Offset = 0;
    }

    public override void _Process(double delta){
        base._Process(delta);
        Offset+=(float)delta;
        SwitchToInterState();
    }
        
}
