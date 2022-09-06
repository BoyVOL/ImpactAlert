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
    /// Method wich creates end rail point
    /// </summary>
    /// <param name="delta"></param>
    public void GenEndPoint(float delta){
        RailPoint Start = Points[0];
        Points.Add(Start.GetNext(delta));
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