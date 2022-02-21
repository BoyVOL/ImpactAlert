using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

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
        /// Поле, ответственное за угловое ускорение в данной точке рельсы
        /// </summary>
        public float RotAccel;        

        /// <summary>
        /// Метод, возвращающий новую точку, основываясь на данных старой.
        /// </summary>
        /// <param name="Point">Предыдущая точка</param>
        /// <param name="T">интервал времени, на котором проходит симуляция</param>
        /// <returns></returns>
        public RailPoint GetNextPoint(float T){
            RailPoint Result = new RailPoint();
            Result.Position = Position+Speed*T+(Acceleration*T*T)/2;
            Result.Rotation = Rotation+RotSpeed*T+(RotAccel*T*T)/2;
            Result.Speed = Speed+Acceleration*T;
            Result.RotSpeed = RotSpeed+RotAccel*T;
            Result.Acceleration = Acceleration;
            Result.RotAccel = RotAccel;
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

        public RailPoint(RailPoint Other){
            Position = Other.Position;
            Rotation = Other.Rotation;
            Speed = Other.Speed;
            RotSpeed = Other.RotSpeed;
            Acceleration = Other.Acceleration;
            RotAccel = Other.RotAccel;
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
        /// Объект для блокировки последующих вызовов обновления рельс, пока текущее обновление не завершится
        /// </summary>
        /// <returns></returns>
        CountdownEvent UpdateLock = new CountdownEvent(0);

        /// <summary>
        /// Поток для выполнения обновления рельс
        /// </summary>
        System.Threading.Thread UpdateThread;

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
        /// Словарь массивов, отображающих копии рельс для чтения, который копируется из основного при каждом обновлении
        /// </summary>
        Dictionary<int,List<RailPoint>> RailBuff = new Dictionary<int, List<RailPoint>>();

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
            Result += "Buffer rail = \n";
            foreach (var item in RailBuff[ID])
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

        /// <summary>
        /// Метод для ассинхронного обновления массива рельс
        /// </summary>
        void AsyncUpdate(){
            GD.Print("Update function has been started");
            System.Threading.Thread.Sleep(5000);
            AdaptCount();
            MoveForwardAll();
            GD.Print("Update function has been Finished");
            UpdateLock.Signal();
        }

        public bool RailExists(int ID){
            return Rails.ContainsKey(ID);
        }

        /// <summary>
        /// Метод для обрезки количества элементов отдельно взятой рельсы к максимальному числу элементов
        /// </summary>
        /// <param name="ID"></param>
        void CutToMax(int ID){
            int CountDiff = Rails[ID].Count - RailSize;
            if(CountDiff > 0){
                RemoveFromEnd(ID,CountDiff);
            }
        }

        /// <summary>
        /// Метод для обрезки всех рельс до максимального значения
        /// </summary>
        void CutToMaxAll(){
            foreach (var ID in Rails.Keys)
            {
                CutToMax(ID);
            }
        }

        /// <summary>
        /// Метод для расширения рельс до общего количество поэтапно
        /// </summary>
        void ExpandAll(){
            for (int i = 0; i < RailSize; i++)
            {
                AddAtIndex(i);
            }
        }

        /// <summary>
        /// Метод для подстройки количества элементов в рельсе по указанному индексу под общее число элементов 
        /// </summary>
        /// <param name="ID"></param>
        void AdaptCount(){
            CutToMaxAll();
            ExpandAll();
        }

        /// <summary>
        /// Метод для добавления нового элемента на выбранном местоположении
        /// </summary>
        /// <param name="Position">Порядковый индекс точки, которую надо добавить</param>
        void AddAtIndex(int Position){
            foreach (var ID in Rails.Keys)
            {
                //Проверка, чтобы индекс последнего элемента рельсы строго был на один ниже нового индекса
                if(Rails[ID].Count == Position){   
                    Rails[ID].Add(Rails[ID][Position-1].GetNextPoint(TimeInterval));
                }
            }
        }

        /// <summary>
        /// Метод для копирования рельсы с выбранным индексом полностью в массив чтения
        /// </summary>
        /// <param name="ID"></param>
        void CopyRailToRead(int ID){
            RailBuff.Add(ID,new List<RailPoint>());
            foreach (var item in Rails[ID])
            {
                //Для каждого элемента родительского списка добавляем новый элемент
                RailBuff[ID].Add(new RailPoint(item));
            }
        }

        /// <summary>
        /// Метод для обновления буффера чтения данных рельс
        /// </summary>
        void UpdateReadBuffer(){
            //Очищаем массив
            RailBuff = new Dictionary<int, List<RailPoint>>();
            foreach (var ID in Rails.Keys)
            {
                CopyRailToRead(ID);
            }
        }

        /// <summary>
        /// Метод для удаления одного начального элемента всех рельс
        /// </summary>
        void DeleteStart(){
            foreach (var ID in Rails.Keys)
            {
                Rails[ID].RemoveAt(0);
            }
        }

        /// <summary>
        /// Метод для движения разом всех рельс
        /// </summary>
        void MoveForwardAll(){
            int NewID = RailSize-1;
            DeleteStart();
            AddAtIndex(NewID);
        }

        /// <summary>
        /// Метод, позволяющий вызывающему потоку подождать, пока не закончится поток обновления данного массива
        /// </summary>
        public void WaitForUpdate(){
            UpdateLock.Wait();
        }

        /// <summary>
        /// Метод для обновления состояния рельсы
        /// </summary>
        public void Update(){
            WaitForUpdate();
            UpdateReadBuffer();
            UpdateThread = new System.Threading.Thread(AsyncUpdate);
            UpdateThread.Start();
            UpdateLock = new CountdownEvent(1);
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
                CopyRailToRead(ID);
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
