using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics
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
        
        /// <summary>
		/// Метод, возвращающий точку, интерполированную на заданном моменте времени
		/// </summary>
		/// <param name="T">Момент времени, данные в котором надо вернуть</param>
		/// <returns></returns>
		public RailPoint Interpolate(float T){
            RailPoint Result = new RailPoint();
            Result.Rotation = RotSpeed*T;
            Result.Position = Speed*T;
            return Result;
		}

		/// <summary>
		/// Возвращает значение времени пересечения с указанной точкой, начиная от текущей точки.
		/// </summary>
		/// <param name="Target">Вторая точка, с которой просчитывается пересечение путей</param>
		/// <returns></returns>
		public float CPA(RailPoint Target){
			return MathExtra.cpaTime(Position,Target.Position,Speed,Target.Speed);
		}

		/// <summary>
		/// Возвращает значение времени пересечения с указанной точкой, начиная от текущей точки.
		/// Перегрузка для 2д вектора
		/// </summary>
		/// <param name="Vector">2Д вектор, с которым просчитывается пересечение путей</param>
		/// <returns></returns>
		public float CPA(Vector2 Vector){
			return MathExtra.cpaTime(Position,Vector,Speed,new Vector2(0,0));
		}

        /// <summary>
        /// Метод для преобразования контента в стринг
        /// </summary>
        /// <returns></returns>
        public string Stringify(){
            string Result = "";
            Result += "Position = ("+Position.x+";"+Position.y+")";
            return Result;
        }
    }

    /// <summary>
    /// Класс для хранения и обработки рельс
    /// </summary>
    public class RailArray{

        /// <summary>
        /// Объект для случайной генерации идентификаторов
        /// </summary>
        /// <returns></returns>
        Random IDGen = new Random();

        /// <summary>
        /// Словарь массивов, отображающих рельсы
        /// </summary>
        Dictionary<int,List<RailPoint>> Rails = new Dictionary<int, List<RailPoint>>();

        /// <summary>
        /// Класс, отвечающий за обработку гравитационного взаимодействия
        /// </summary>
        public readonly GravityHandler Gravity;

        /// <summary>
        /// Размер рельс в данном классе
        /// </summary>
        public readonly int RailSize;

        /// <summary>
        /// Временной интервал между точками
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
            Gravity = new GravityHandler(Rails);
        }

        public string StringifyRail(int ID){
            string Result = "";
            foreach (var item in Rails[ID])
            {
                Result += item.Stringify()+"\n";
            }
            return Result;
        }

        /// <summary>
        /// Метод для Удаления с конца рельсы нужного количества элементов
        /// </summary>
        /// <param name="ID"></param>
        void RemoveFromEnd(int ID, int Count){
            int LastID = Rails[ID].Count - 1;
            Rails[ID].RemoveRange(LastID,Count);
        }

        public bool RailExists(int ID){
            return Rails.ContainsKey(ID);
        }

        /// <summary>
        /// Метод для подстройки количества элементов в рельсе по указанному индексу под общее число элементов 
        /// </summary>
        /// <param name="ID"></param>
        void AdaptCount(int ID){
            int CountDiff = Rails[ID].Count - RailSize;
            if(CountDiff == 0){
                return;
            } else if (CountDiff > 0){
                RemoveFromEnd(ID,CountDiff);
            } else {
                //CountDiff < 0
                int CD = Math.Abs(CountDiff);
                for (int i = 0; i < CD; i++)
                {
                    Expand(ID);
                }
            }
        }

        /// <summary>
        /// Метод для добавления нового элемента рельсы
        /// </summary>
        /// <param name="ID"></param>
        void Expand(int ID){
            int LastID = Rails[ID].Count - 1;
            Rails[ID].Add(Rails[ID][LastID].GetNextPoint(TimeInterval));
        }

        /// <summary>
        /// Метод для обновления рельсы на один элемент вперёд
        /// </summary>
        /// <param name="ID"></param>
        void MoveForward(int ID){
            Expand(ID);
            Rails[ID].RemoveAt(0);
        }

        /// <summary>
        /// Метод для движения разом всех рельс
        /// </summary>
        public void MoveForwardAll(){
            foreach (var ID in Rails.Keys)
            {
                MoveForward(ID);
            }
        }

        /// <summary>
        /// Метод, который возвращает случайный свободный айди в словаре
        /// </summary>
        /// <returns></returns>
        public int GetFreeID(){
            int ID = IDGen.Next();
            while(Rails.ContainsKey(ID)){
                ID = IDGen.Next();
            }
            return ID;
        }

        /// <summary>
        /// Метод добавления новой рельсы
        /// </summary>
        /// <param name="Data">Данные по рельсе</param>
        /// <returns></returns>
        public int AddRail(List<RailPoint> Data){
            int ID = GetFreeID();
            AddRail(ID,Data);
            return ID;
        }

        /// <summary>
        /// Метод добавления новой рельсы
        /// Перегрузка для добавления рельсы из одной точки
        /// </summary>
        /// <param name="Start">Эта самая одна точка</param>
        /// <returns></returns>
        public int AddRail(RailPoint Start){
            List<RailPoint> Rail = new List<RailPoint>();
            Rail.Add(Start);
            return AddRail(Rail);
        }

        /// <summary>
        /// Метод добавления новой рельсы
        /// Перегрузка для добавления с заданным айди. Возвращает, была ли добавлена рельса
        /// </summary>
        /// <returns></returns>
        public bool AddRail(int ID, List<RailPoint> Data){
            bool result = RailExists(ID);
            if(!result){
                Rails.Add(ID,Data);
                AdaptCount(ID);
            }
            return !result;
        }

        /// <summary>
        /// Метод добавления новой рельсы
        /// Перегрузка для добавления рельсы из одной точки с заданным айди
        /// возвращает, была ли добавлена рельса
        /// </summary>
        /// <param name="Start">Эта самая одна точка</param>
        /// <returns></returns>
        public bool AddRail(int ID, RailPoint Start){
            List<RailPoint> Rail = new List<RailPoint>();
            Rail.Add(Start);
            return AddRail(ID,Rail);
        }

        /// <summary>
        /// Метод удаления рельсы из общего массива
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveRail(int ID){
            Rails.Remove(ID);
        }
    
    }
}
