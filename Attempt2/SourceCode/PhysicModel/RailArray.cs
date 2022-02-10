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
    }

    /// <summary>
    /// Класс для хранения и обработки рельс
    /// </summary>
    public class RailArray{

        /// <summary>
        /// Словарь массивов, отображающих рельсы
        /// </summary>
        Dictionary<Guid,RailPoint[]> Rails = new Dictionary<Guid, RailPoint[]>();

        int Size = 1;

        /// <summary>
        /// Метод, возвращающий новую точку, основываясь на данных старой.
        /// </summary>
        /// <param name="Point">Предыдущая точка</param>
        /// <param name="T">интервал времени, на котором проходит симуляция</param>
        /// <returns></returns>
        public RailPoint GetNextPoint(RailPoint Point,float T){
            RailPoint Result = new RailPoint();
            Result.Position = Point.Position+Point.Speed*T+(Point.Acceleration*T*T)/2;
            Result.Rotation = Point.Rotation+Point.RotSpeed*T;
            Result.Speed = Point.Speed+Point.Acceleration*T;
            Result.RotSpeed = Point.RotSpeed;
            Result.Acceleration = Point.Acceleration;
            return Result;
        }

        public RailPoint[] ConstructRail(RailPoint Start){
            RailPoint[] Result = new RailPoint[Size];
            for (int i = 0; i < Result.Length; i++)
            {
                Result[i]
            }
            return Result;
        }

        /// <summary>
        /// Метод добавления новой рельсы
        /// </summary>
        /// <param name="Data">Данные по рельсе</param>
        /// <returns></returns>
        public Guid AddRail(RailPoint[] Data){
            Guid ID = Guid.NewGuid();
            while(Rails.ContainsKey(ID)){
                ID = Guid.NewGuid();
            }
            Rails.Add(ID,Data);
            return ID;
        }
    }
}
