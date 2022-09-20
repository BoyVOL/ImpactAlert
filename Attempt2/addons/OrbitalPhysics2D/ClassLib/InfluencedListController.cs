using Godot;

public class InfListController: RPListController{

    public PhysInfController InfContr;

    public InfListController(PhysicsControlNode parent, PhysInfController infContr):base(parent){
        InfContr = infContr;
    }
    
    public void UpdateAccel(int id, bool PhysRail, float t){
        foreach (var rail in Rails)
        {
            rail[id].Acceleration = InfContr.CombineAccels(rail[id],PhysRail,id,rail);
            Vector2 FirstAccel = rail[id].Acceleration;
            RailPoint Temp = rail[id].GetNext(t);
            Vector2 SecAccel = InfContr.CombineAccels(Temp,PhysRail,id,rail);
            rail[id].Acceleration = (FirstAccel+SecAccel)/2;
        }
    }
}