public partial class PredRPListController: RPListController{

    public PredRPListController(PhysicsControlNode parent):base(parent){

    }

    public void LoadFromPhys(){
        foreach (var rail in Items)
        {
            rail.SetFirstPoint(rail.Parent.PhysRail[0]);
        }
    }

}