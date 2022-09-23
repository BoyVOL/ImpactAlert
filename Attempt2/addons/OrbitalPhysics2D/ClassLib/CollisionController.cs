using Godot;
using System.Collections.Generic;

public class CollisionController: AddonWithList<Collider>{

    public CollisionController(PhysicsControlNode parent):base(parent){

    }

    public void CollisionTest(){
        foreach (var collider in Items)
        {
            collider.CollisionPoints.Clear();
        }
        foreach (var collider1 in Items)
        {
            foreach (var collider2 in Items)
            {
                if(collider1!=collider2){
                    collider1.CollisionPoints.Add(collider1.GetClosestPos(collider2,true,0));
                    collider2.CollisionPoints.Add(collider2.GetClosestPos(collider1,true,0));
                }
            }
        }
    }
}
