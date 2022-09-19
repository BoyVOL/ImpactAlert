using System.Collections.Generic;

public class AddonWithList<T> : PhysicsControlAddon{

    private List<T> Items = new List<T>();

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