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

    public GlobalPhysUpdater(){
        WatcherRail.SetFirstPoint(new KineticPoint());
        RailController.AddRail(WatcherRail);
        Watcher = RailController.GetRailFollower(WatcherRail);
    }

    /// <summary>
    /// Метод обновления массового теста рельс
    /// </summary>
    /// <param name="delta">интервал времени, который надо обновить</param>
    public void MoveToWatcher(){
        int DeletedCount = Watcher.CurrentID()-1;
        for (int i = 0; i < DeletedCount; i++)
        {
            MoveForward();
        }
    } 

    

    /// <summary>
    /// Перемещение вперёд на один элемент
    /// </summary>
    public void MoveForward(){
        RailController.MoveForvard(1);
        Collider.GlobalCollProcess(RailController.GetGlobalCount()-1);
        RailController.GlobalAdapt();
    }
}