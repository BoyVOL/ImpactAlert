using Godot;
using RailSystem;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Класс для хранения буффера рельс
/// </summary>
public abstract class Buffer{

    /// <summary>
    /// Поле для хранения буфферов объектов
    /// </summary>
    public IDictionary Dict;
}

public class RailBuffer : Buffer{

    /// <summary>
    /// Струкрура для хранения данных рельсы
    /// </summary>
    public struct RailData{
        Vector2 Position;
    }

    
    public RailBuffer(){
        Dict = new Dictionary<Rail,RailData[]>();
    }

    public void Add(Rail rail){
        
    }

    public RailData[] GetDatas(Rail rail){
        RailData[] Result = new RailData[rail.GetCount()];
    }
}