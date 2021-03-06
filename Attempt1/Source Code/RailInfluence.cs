using Godot;
using RailSystem;
using System.Collections;

public interface IRaillnfluence{

    void UpdatePoint(RailPoint Point, float T, float Interval);

}


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
/// Дочерний класс рельсы для работы с различными механизмами влияния на её просчёт
/// </summary>
public class InfluencedRail : ProxyRail{
    
    /// <summary>
    /// Список классов, способных изменять моделирование точек относительно времени
    /// </summary>
    /// <returns></returns>
    public ArrayList Influencers = new ArrayList();
    
    public InfluencedRail(){}

    /// <summary>
    /// Перегрузка конструктора копирования
    /// </summary>
    /// <param name="Other">другая рельса, которую надо скопировать</param>
    /// <returns></returns>
    public InfluencedRail(InfluencedRail Other) : base(Other){
        Influencers = Other.Influencers;
    }

    /// <summary>
    /// Добавление объекта влияния
    /// </summary>
    /// <param name="Influencer"></param>
    public void AddInfluencer(IRaillnfluence Influencer){
        Influencers.Add(Influencer);
    }

    /// <summary>
    /// Удаление объекта влияния
    /// </summary>
    /// <param name="Influencer"></param>
    public void DeleteInfluencer(IRaillnfluence Influencer){
        Influencers.Remove(Influencer);
    }

    /// <summary>
    /// Удаление объекта влияния основываясь на его индексе
    /// </summary>
    /// <param name="ID"></param>
    public void DeleteInfluencer(int ID){
        Influencers.RemoveAt(ID);
    }

    public IRaillnfluence GetInfluencer(int ID){
        return (IRaillnfluence)Influencers[ID];
    }

    /// <summary>
    /// Метод для экстраполирования точек с учётом указанного в классе рельсы влияния.
    /// </summary>
    /// <param name="Count">количество точек, которые надо экстраполировать</param>
    /// <param name="shiftT">смещение относительно начала рельс силовых проекторов</param>
    public void InfluencedExtrapolate(int Count, float shiftT){
        int LastID;
        if (Influencers.Count == 0) base.Extrapolate(Count);
        else {
            //Проверка на то, что последняя точка является типа AccelPoint
            LastID = GetCount()-1;
            for (int i = 0; i < Count; i++)
            {
                IRaillnfluence Temp;
                for (int j = 0; j < Influencers.Count; j++)
                {
                    Temp = (IRaillnfluence)Influencers[j];
                    Temp.UpdatePoint(GetPoint(LastID),shiftT + LastID*GetInterval(), GetInterval());
                }
                base.Extrapolate(1);
                LastID++;
            }
        }
    }

    /// <summary>
    /// Метод для экстраполирования точек с учётом приложенных на них сил. Работает только для AccelPoint и его дочерних классов
    /// </summary>
    /// <param name="Count">количество точек, которые надо экстраполировать</param>
    public override void Extrapolate(int Count)
    {
        InfluencedExtrapolate(Count,base.ShiftT);
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