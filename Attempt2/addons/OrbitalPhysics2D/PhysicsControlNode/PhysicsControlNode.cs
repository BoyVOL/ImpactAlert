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

    public InfListController PredictRail;

    public List<CustomPhysObject> NotLoaded = new List<CustomPhysObject>();

    public PhysicsControlNode():base(){
        InfContr = new PhysInfController(this);
        CollContr = new CollisionController(this);
        PhysRail = new CollListController(this,InfContr,CollContr);
        PredictRail = new InfListController(this,InfContr);
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

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        LoadAll();
        PhysRail.Reset();
        PredictRail.Reset();
        PhysRail.UpdateAccel(0,true,delta);
        PhysRail.AppendPoint(delta,1);
        PredictRail.AppendPoint(delta,1);
        /*for (int i = 0; i < 10; i++)
        {
            PredictRail.AppendPoint(delta,i+1);
        }*/
    }

}
