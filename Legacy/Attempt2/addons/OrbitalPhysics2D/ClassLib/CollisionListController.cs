public partial class CollListController: InfListController{

    public CollisionController CollContr;

    public CollListController(PhysicsControlNode parent, PhysInfController infContr, CollisionController collContr):base(parent,infContr){
        CollContr = collContr;
    }
}