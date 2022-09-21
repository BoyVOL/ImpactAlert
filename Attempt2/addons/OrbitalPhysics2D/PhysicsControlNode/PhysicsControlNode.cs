using Godot;
using System.Collections.Generic;
using System;

/// <summary>
/// Класс, отвечающий за обработку физических взаимодействий 
/// </summary>
public class PhysicsControlNode: Node{

    private List<CustomPhysObject> Objects = new List<CustomPhysObject>();

    public PhysInfController InfContr;

    public CollisionController CollContr;

    public CollListController PhysRail;

    public PredictListController PredictRail;

    [Export]
    public float MaxPredictionRange = 10;

    public List<CustomPhysObject> NotLoaded = new List<CustomPhysObject>();

    public PhysicsControlNode():base(){
        InfContr = new PhysInfController(this);
        CollContr = new CollisionController(this);
        PhysRail = new CollListController(this,InfContr,CollContr);
        PredictRail = new PredictListController(this,InfContr);
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
    }

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public void PhysRailUpdate(float delta){
        PhysRail.Reset();
        PhysRail.UpdateAccel(0,true,delta);
        PhysRail.AppendPoint(delta,1);
    }

    public int CalcStepCount(float delta){
        return (int)(MaxPredictionRange/delta);
    }

    public void PredictRailUpdate(float delta){
        PredictRail.LoadFromPhys();
        for (int i = 0; i < CalcStepCount(delta); i++)
        {
            PredictRail.UpdateAccel(i,false,delta);
            PredictRail.AppendPoint(delta,i+1);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        LoadAll();
        PhysRailUpdate(delta);
        PredictRailUpdate(delta);
    }

}
