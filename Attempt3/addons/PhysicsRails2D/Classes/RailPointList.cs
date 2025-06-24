using Godot;
using System.Collections.Generic;

public partial class RailPointList : List<RailPoint>
{

    public RailPointList()
    {
    }

    /// <summary>
    /// Method that clears rail and sets it's last element as it's first
    /// </summary>
    public void ResetToStart()
    {
        int LocCount = Count - 1;
        RemoveRange(0, LocCount);
        this[0].time = 0;
    }

    /// <summary>
    /// Shifting time values of items untill Item[0].time == 0
    /// </summary>
    public void AdjustTime()
    {
        if (Count < 1) throw new System.Exception("No items in list");
        float shift = this[0].time;
        foreach (RailPoint p in this)
        {
            p.time -= shift;
        }
    }

    /// <summary>
    /// Shifting time values of items with parameter
    /// <paramref name="shift"/>
    /// </summary>
    public void AdjustTime(float shift)
    {
        if (Count < 1) throw new System.Exception("No items in list");
        foreach (RailPoint p in this)
        {
            p.time -= shift;
        }
    }

    /// <summary>
    /// Method wich inserts new point at a specific time based on already existing ones
    /// Next nodes will be recalculated
    /// </summary>
    /// <param name="time"></param>
    public int InsertPoint(float time)
    {
        int id = GetBeforeTime(time);
        RailPoint PointBefore = this[id];
        this.Insert(id + 1, PointBefore.GetNext(time - PointBefore.time));
        return id + 1;
    }

    /// <summary>
    /// Adds a simulated point ant the end of the list based on a previous point
    /// </summary>
    /// <param name="step"></param>
    public void Simulate(float step)
    {
        RailPoint Last = this[Count - 1];
        Add(Last.GetNext(step));
    }

    /// <summary>
    /// Method for getting nearest point before specified time
    /// </summary>
    /// <param name="T">specified time</param>
    /// <returns></returns>
    public int GetBeforeTime(float T)
    {
        for (int i = 0; i < Count; i++)
        {
            if (this[i].time > T) return i - 1;
        }
        return Count - 1;
    }

    /// <summary>
    /// Method for getting time interval between Rail point item with id index and the next one
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public float TimeBetweenPoints(int id)
    {
        if (id < this.Count)
        {
            return this[id + 1].time - this[id].time;
        }
        return 0;
    }

    public void LeapFrogAdjust(int id, float T)
    {
        this[id].Speed = this[id - 1].Speed + (this[id - 1].Acceleration + this[id].Acceleration) / 2 * T;
    }

    /// <summary>
    /// Method that will clear point and set it's first point as specified
    /// </summary>
    /// <param name="point"></param>
    public void SetFirstPoint(RailPoint point)
    {
        Clear();
        Add(point);
    }

    public string Stringify(int count = 5)
    {
        string Result = "";
        for (int i = 0; i < Count && i < count; i++)
        {
            Result += " " + this[i].Stringify();
        }
        return Result;
    }
}