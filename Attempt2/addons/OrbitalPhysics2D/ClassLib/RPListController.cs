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

    /// <summary>
    /// Method for appending point at id 
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="id"></param>
    public void AppendPoint(float delta,int id){
        foreach (var list in Rails)
        {
            if(list.Count()==id) list.AppendPoint(delta);
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
    
    public RailPointList this[int i]{
        get => Rails[i];
    }

    public void Remove(RailPointList List){
        Rails.Remove(List);
    }

    public int Count(){
        return Rails.Count;
    }
}
