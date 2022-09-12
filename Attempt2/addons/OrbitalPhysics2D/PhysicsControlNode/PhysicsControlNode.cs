using Godot;
using System.Collections.Generic;

/// <summary>
/// Класс, отвечающий за обработку физических взаимодействий 
/// </summary>
public class PhysicsControlNode: Node{

    private List<GravityObject> Objects = new List<GravityObject>();

    public void AddObject(GravityObject Object){
        Objects.Add(Object);
    }
    
    public void RemoveObject(GravityObject Object){
        Objects.Remove(Object);
    }

    public void Reset(){
        foreach (var obj in Objects)
        {
            obj.RailPoints.ResetToStart();
        }
    }

    public void Update(float delta){
        foreach (var obj in Objects)
        {
            obj.RailPoints.AppendPoint(delta);
        }
    }

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
        Reset();
        Update(delta);
    }

}
