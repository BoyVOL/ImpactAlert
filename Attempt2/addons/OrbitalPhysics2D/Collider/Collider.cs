using Godot;
using System.Collections.Generic;

public class Collider:SelfUnloadingNode{

    public struct Collision{
        public float time;

        public Collider collider;

        public Collision(float Time, Collider Collider){
            time = Time;
            collider = Collider;
        }
    }

    [Export]
    public float Radius = 1;

    public List<Collision> Collisions = new List<Collision>();

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
    
    Collision ScanForApproaches(Collider collider, RailPointList OwnRail, RailPointList OtherRail,int id){
        float TimeFrame = OwnRail[id+1].time-OwnRail[id].time;
        float calltime = OwnRail[id].CPA(OtherRail[id],TimeFrame)+OwnRail[id].time;
        Collision Result = new Collision(calltime,collider);
        return Result;
    }

    public Collision ScanForApproaches(Collider collider, bool Phys,int id){
        if(Phys) {
            return ScanForApproaches(collider,Parent.PhysRail,collider.Parent.PhysRail,id);
        } else {
            return ScanForApproaches(collider,Parent.PredictionRail,collider.Parent.PredictionRail,id);
        }
    }

    public void ScanRailForApproaches(Collider collider, RailPointList OwnRail, RailPointList OtherRail){
        for (int i = 0; i < OwnRail.Count-1; i++)
        {
            Collisions.Add(ScanForApproaches(collider,OwnRail,OtherRail,i));
        }
    }

    public void ScanRailForApproaches(Collider collider, bool Phys){
        if(Phys) {
            ScanRailForApproaches(collider,Parent.PhysRail,collider.Parent.PhysRail);
        } else {
            ScanRailForApproaches(collider,Parent.PredictionRail,collider.Parent.PredictionRail);
        }
    }

    public void AddCollision(Collision Coll){
        Collisions.Add(Coll);
    }

    public void ClearCollisions(){
        Collisions.Clear();
    }
}