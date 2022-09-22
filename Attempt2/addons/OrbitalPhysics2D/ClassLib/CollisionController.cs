using Godot;
using System.Collections.Generic;

public class CollisionController: AddonWithList<Collider>{

    public CollisionController(PhysicsControlNode parent):base(parent){

    }

    public void CollisionTest(){
        foreach (var collider1 in Items)
        {
            foreach (var collider2 in Items)
            {
                GD.Print(collider1.GetClosestTime(collider2,true,0)/1000);
            }
        }
    }
}
