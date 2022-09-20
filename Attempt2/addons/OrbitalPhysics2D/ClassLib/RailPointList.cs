using Godot;
using System.Collections.Generic;

public class RailPointList{
    
    /// <summary>
    /// List of all simulation points of this object
    /// </summary>
    /// <typeparam name="RailPoint"></typeparam>
    /// <returns></returns>
    protected List<RailPoint> Points = new List<RailPoint>();

    /// <summary>
    /// Method that clears rail and sets it's last element as it's first
    /// </summary>
    public void ResetToStart(){
        int Count = Points.Count-1;
        Points.RemoveRange(0,Count);
        Points[0].time = 0;
    }

    /// <summary>
    /// Method wich inserts new point at a specific time based on already existing ones
    /// Next nodes will be recalculated
    /// </summary>
    /// <param name="time"></param>
    public int InsertPoint(float time){
        int id = GetBeforeTime(time);
        RailPoint PointBefore = Points[id];
        Points.Insert(id+1,PointBefore.GetNext(time-PointBefore.time));
        return id+1;
    }

    public void RecalcAfter(int id){
        for (int i = id; i < Points.Count-1; i++)
        {
            Points[i+1] = Points[i].GetNext(Points[i+1].time-Points[i].time);
        }
    }

    public void Add(RailPoint point){
        Points.Add(point);
    }

    public void Remove(RailPoint point){
        Points.Remove(point);
    }

    /// <summary>
    /// Method which adds new point at the end of the list with a specific step
    /// </summary>
    /// <param name="step"></param>
    public void AppendPoint(float step){
        RailPoint Last = Points[Points.Count-1];
        Points.Add(Last.GetNext(step));
    }

    /// <summary>
    /// Method for getting point before specified time
    /// </summary>
    /// <param name="T">specified time</param>
    /// <returns></returns>
    public int GetBeforeTime(float T){
        for (int i = 0; i < Points.Count; i++)
        {
            if(Points[i].time>T) return i-1;
        }
        return Points.Count-1;
    }

    public RailPoint this[int i]{
        get => Points[i];
        set => Points[i] = value;
    }

    public int Count(){
        return Points.Count;
    }

    /// <summary>
    /// Method that will set first rail point
    /// </summary>
    /// <param name="point"></param>
    public void SetFirstPoint(RailPoint point){
        Points.Clear();
        Points.Add(point);
    }
}