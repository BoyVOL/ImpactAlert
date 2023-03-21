using Godot;
using System.Collections.Generic;

/// <summary>
/// Subclass of physic control node addon for containing and updating rails
/// </summary>
public partial class RPListController: AddonWithList<RailPointList>{
    
    public RPListController(PhysicsControlNode parent):base(parent){

    }
    
    public Vector2 CombineAccels(RailPoint target, int id, RailPointList ThisRail){
        Vector2 result = Vector2.Zero;
        foreach (var rail in Items)
        {   
            if(rail != ThisRail){  
                foreach(AccelInfluencer inf in rail.Influencers)
                result += inf.GetAccel(target, id);
            }
        }
        return result;
    }
    
    public void UpdateAccel(int id, bool PhysRail, float t){
        foreach (var rail in Items)
        {
            rail[id].Acceleration = CombineAccels(rail[id],id,rail);
            Vector2 FirstAccel = rail[id].Acceleration;
            RailPoint Temp = rail[id].GetNext(t);
            Vector2 SecAccel = CombineAccels(Temp,id,rail);
            rail[id].Acceleration = (FirstAccel+SecAccel)/2;
        }
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
