using Godot;
using System.Collections.Generic;

/// <summary>
/// Класс, отвечающий за обработку физических взаимодействий 
/// </summary>
public class PhysicsControlNode: Node{

    private List<CustomPhysObject> Objects = new List<CustomPhysObject>();

    public InfluenceController InfContr = null;

    public CollisionController CollContr = null;

    public RPListController PhysRail = null;

    public RPListController PredictRail = null;

    public List<CustomPhysObject> NotLoaded = new List<CustomPhysObject>();

    public PhysicsControlNode():base(){
        InfContr = new InfluenceController(this);
        CollContr = new CollisionController(this);
        PhysRail = new RPListController(this);
        PredictRail = new RPListController(this);
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
        PhysRail.Reset();
        LoadAll();
        PhysRail.AppendPoint(delta);
    }

}
