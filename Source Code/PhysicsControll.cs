using Godot;
using System.Collections;
using ForceProjection;
using CollisionCalculation;
using RailSystem;

/// <summary>
/// Класс для глобального обновления физики
/// </summary>
public class GlobalPhysUpdater{

    /// <summary>
    /// Рельса, по которой движется синхронизирующая отметка
    /// </summary>
    /// <returns></returns>
    Rail WatcherRail = new Rail();

    /// <summary>
    /// Текущая отметка движения по рельсе, с которой синхронизируются остальные
    /// </summary>
    public RailFollower Watcher;

    /// <summary>
    /// Объект для хранения обработчиков силовых взаимодействий
    /// </summary>
    /// <returns></returns>
    public ForceProjHandler ForceHandler = new ForceProjHandler();

    /// <summary>
    /// Класс для глобальной обработки коллизий
    /// </summary>
    /// <returns></returns>
    public GlobalCollider Collider = new GlobalCollider();

    /// <summary>
    /// Основной контроллер для обновления рельс
    /// </summary>
    /// <returns></returns>
    public GlobalRailController RailController = new GlobalRailController();

    Queue Buffer = new Queue();

    public GlobalPhysUpdater(){
        WatcherRail.SetFirstPoint(new KineticPoint());
        RailController.AddRail(WatcherRail);
        Watcher = RailController.GetRailFollower(WatcherRail);
    }

    /// <summary>
    /// Метод для проверки коллизий для всех непроверенных участков
    /// </summary>
    public void RecalcCollisions(){
        int Count = RailController.MaxChange();
        RailController.CutToIndex(RailController.GetGlobalCount()-Count);
        for (int i = 0; i < Count; i++)
        {
            int ID = RailController.GetGlobalCount()-RailController.MaxChange()-1;
            Collider.BufferSetup();
            Collider.GlobalCalcResults(ID,ID);
            Collider.GlobalApplyResults(ID,ID);
            RailController.GlobalAdapt(1);
        }
    }

    /// <summary>
    /// Преобразование буффера рельс в массив
    /// </summary>
    /// <returns></returns>
    Rail[] RailArrayFromBuffer(){
        Rail[] Result = new Rail[Buffer.Count];
        for (int i = 0; i < Result.Length; i++)
        {
            Result[i] = (Rail)Buffer.Dequeue();
        }
        return Result;
    }

    /// <summary>
    /// Метод обновления массового теста рельс
    /// </summary>
    /// <param name="delta">интервал времени, который надо обновить</param>
    public void MoveToWatcher(){
        Rail[] Temp = RailArrayFromBuffer();
        RailController.AddRail(Temp);
        RecalcCollisions();
        int DeletedCount = Watcher.CurrentID()-1;
        for (int i = 0; i < DeletedCount; i++)
        {
            MoveForward();
        }
    } 
    
    /// <summary>
    /// Проверка коллизий на указанном элементе
    /// </summary>
    /// <param name="ID"></param>
    void CheckCollision(int ID){
        Collider.BufferSetup();
        Collider.GlobalCalcResults(ID,ID);
        Collider.GlobalApplyResults(ID,ID);
        RailController.GlobalAdapt();
    }

    /// <summary>
    /// Перемещение вперёд на один элемент
    /// </summary>
    void MoveForward(){
        RailController.MoveForvard(1);
        CheckCollision(RailController.GetGlobalCount()-1);
        RailController.GlobalAdapt();
    }

    /// <summary>
    /// Метод для добавления рельсы
    /// </summary>
    public void AddRail(Rail rail){
        Buffer.Enqueue(rail);
    }
}