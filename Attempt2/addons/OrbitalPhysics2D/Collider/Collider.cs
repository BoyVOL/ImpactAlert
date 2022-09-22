using Godot;

public class Collider:SelfUnloadingNode{

    [Export]
    public float Radius = 1;

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.PhysNode.CollContr.Add(this);
    }

    public float GetClosestTime(Collider collider, bool Phys, int id){
        if(Phys){
            return Parent.PhysRail[id].CPA(collider.Parent.PhysRail[id],collider.Parent.PhysRail[id+1].time-collider.Parent.PhysRail[id].time);
        } else {
            return Parent.PredictionRail[id].CPA(collider.Parent.PredictionRail[id],collider.Parent.PredictionRail[id+1].time-collider.Parent.PredictionRail[id].time);
        }
    }
}