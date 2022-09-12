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
        for (int i = 0; i < Points.Count-1; i++)
        {
            Points.RemoveAt(i);
        }
        Points[0].time = 0;
    }

    /// <summary>
    /// Method wich inserts new point at a specific time based on already existing ones
    /// </summary>
    /// <param name="time"></param>
    public void InsertPoint(float time){
        RailPoint PointBefore = Points[GetBeforeTime(time)];
        Points.Add(PointBefore.GetNext(time-PointBefore.time));
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