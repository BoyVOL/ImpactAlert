using Godot;
using System.Collections.Generic;

public class RailInterpolator: Node2D{

    public CustomPhysObject Parent;

    /// <summary>
    /// Time offset im ms
    /// </summary>
    public float Offset = 0;

    public float PhysTime = 0;
    
    public void SwitchToInterState(){
        int PointID = Parent.RailPoints.GetBeforeTime(Offset);
        float T = Offset - Parent.RailPoints[PointID].time;
        if(T > PhysTime) T = PhysTime;
        Position = Parent.RailPoints[PointID].GetInterPos(T)-Parent.Position;
        Rotation = Parent.RailPoints[PointID].GetInterRot(T)-Parent.Rotation;
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
