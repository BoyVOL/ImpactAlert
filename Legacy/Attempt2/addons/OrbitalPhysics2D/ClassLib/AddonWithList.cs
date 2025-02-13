using System.Collections.Generic;

public partial class AddonWithList<T> : PhysicsControlAddon{

    protected List<T> Items = new List<T>();

    public AddonWithList(PhysicsControlNode parent):base(parent){

    }

    public void Add(T item){
        Items.Add(item);
    }

    public void Remove(T item){
        Items.Remove(item);
    }

    public int Count(){
        return Items.Count;
    }
}