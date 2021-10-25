using Godot;
using RailSystem;
using ForceProjection;

 /// <summary>
 /// Класс для моделирования движения с ускорением
 /// </summary>
public class AccelPoint : KineticPoint{

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
    /// Перегрузка конструктора копирования
    /// </summary>
    /// <param name="Other">Другая точка, с которой надо скопировать данные</param>
    /// <returns></returns>
    public AccelPoint(AccelPoint Other) : base(Other){
        Accel = Other.Accel;
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
public class ForceRail : Rail{
    
    /// <summary>
    /// Обработчик сил для данной рельсы
    /// </summary>
    public ForceProjHandler Handler = null;

    /// <summary>
    /// Устанавливает, сколько раз пересчитывать ускорение для более точного результата
    /// </summary>
    public int ApprCount = 2;

    /// <summary>
    /// Устанавливает, в скольких промежуточных точках брать ускорение, из которого потом берётся среднее
    /// </summary>
    public int ApprSteps = 2;

    public ForceRail(){}

    /// <summary>
    /// Перегрузка конструктора копирования
    /// </summary>
    /// <param name="Other">другая рельса, которую надо скопировать</param>
    /// <returns></returns>
    public ForceRail(ForceRail Other) : base(Other){
        Handler = Other.Handler;
        ApprCount = Other.ApprCount;
        ApprSteps = Other.ApprSteps;
        Params = Other.Params;
    }

    /// <summary>
    /// Поле, отвечающее за хранение параметров силового взаимодействия данной рельсы
    /// </summary>
    /// <returns></returns>
    public ForceParams Params = new ForceParams(Vector2.Zero,0);

    /// <summary>
    /// Функция, отвечающая за апробацию ускорения в нескольких точках рельсы, количество которых задаётся параметром ApprSteps, и вывод среднего арифметического
    /// </summary>
    /// <param name="point">Точка, которую надо проверить</param>
    /// <param name="GlobalT">Глобальное время</param>
    /// <returns></returns>
    ForceResult GetMidResults(AccelPoint point, float GlobalT){
        ForceResult Result = new ForceResult();
        Result.Accel = Vector2.Zero;
        ForceResult Temp;
        float InterStep = GetInterval()/ApprSteps;
        for (float t = 0; t <= GetInterval(); t+=InterStep)
        {
            Params.Pos = point.MidPos(t);
            Params.Speed = point.MidSpeed(t);
            Temp = Handler.GetResult(Params, GlobalT);
            Result.Accel += Temp.Accel;
        }
        Result.Accel /= (ApprSteps+1);
        return Result;
    }  

    /// <summary>
    /// Метод для экстраполирования точек с учётом приложенных на них сил. Работает только для AccelPoint и его дочерних классов
    /// Перегрузка, учитывающая смещение по времени относительно родителя
    /// </summary>
    /// <param name="Count">количество точек, которые надо экстраполировать</param>
    /// <param name="shiftT">смещение относительно начала рельс силовых проекторов</param>
    public void Extrapolate(int Count, float shiftT){
        ForceResult Temp = new ForceResult();
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
                    float ApprStep = GetInterval()/ApprCount;
                    for (int j = 0; j < ApprCount; j++)
                    {
                        Params.Pos = LastPoint.MidPos(GetInterval()/2);
                        Params.Speed = LastPoint.MidSpeed(GetInterval()/2);
                        Temp = GetMidResults(LastPoint, shiftT + LastID*GetInterval());
                        LastPoint.Accel = Temp.Accel;
                    }
                    base.Extrapolate(1);
                    LastID++;
                }
            }
        }
    }

    /// <summary>
    /// Метод для экстраполирования точек с учётом приложенных на них сил. Работает только для AccelPoint и его дочерних классов
    /// </summary>
    /// <param name="Count">количество точек, которые надо экстраполировать</param>
    public override void Extrapolate(int Count)
    {
        Extrapolate(Count,base.ShiftT);
    }

    /// <summary>
    /// Заменяет метод на эктраполирование с новыми механизмами
    /// </summary>
    /// <param name="Count"></param>
    public override void ReExtrapolate(int Count){
        if(GetCount()>1){
                RemoveFromEnd(GetCount()-1);
				this.Extrapolate(Count);
			}
    }

    public override void SetInterval(float newInterval){
        int CountBefore = GetCount();
        if(CountBefore>1){
                RemoveFromEnd(CountBefore-1);
        }
        base.SetInterval(newInterval);
        this.ReExtrapolate(CountBefore);
    }
}
