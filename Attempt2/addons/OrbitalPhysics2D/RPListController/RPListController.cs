using Godot;
using System.Collections.Generic;

public class RPListController: PhysicsControlAddon{

    protected List<RailPointList> PhysicRails = new List<RailPointList>();

    protected List<RailPointList> PredictionRails = new List<RailPointList>();

    public override void _EnterTree()
    {
        base._EnterTree();
        Parent.RPLists = this;
    }

    public void Reset(){
        foreach (var list in PhysicRails)
        {
            list.ResetToStart();
        }
    }

    public void UpdatePhysic(float delta){
        foreach (var list in PhysicRails)
        {
            list.AppendPoint(delta);
        }
    }

    public void UpdatePredictionSingle(float delta){
        foreach (var list in PhysicRails)
        {
            list.AppendPoint(delta);
        }
    }

    public void AddPhysic(RailPointList List){
        PhysicRails.Add(List);
    }

    public void AddPrediction(RailPointList List){
        PredictionRails.Add(List);
    }

    public void RemovePhysic(RailPointList List){
        PhysicRails.Remove(List);
    }

    public void RemovePrediction(RailPointList List){
        PredictionRails.Remove(List);
    }
}
