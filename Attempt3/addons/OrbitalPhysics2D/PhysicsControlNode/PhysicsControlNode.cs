using Godot;
using System.Collections.Generic;
using System;

/// <summary>
/// Класс, отвечающий за обработку физических взаимодействий 
/// </summary>
public partial class PhysicsControlNode: Node{

    private List<CustomPhysObject> Objects = new List<CustomPhysObject>();

    public RPListController PhysRail;

    public RPListController PredictRail;

    public RPListController FramePredictRail;

    [Export]
    public float MaxPredictionRange = 10;

    public List<CustomPhysObject> NotLoaded = new List<CustomPhysObject>();

    public PhysicsControlNode():base(){
        PhysRail = new RPListController(this);
        PredictRail = new RPListController(this);
        FramePredictRail = new RPListController(this);
    }

    public void Add(CustomPhysObject Object){
        NotLoaded.Add(Object);
    }

    public void LoadAll(){
        foreach (CustomPhysObject item in NotLoaded)
        {
            Objects.Add(item);
            item.LoadObject();
        }
        NotLoaded.Clear();
    }
    
    public void Remove(CustomPhysObject Object){
        Objects.Remove(Object);
        if(Object.PhysRail != null) PhysRail.Remove(Object.PhysRail);
        if(Object.PredictionRail != null) PredictRail.Remove(Object.PredictionRail);
    }

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public void PhysRailUpdate(double delta){
        PhysRail.Reset();
        PhysRail.UpdateAccel(0,(float)delta);
        PhysRail.AppendPoint((float)delta,1);
    }

    public int CalcStepCount(float delta){
        return (int)(MaxPredictionRange/delta);
    }

    public void PredictRailUpdate(float delta){
        PredictRail.LoadFromPhys();
        for (int i = 0; i < CalcStepCount(delta); i++)
        {
            PredictRail.UpdateAccel(i,delta);
            PredictRail.AppendPoint(delta,i+1);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess((float)delta);
        LoadAll();
        PhysRailUpdate((float)delta);
        PredictRailUpdate((float)delta);
    }

}
