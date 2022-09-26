using Godot;
using System.Collections.Generic;

public class RailPointList: List<RailPoint>{

    public CustomPhysObject Parent;

    public RailPointList(CustomPhysObject parent){
        Parent = parent;
    }

    /// <summary>
    /// Method that clears rail and sets it's last element as it's first
    /// </summary>
    public void ResetToStart(){
        int LocCount = Count-1;
        RemoveRange(0,LocCount);
        this[0].time = 0;
    }

    /// <summary>
    /// Method wich inserts new point at a specific time based on already existing ones
    /// Next nodes will be recalculated
    /// </summary>
    /// <param name="time"></param>
    public int InsertPoint(float time){
        int id = GetBeforeTime(time);
        RailPoint PointBefore = this[id];
        this.Insert(id+1,PointBefore.GetNext(time-PointBefore.time));
        return id+1;
    }

    /// <summary>
    /// Method which adds new point at the end of the list with a specific step
    /// </summary>
    /// <param name="step"></param>
    public void AppendPoint(float step){
        RailPoint Last = this[Count-1];
        Add(Last.GetNext(step));
    }

    /// <summary>
    /// Method for getting point before specified time
    /// </summary>
    /// <param name="T">specified time</param>
    /// <returns></returns>
    public int GetBeforeTime(float T){
        for (int i = 0; i < Count; i++)
        {
            if(this[i].time>T) return i-1;
        }
        return Count-1;
    }

    public Vector2 InterpolatePos(float T){
        int beforeID = GetBeforeTime(T);
        float TDiff = T-this[beforeID].time;
        if(beforeID < Count-1){ 
            return this[beforeID].GetInterPos(TDiff,this[beforeID+1].time-this[beforeID].time);
        } else {
            return this[beforeID].Position;
        }
    }

    /// <summary>
    /// Method that will set first rail point
    /// </summary>
    /// <param name="point"></param>
    public void SetFirstPoint(RailPoint point){
        Clear();
        Add(point);
    }
}