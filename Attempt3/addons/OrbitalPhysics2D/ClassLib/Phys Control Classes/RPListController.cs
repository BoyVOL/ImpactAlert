using Godot;
using System.Collections.Generic;

/// <summary>
/// Subclass of physic control node addon for containing and updating rails
/// </summary>
public partial class RPListController: AddonWithList<RailPointList>{
    
    public RPListController(PhysicsControlNode parent):base(parent){

    }

    public void Reset(){
        foreach (var list in Items)
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
        foreach (var list in Items)
        {
            if(list.Count==id) list.AppendPoint(delta);
        }
    }
    
    public RailPointList this[int i]{
        get => Items[i];
    }
}
