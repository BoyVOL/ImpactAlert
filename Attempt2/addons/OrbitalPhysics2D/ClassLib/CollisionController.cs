using Godot;
using System.Collections.Generic;

public class CollisionController: AddonWithList<Collider>{

    public CollisionController(PhysicsControlNode parent):base(parent){

    }

    public void CollisionTest(){
        foreach (var collider in Items)
        {
            collider.ClearCollisions();
        }
        for (int i = 0; i < Items.Count; i++)
        {
            for (int j = 0; j < Items.Count; j++)
            {
                if(i>j){
                    Items[i].ScanForApproaches(Items[j],true);
                    Items[j].ScanForApproaches(Items[i],true);
                }
            }
        }
    }
}
