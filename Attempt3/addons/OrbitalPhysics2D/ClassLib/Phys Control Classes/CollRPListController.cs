public partial class CollRPListController: InfRPListController{

    public ColliderController CollContr;

    public CollRPListController(PhysicsControlNode parent, PhysInfController infContr, ColliderController collContr):base(parent,infContr){
        CollContr = collContr;
    }
}