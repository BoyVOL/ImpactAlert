using Godot;
using System.Collections.Generic;

/// <summary>
/// Класс, отвечающий за обработку физических взаимодействий 
/// </summary>
public class PhysicsControlNode: Node{

    private List<CustomPhysObject> Objects = new List<CustomPhysObject>();

    public InfluenceController InfContr = null;

    public CollisionController CollContr = null;

    public RPListController RPLists = null;

    public void Add(CustomPhysObject Object){
        Objects.Add(Object);
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
        RPLists.Reset();
        RPLists.UpdatePhysic(delta);
    }

}
