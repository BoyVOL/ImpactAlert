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
                //result += item.GetAccel(target,physRail, id);
            }
        }
        return result;
    }
    
    public void UpdateAccel(int id, bool PhysRail, float t){
        foreach (var rail in Items)
        {
            rail[id].Acceleration = InfContr.CombineAccels(rail[id],PhysRail,id,rail);
            Vector2 FirstAccel = rail[id].Acceleration;
            RailPoint Temp = rail[id].GetNext(t);
            Vector2 SecAccel = InfContr.CombineAccels(Temp,PhysRail,id,rail);
            rail[id].Acceleration = (FirstAccel+SecAccel)/2;
        }
    }
}