using Godot;
using System;
using System.Collections.Generic;
using Physics;

namespace Physics
{
    /// <summary>
    /// Структура, отвечающая за состояние точки в каждый момент времени
    /// </summary>
    public struct RailPoint
    {
        /// <summary>
        /// Положение точки в пространстве
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Угол поворота точки
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Поле, отвечающее за скорость в данной точке
        /// </summary>
        public Vector2 Speed;

        /// <summary>
        /// Поле, отвечающее за скорость поворота в данной точке
        /// </summary>
        public float RotSpeed;

        /// <summary>
        /// Поле, отвечающее за ускорение в данной точке
        /// </summary>
        public Vector2 Acceleration;        

        /// <summary>
        /// Метод, возвращающий новую точку, основываясь на данных старой.
        /// </summary>
        /// <param name="Point">Предыдущая точка</param>
        /// <param name="T">интервал времени, на котором проходит симуляция</param>
        /// <returns></returns>
        public RailPoint GetNextPoint(float T){
            RailPoint Result = new RailPoint();
            Result.Position = Position+Speed*T+(Acceleration*T*T)/2;
            Result.Rotation = Rotation+RotSpeed*T;
            Result.Speed = Speed+Acceleration*T;
            Result.RotSpeed = RotSpeed;
            Result.Acceleration = Acceleration;
            return Result;
        }

    }

    /// <summary>
    /// Класс для хранения и обработки рельс
    /// </summary>
    public class RailArray{

        /// <summary>
        /// Словарь массивов, отображающих рельсы
        /// </summary>
        Dictionary<Guid,List<RailPoint>> Rails = new Dictionary<Guid, List<RailPoint>>();

        /// <summary>
        /// Размер рельс в данном классе
        /// </summary>
        public readonly int RailSize;

        /// <summary>
        /// Временной интервал моделирования
        /// </summary>
        public readonly float TimeInterval;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="size">Размер рельс</param>
        /// <param name="TimeInterval">Интервал интерполяции</param>
        public RailArray(int size, float timeInterval){
            RailSize = size;
            TimeInterval = timeInterval;
        }

        /// <summary>
        /// Метод для Удаления с конца рельсы нужного количества элементов
        /// </summary>
        /// <param name="ID"></param>
        void RemoveFromEnd(Guid ID, int Count){
            int LastID = Rails[ID].Count - 1;
            Rails[ID].RemoveRange(LastID,Count);
        }

        /// <summary>
        /// Метод для подстройки количества элементов в рельсе по указанному индексу под общее число элементов 
        /// </summary>
        /// <param name="ID"></param>
        void AdaptCount(Guid ID){
            int CountDiff = Rails[ID].Count - RailSize;
            if(CountDiff)
        }

        /// <summary>
        /// Метод для добавления нового элемента рельсы
        /// </summary>
        /// <param name="ID"></param>
        void Expand(Guid ID){
            int LastID = Rails.Count - 1;
            Rails[ID].Add(Rails[ID][LastID].GetNextPoint(TimeInterval));
        }

        /// <summary>
        /// Метод для обновления рельсы на один элемент вперёд
        /// </summary>
        /// <param name="ID"></param>
        void MoveForward(Guid ID){
            Expand(ID);
            Rails[ID].RemoveAt(0);
        }

        /// <summary>
        /// Метод добавления новой рельсы
        /// </summary>
        /// <param name="Data">Данные по рельсе</param>
        /// <returns></returns>
        public Guid AddRail(List<RailPoint> Data){
            Guid ID = Guid.NewGuid();
            while(Rails.ContainsKey(ID)){
                ID = Guid.NewGuid();
            }
            Rails.Add(ID,Data);
            return ID;
        }

        /// <summary>
        /// Метод удаления рельсы из общего массива
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveRail(Guid ID){
            Rails.Remove(ID);
        }
    }
}
