using Godot;
using RailSystem;
using ForceProjection;

 /// <summary>
 /// Класс для моделирования движения с ускорением
 /// </summary>
class AccelPoint : KineticPoint{

    /// <summary>
    /// Параметр, хранящий ускорение
    /// </summary>
    public Vector2 Accel;
    
    public AccelPoint(Vector2 pos, float rot) : base(pos,rot){
        Accel = Vector2.Zero;
    }

    public AccelPoint(Vector2 pos, float rot, Vector2 simSpeed, Vector2 accel, float simRotSpeed = 0) : base(pos,rot,simSpeed,simRotSpeed){
        Accel = accel;
    }

    /// <summary>
    /// Создание следующей точки с учётом ускорения
    /// </summary>
    /// <param name="T"></param>
    /// <returns></returns>
    public override RailPoint CreateNextPoint(float T)
    {
        return new AccelPoint(Position+SimSpeed*T+(Accel*T*T)/2,Rotation+SimRotSpeed*T,SimSpeed+Accel*T,Accel,SimRotSpeed);
    }
}

/// <summary>
/// Дочерний класс рельсы для работы с силами взаимодействия
/// </summary>
class ForceRail : Rail{
    /// <summary>
    /// Обработчик сил для данной рельсы
    /// </summary>
    public ForceProjHandler Handler = null;

    /// <summary>
    /// Метод для экстраполирования точек с учётом приложенных на них сил. Работает только для AccelPoint и его дочерних классов
    /// </summary>
    /// <param name="Count">количество точек, которые надо экстраполировать</param>
    /// <param name="Params">Параметры для обработки сил. Положение и скорость будут переписаны</param>
    /// <param name="shiftT">Смещение по времени/param>
    public void ExtrapolateForce(int Count, ForceParams Params, float shiftT = 0)
    {
        if (Handler == null) base.Extrapolate(Count);
        else {
            //Проверка на то, что последняя точка является типа AccelPoint
            int LastID = GetCount()-1;
            if (GetPoint(LastID).GetType() != typeof(AccelPoint)){
                base.Extrapolate(Count);
            }
            else {
                for (int i = 0; i < Count; i++)
                {
                    AccelPoint LastPoint = (AccelPoint)GetPoint(LastID);
                    Params.Pos = LastPoint.Position;
                    Params.Speed = LastPoint.SimSpeed;
                    LastPoint.Accel = Handler.GetResultAccel(Params, shiftT + LastID*GetInterval());
                    Extrapolate(1);
                    LastID++;
                }
            }
        }
    }
}