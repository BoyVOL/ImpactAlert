public partial class PredRPListController: InfRPListController{

    public PredRPListController(PhysicsControlNode parent, PhysInfController infContr):base(parent,infContr){

    }

    public void LoadFromPhys(){
        foreach (var rail in Items)
        {
            rail.SetFirstPoint(rail.Parent.PhysRail[0]);
        }
    }

}