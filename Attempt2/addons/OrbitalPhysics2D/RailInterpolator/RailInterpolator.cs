using Godot;
using System.Collections.Generic;

public class RailInterpolator: Node2D{

    public CustomPhysObject Parent;

    /// <summary>
    /// Time offset im ms
    /// </summary>
    public float Offset = 0;

    public float PhysTime = 0;
    
    /// <summary>
    /// Snaps node pos to interpolated state
    /// </summary>
    public void SwitchToInterState(){
        int PrevPointID = Parent.RailPoints.GetBeforeTime(Offset);
        if(PrevPointID >= Parent.RailPoints.Count()-1){
            Position = Parent.RailPoints[Parent.RailPoints.Count()-1].Position-Parent.Position;
            Rotation = Parent.RailPoints[Parent.RailPoints.Count()-1].Rotation-Parent.Rotation;
        } else {
            int NextPointID = PrevPointID + 1;
            Vector2 InterSpeed = CalcInterpolSpeed(Parent.RailPoints[PrevPointID],Parent.RailPoints[NextPointID]);
            float InterRotSpeed = CalcInterpolRot(Parent.RailPoints[PrevPointID],Parent.RailPoints[NextPointID]);
            float LocalOffset = Offset-Parent.RailPoints[PrevPointID].time;
            Position = (Parent.RailPoints[PrevPointID].Position + LocalOffset*InterSpeed)-Parent.Position;
            Rotation = (Parent.RailPoints[PrevPointID].Rotation + LocalOffset*InterRotSpeed)-Parent.Rotation;
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

    public override void _PhysicsProcess(float delta){
        base._PhysicsProcess(delta);
        PhysTime = delta;
        Offset = 0;
    }

    public override void _Process(float delta){
        base._Process(delta);
        Offset+=delta;
        SwitchToInterState();
    }
        
}