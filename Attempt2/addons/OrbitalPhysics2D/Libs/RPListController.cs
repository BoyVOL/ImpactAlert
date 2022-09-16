using Godot;
using System.Collections.Generic;

public class RPListController: PhysicsControlAddon{
    
    public RPListController(PhysicsControlNode parent):base(parent){

    }

    public List<RailPointList> Rails = new List<RailPointList>();

    public void Reset(){
        foreach (var list in Rails)
        {
            list.ResetToStart();
        }
    }

    public void AppendPoint(float delta){
        foreach (var list in Rails)
        {
            list.AppendPoint(delta);
        }
    }

    public void DebugInsert(float delta){
        foreach (var list in Rails)
        {
            int id = list.InsertPoint(delta/2);
            list[id].Position.y+=20;
            list.RecalcAfter(id);
        }
    }

    public void Add(RailPointList List){
        Rails.Add(List);
    }

    public void Remove(RailPointList List){
        Rails.Remove(List);
    }
}
