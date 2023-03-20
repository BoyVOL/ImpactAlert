using Godot;
using System.Collections.Generic;
using System;

/// <summary>
/// Класс, отвечающий за обработку физических взаимодействий 
/// </summary>
public partial class PhysicsControlNode: Node{

    private List<CustomPhysObject> Objects = new List<CustomPhysObject>();

    public PhysInfController InfContr;

    public ColliderController CollContr;

    public CollRPListController PhysRail;

    public PredRPListController PredictRail;

    public PredRPListController FramePredictRail;

    [Export]
    public float MaxPredictionRange = 10;

    public List<CustomPhysObject> NotLoaded = new List<CustomPhysObject>();

    public PhysicsControlNode():base(){
        InfContr = new PhysInfController(this);
        CollContr = new ColliderController(this);
        PhysRail = new CollRPListController(this,InfContr,CollContr);
        PredictRail = new PredRPListController(this,InfContr);
        FramePredictRail = new PredRPListController(this,InfContr);
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

    public void PhysRailUpdate(double delta){
        PhysRail.Reset();
        PhysRail.UpdateAccel(0,true,(float)delta);
        PhysRail.AppendPoint((float)delta,1);
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

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess((float)delta);
        LoadAll();
        PhysRailUpdate((float)delta);
        PredictRailUpdate((float)delta);
        CollContr.CollisionTest();
    }

}
