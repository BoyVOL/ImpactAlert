using Godot;
using System.Collections;
using RailSystem;
using System;

namespace ForceProjection{

    /// <summary>
    /// Структура для передачи результатов воздействия
    /// </summary>
    public struct ForceResult{
        public Vector2 Accel;
    }

    /// <summary>
        /// Структура для задания параметров сил, которые будут накладываться с помощью данного проектора
        /// </summary>
    public struct ForceParams{

        /// <summary>
        /// Положение, в котором надо посмотреть данные
        /// </summary>
        public Vector2 Pos;
        public Vector2 Speed;

        /// <summary>
        /// Массив проекторов сил, которые надо исключить из вычисления
        /// </summary>
        public ForceProjector[] Exclude;

        public float Mass;
        public ForceParams(Vector2 pos, float mass){
            Pos = pos;
            Speed = Vector2.Zero;
            Mass = mass;
            Exclude = new ForceProjector[0];
        }
            
        public ForceParams(Vector2 pos, Vector2 speed, float mass){
            Pos = pos;
            Speed = speed;
            Mass = mass;
            Exclude = new ForceProjector[0];
        }
    }
    
    /// <summary>
    /// Базовый класс для всех источников силовых полей
    /// </summary>
    public abstract class ForceProjector{
        /// <summary>
        /// Возвращает вектор ускорения, соответствующий результату приложения силы в данной точке
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public abstract Vector2 GetAccelVector(ForceParams Params, float T = 0);
    }

    /// <summary>
    /// Класс, осуществляющий сведение всех взаимодействий в нужной точке в единый вектор
    /// </summary>
    public class ForceProjHandler{
        ArrayList MainArray = new ArrayList();

        /// <summary>
        /// Добавляет проектор силы в систему
        /// </summary>
        /// <param name="New"></param>
        public void AddProjector(ForceProjector New){
            MainArray.Add(New);
        }

        /// <summary>
        /// Убирает указанный проектор силы из системы
        /// </summary>
        /// <param name="Old"></param>
        public void RemoveProjector(ForceProjector Old){
            MainArray.Remove(Old);
        }

        /// <summary>
        /// Убирает проектор силы из системы по указанному индексу
        /// </summary>
        /// <param name="i"></param>
        public void RemoveProjector(int i){
            MainArray.RemoveAt(i);
        }

        public bool CheckIfExcluded(ForceParams Params, ForceProjector Item){
            for (int i = 0; i < Params.Exclude.Length; i++)
            {
                if(Params.Exclude[i] == Item) return true;
            }
            return false;
        }

        /// <summary>
        /// Метод сведения всех силовых влияний в единый вектор, воздействующий ускорению объекта в указанной точке
        /// </summary>
        /// <param name="forceParams"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public ForceResult GetResult(ForceParams forceParams, float T = 0){
            ForceResult Result = new ForceResult();
            Result.Accel = Vector2.Zero;
            foreach (ForceProjector item in MainArray)
            {
                if(!CheckIfExcluded(forceParams,item)){
                    Result.Accel += item.GetAccelVector(forceParams,T);
                }
            }
            return Result;
        }
    }

    public class ForceFieldInfluencer : IRaillnfluence{
    
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
    /// <param name="GlobalT">Глобальное время</param>
    /// <returns></returns>
    ForceResult GetMidResults(AccelPoint point, float GlobalT, float Interval){
        if(Handler == null) throw new Exception("ForceProjHandler must not be null");
        ForceResult Result = new ForceResult();
        Result.Accel = Vector2.Zero;
        ForceResult Temp;
        float InterStep = Interval/ApprSteps;
        for (float t = 0; t <= Interval; t+=InterStep)
        {
            Params.Pos = point.MidPos(t);
            Params.Speed = point.MidSpeed(t);
            Temp = Handler.GetResult(Params, GlobalT);
            Result.Accel += Temp.Accel;
        }
        Result.Accel /= (ApprSteps+1);
        return Result;
    }

    void IRaillnfluence.UpdatePoint(RailPoint Point, float T, float Interval){
        if(Handler == null) throw new Exception("ForceProjHandler must not be null");
       if(Point.GetType() == typeof(AccelPoint)){
            AccelPoint TempPoint = (AccelPoint)Point;
            ForceResult Temp = new ForceResult();
            float ApprStep = Interval/ApprCount;
            for (int j = 0; j < ApprCount; j++)
            {
                Params.Pos = TempPoint.MidPos(Interval/2);
                Params.Speed = TempPoint.MidSpeed(Interval/2);
                Temp = GetMidResults(TempPoint, T, Interval);
                TempPoint.Accel = Temp.Accel;
            }
        }
    }
}
}