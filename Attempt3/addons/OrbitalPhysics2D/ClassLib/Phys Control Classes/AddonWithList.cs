using System.Collections.Generic;

/// <summary>
/// Subclass for physic control node addons that contains list
/// </summary>
/// <typeparam name="T">List item type</typeparam>
public partial class AddonWithList<T> : PhysicsControlAddon{

    protected List<T> Items = new List<T>();

    public AddonWithList(PhysicsControlNode parent):base(parent){

    }

    public void Add(T item){
        if(!Items.Contains(item)){
            Items.Add(item);
        }
    }

    public void Remove(T item){
        Items.Remove(item);
    }

    public int Count(){
        return Items.Count;
    }
}