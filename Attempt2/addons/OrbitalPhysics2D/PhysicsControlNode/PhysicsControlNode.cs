using Godot;
using System.Collections.Generic;

/// <summary>
/// Класс, отвечающий за обработку физических взаимодействий 
/// </summary>
public class PhysicsControlNode: Node{
    
    private List<GravityInfluencer> Influencers = new List<GravityInfluencer>();

    private List<GravityObject> Objects = new List<GravityObject>();

    public override void _EnterTree()
    {
        base._EnterTree();
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }

}
