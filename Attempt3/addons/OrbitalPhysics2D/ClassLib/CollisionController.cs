using Godot;
using System.Collections.Generic;

public partial class CollisionController: AddonWithList<Collider>{

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
                    Items[i].ScanRailForApproaches(Items[i],Items[i].Parent.PhysRail,Items[j].Parent.PhysRail);
                    Items[i].ScanRailForApproaches(Items[i],Items[i].Parent.PredictionRail,Items[j].Parent.PredictionRail);                   
                    Items[j].ScanRailForApproaches(Items[j],Items[j].Parent.PhysRail,Items[i].Parent.PhysRail);
                    Items[j].ScanRailForApproaches(Items[j],Items[j].Parent.PredictionRail,Items[i].Parent.PredictionRail);
                }
            }
        }
    }
}