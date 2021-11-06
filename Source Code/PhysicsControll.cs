using Godot;
using System.Collections;
using ForceProjection;
using CollisionCalculation;

/// <summary>
/// Класс для глобального обновления физики
/// </summary>
public class GlobalPhysUpdater{

    /// <summary>
    /// Объект для обработки силовых взаимодействий между объектами
    /// </summary>
    /// <returns></returns>
    ForceProjHandler ForceHandler = new ForceProjHandler();

    /// <summary>
    /// Класс для глобальной обработки коллизий
    /// </summary>
    /// <returns></returns>
    GlobalCollider Collider = new GlobalCollider();

    
}