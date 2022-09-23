using Godot;
using System.Collections.Generic;

public class Collider:SelfUnloadingNode{

    [Export]
    public float Radius = 1;

    public List<Vector2> CollisionPoints = new List<Vector2>();

    [Export]
    public Color RadiusColor;

    [Export]
    public Color CollisionColor;

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.PhysNode.CollContr.Add(this);
        Parent.ColList.Add(this);
    }

    public float GetClosestTime(Collider collider, bool Phys, int id){
        if(Phys){
            return Parent.PhysRail[id].CPA(collider.Parent.PhysRail[id],collider.Parent.PhysRail[id+1].time-collider.Parent.PhysRail[id].time);
        } else {
            return Parent.PredictionRail[id].CPA(collider.Parent.PredictionRail[id],collider.Parent.PredictionRail[id+1].time-collider.Parent.PredictionRail[id].time);
        }
    }

    public Vector2 GetClosestPos(Collider collider, bool Phys, int id){
        float CollTime = GetClosestTime(collider,Phys,id);
        if(Phys){
            return Parent.PhysRail[id].GetInterPos(CollTime,Parent.PhysRail[id+1].time-Parent.PhysRail[id].time);
        } else {
            return Parent.PredictionRail[id].GetInterPos(CollTime,Parent.PredictionRail[id+1].time-Parent.PredictionRail[id].time);
        }
    }
}