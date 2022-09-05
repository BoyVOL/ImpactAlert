using Godot;
using System;
using System.Collections.Generic;

public class CustomPhysObject: Node2D{

    protected PhysicsControlNode PhysNode = null;

    /// <summary>
    /// список точек симуляции объекта
    /// </summary>
    /// <typeparam name="RailPoint"></typeparam>
    /// <returns></returns>
    public List<RailPoint> RailPoints = new List<RailPoint>();
    
    /// <summary>
    /// Метод, обнуляющий состояние рельсы
    /// </summary>
    public void ResetRail(){
        
    }

    public override void _EnterTree()
    {
        base._EnterTree();
        PhysNode = GetNode<PhysicsControlNode>("/root/Autoload/PhysicsControlNode");
    }

    public override void _PhysicsProcess(float delta)
    {
        base._PhysicsProcess(delta);
    }
}
