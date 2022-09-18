public class InfListController: RPListController{

    public InfluenceController InfContr;

    public InfListController(PhysicsControlNode parent, InfluenceController infContr):base(parent){
        InfContr = infContr;
    }

}