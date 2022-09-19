public class InfListController: RPListController{

    public PhysInfController InfContr;

    public InfListController(PhysicsControlNode parent, PhysInfController infContr):base(parent){
        InfContr = infContr;
    }

}