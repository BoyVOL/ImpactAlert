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
    /// Метод для вычисления промежуточной точки для вектора приложенных сил
    /// </summary>
    /// <param name="T">момент времени, в которой нужно провести моделирование</param>
    /// <returns></returns>
    public virtual Vector2 MidPos(float T){
        return Position+SimSpeed*T+(Accel*T*T)/2;
    }

    /// <summary>
    /// Метод для вычисления промежуточной скорости для вычисления приложенных сил
    /// </summary>
    /// <param name="T">момент времени, в которой нужно провести моделирование</param>
    /// <returns>Промежуточная скорость</returns>
    public virtual Vector2 MidSpeed(float T){
        return SimSpeed+Accel*T;
    }

    /// <summary>
    /// Создание следующей точки с учётом ускорения
    /// </summary>
    /// <param name="T">момент времени до следующей точки</param>
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
    /// Устанавливает, сколько приближений делать при рассчёте воздействия сил
    /// </summary>
    public int ApprCount = 3;

    /// <summary>
    /// Метод для экстраполирования точек с учётом приложенных на них сил. Работает только для AccelPoint и его дочерних классов
    /// </summary>
    /// <param name="Count">количество точек, которые надо экстраполировать</param>
    /// <param name="Params">Параметры для обработки сил. Положение и скорость будут переписаны</param>
    /// <param name="shiftT">Смещение по времени/param>
    public void ExtrapolateForce(int Count, ForceParams Params, float shiftT = 0)
    {
        int LastID;
        if (Handler == null) base.Extrapolate(Count);
        else {
            //Проверка на то, что последняя точка является типа AccelPoint
            LastID = GetCount()-1;
            if (GetPoint(LastID).GetType() != typeof(AccelPoint)){
                base.Extrapolate(Count);
            }
            else {
                for (int i = 0; i < Count; i++)
                {
                    AccelPoint LastPoint = (AccelPoint)GetPoint(LastID);
                    for (int j = 0; j < ApprCount; j++)
                    {
                        Params.Pos = LastPoint.MidPos(GetInterval()/2);
                        Params.Speed = LastPoint.MidSpeed(GetInterval()/2);
                        LastPoint.Accel = Handler.GetResultAccel(Params, shiftT + LastID*GetInterval());
                    }
                    Extrapolate(1);
                    LastID++;
                }
            }
        }
    }
}
