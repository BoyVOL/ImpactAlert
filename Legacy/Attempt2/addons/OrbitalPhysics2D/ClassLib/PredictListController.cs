public partial class PredictListController: InfListController{

    public PredictListController(PhysicsControlNode parent, PhysInfController infContr):base(parent,infContr){

    }

    public void LoadFromPhys(){
        foreach (var rail in Rails)
        {
            rail.SetFirstPoint(rail.Parent.PhysRail[0]);
        }
    }

}