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
                    Items[i].ScanRailForApproaches(Items[j],true);
                    Items[i].ScanRailForApproaches(Items[j],false);
                    Items[j].ScanRailForApproaches(Items[i],true);
                    Items[j].ScanRailForApproaches(Items[i],false);
                }
            }
        }
    }
}
