using Godot;

/// <summary>
/// Subclass of rail updater that enables influence betweeb rails
/// </summary>
public partial class InfRPListController: RPListController{

    public PhysInfController InfContr;

    public InfRPListController(PhysicsControlNode parent, PhysInfController infContr):base(parent){
        InfContr = infContr;
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
}