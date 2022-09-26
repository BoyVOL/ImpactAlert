using Godot;
using System.Collections.Generic;

public class Collider:SelfUnloadingNode{

    public struct Collision{
        public float time;

        public Collider collider;

        public bool Physic;

        public Collision(float Time, Collider Collider, bool phys){
            time = Time;
            collider = Collider;
            Physic = phys;
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
    
    Collision ScanForApproaches(Collider collider, RailPointList OwnRail, RailPointList OtherRail){
        if(OwnRail.Count != OtherRail.Count) throw new System.Exception("Rail counts dont match");
        float Minlength = OwnRail[0].Position.DistanceSquaredTo(OtherRail[0].Position);
        float MinTime = 0;
        for (int i = 0; i < OwnRail.Count-1; i++)
        {
        }
        return new Collision(MinTime,collider,false);
    }

    public void ScanForApproaches(Collider collider, bool Phys){
        if(Phys) {
            ScanForApproaches(collider,Parent.PhysRail,collider.Parent.PhysRail);
        } else {
            ScanForApproaches(collider,Parent.PredictionRail,collider.Parent.PredictionRail);
        }
    }

    public void AddCollision(Collision Coll){
        Collisions.Add(Coll);
    }

    public void ClearCollisions(){
        Collisions.Clear();
    }
}