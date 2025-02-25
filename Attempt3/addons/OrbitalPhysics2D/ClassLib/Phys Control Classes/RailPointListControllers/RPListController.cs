using Godot;
using System.Collections.Generic;

/// <summary>
/// Subclass of physic control node addon for containing and updating rails
/// </summary>
public partial class RPListController: AddonWithList<RailPointList>{
    
    public RPListController(PhysicsControlNode parent):base(parent){

    }

    public void LoadFromPhys(){
        foreach (var rail in Items)
        {
            if(rail.Parent.PhysRail != null){   
                rail.SetFirstPoint(rail.Parent.PhysRail[0]);
            } else {
                PredictionRailController parent = (PredictionRailController)rail.Parent;
                if(parent.Parent.PhysRail != null){
                    rail.SetFirstPoint(new RailPoint(parent.Parent.PhysRail[0]));
                }
            }
        }
    }
    
    public Vector2 CombineAccels(RailPoint target, int id, RailPointList ThisRail){
        Vector2 result = Vector2.Zero;
        foreach (var rail in Items)
        {
            if(rail != ThisRail){  
                foreach(ObjectInfluencer inf in rail.Influencers)
                result += inf.GetAccel(target, id);
            }
        }
        return result;
    }
    
    public void UpdateAccel(int id){
        foreach (var rail in Items)
        {
            rail[id].Acceleration = CombineAccels(rail[id],id,rail);
        }
    }

    public void Reset(){
        foreach (var rail in Items)
        {
            rail.ResetToStart();
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

    public void LeapFrogAdjust(int id, float T){
        foreach(var list in Items)
        {
            list.LeapFrogAdjust(id,T);
        }
    }
    
    public RailPointList this[int i]{
        get => Items[i];
    }
}
