using Godot;
using System;
using System.Collections.Generic;
using PhysRails2D;

public partial class RailAutomator : Node
{

    public List<AutoRailNode> rails = new List<AutoRailNode>();

    [Export]
    public int PredictionCount = 100;

    /// <summary>
    /// Moves first point by one step
    /// </summary>
    /// <param name="delta"></param>
    void MoveFirstPoint(double delta)
    {
        foreach (AutoRailNode rail in rails)
        {
            if (rail.Items.Count > 2) rail.Items.LeftAtStart(2);
            rail.Items.LeftAtEnd(1);
        }
    }

    /// <summary>
    /// Adds prediction points
    /// </summary>
    /// <param name="delta"></param>
    void AddPredictions(double delta)
    {
        for (int i = 0; i < PredictionCount - 1; i++)
        {
            foreach (AutoRailNode rail in rails)
            {
                rail.Simulate((float)delta);
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        MoveFirstPoint(delta);
        AddPredictions(delta);
    }

}
