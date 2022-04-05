using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CustomPhysics
{   
    /// <summary>
    /// Класс для обработки запросов на удаление и добавление рельс
    /// </summary>
    public class DictBatchLoader: RailDictOperator{

        /// <summary>
        /// Объект для случайной генерации идентификаторов
        /// </summary>
        /// <returns></returns>
        Random IDGen = new Random();

        /// <summary>
        /// Словарь для добавления новых рельс
        /// </summary>
        Dictionary<int,List<RailPoint>> RailsToAdd = new Dictionary<int, List<RailPoint>>();

        /// <summary>
        /// Очередь для удаления рельс из словаря
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <returns></returns>
        Queue<int> DeleteQueue = new Queue<int>();

        /// <summary>
        /// Конструктор для данного класса
        /// </summary>
        /// <param name="OrigArray">Массив, который требуется изменять</param>
        public DictBatchLoader(Dictionary<int,List<RailPoint>> Orig,RailTimeController timeController) : base(Orig,timeController){
        }

        /// <summary>
        /// Метод, который возвращает случайный свободный айди в словаре
        /// </summary>
        /// <returns></returns>
        public int GetFreeID(){
            int ID = IDGen.Next();
            while(RailExists(ID)){
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
        /// Метод для проверки рельсы на наличие в системе
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        new public bool RailExists(int ID){
            return base.RailExists(ID) || RailsToAdd.ContainsKey(ID);
        }

        /// <summary>
        /// Метод добавления новой рельсы
        /// Перегрузка для добавления с заданным айди. Возвращает, была ли добавлена рельса
        /// </summary>
        /// <returns></returns>
        public bool AddRail(int ID, List<RailPoint> Data){
            bool result = RailExists(ID);
            if(!result){
                RailsToAdd.Add(ID,Data);
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
            DeleteQueue.Enqueue(ID);
        }

        /// <summary>
        /// Метод для применения изменений
        /// </summary>
        public void Sync(){
            //GD.Print("Sync");
            while (DeleteQueue.Count > 0)
            {
                Rails.Remove(DeleteQueue.Dequeue());
            }
            foreach (var Key in RailsToAdd.Keys)
            {
                Rails.Add(Key,RailsToAdd[Key]);
            }
            RailsToAdd.Clear();
        }
    }

    /// <summary>
    /// Класс для буффера чтения рельс
    /// </summary>
    public class ReadBuffer: RailDictOperator{

        /// <summary>
        /// Словарь массивов, отображающих копии рельс для чтения, который копируется из основного при каждом обновлении
        /// </summary>
        public readonly Dictionary<int,List<RailPoint>> RailBuff = new Dictionary<int, List<RailPoint>>();

        public ReadBuffer(Dictionary<int,List<RailPoint>> Orig, RailTimeController timeController) : base(Orig,timeController){
        }
                
        /// <summary>
        /// Метод для проверки рельсы на наличие в буффере
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RailBuffered(int ID){
            return RailBuff.ContainsKey(ID);
        }

        /// <summary>
        /// Метод для копирования рельсы с выбранным индексом полностью в массив чтения
        /// </summary>
        /// <param name="ID"></param>
        void CopyRail(int ID){
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
            RailBuff.Clear();
            foreach (var ID in Rails.Keys)
            {
                CopyRail(ID);
            }
        }

        /// <summary>
        /// Метод для синхронизации данного буфера и основного массива
        /// </summary>
        public virtual void Sync(){
            UpdateReadBuffer();
        }
    }

    /// <summary>
    /// Новый буффер со списком отрисовщиков, которые автоматически обновляются
    /// </summary>
    public class DrawBuffer: ReadBuffer{

        public readonly SpriteDraw SpriteDraw;
        
        public DrawBuffer(Dictionary<int,List<RailPoint>> Orig,RailTimeController timeController) : base(Orig,timeController){
            SpriteDraw = new SpriteDraw(RailBuff,timeController);
        }

        /// <summary>
        /// Метод для перерисовки всех отрисовщиков в списке
        /// </summary>
        void RedrawAll(){
            SpriteDraw.Redraw();
        }

        public override void Sync()
        {
            base.Sync();
            RedrawAll();
        }
    }

    /// <summary>
    /// Класс для хранения размера рельсы
    /// </summary>
    public class RailLengthAdapter: RailDictOperator{
                
        /// <summary>
        /// Размер рельс в данном классе
        /// </summary>
        public readonly int RailSize;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Orig">Словарь, который надо обрабатывать</param>
        /// <param name="size">размер рельсы</param>
        /// <param name="timeInterval">временной интервал обработки рельсы</param>
        /// <returns></returns>
        public RailLengthAdapter(Dictionary<int,List<RailPoint>> Orig, int size, RailTimeController timeController) : base(Orig,timeController){
            RailSize = size;
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
            for (int i = 1; i < RailSize; i++)
            {
                if(CheckAdapt(i)){
                    AddAllAtIndex(i);
                }
            }
        }

        /// <summary>
        /// Метод для подстройки количества элементов в рельсе по указанному индексу под общее число элементов 
        /// </summary>
        /// <param name="ID"></param>
        public void AdaptCount(){
            CutToMaxAll();
            ExpandAll();
        }

        /// <summary>
        /// Метод для проверки, если есть рельсы, требующие корректировки на заданной позиции
        /// </summary>
        /// <param name="Position">Индекс, на котором надо провести проверку</param>
        /// <returns></returns>
        bool CheckAdapt(int Position){
            bool Result = false; 
            foreach (int ID in Rails.Keys)
            {
                // Логическое сложение, если выбранный индекс меньше последнего индекса рельсы
                Result |= Rails[ID].Count <= Position;
            }
            return Result;
        }

        /// <summary>
        /// Метод для добавления элемента рельсы по указанному индексу, если есть предыдущий
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Position"></param>
        void AddAtIndex(int ID, int Position){
            //Проверка, чтобы индекс последнего элемента рельсы строго был на один ниже нового индекса
            if(Rails[ID].Count == Position){   
                Rails[ID].Add(Rails[ID][Position-1].GetNext(TimeControl.TimeInterval));
            }
        }

        /// <summary>
        /// Метод для добавления нового элемента на выбранном местоположении всех рельс, у которых на данном индексе нет элемента, но есть предыдущий
        /// </summary>
        /// <param name="Position">Порядковый индекс точки, которую надо добавить</param>
        protected virtual void AddAllAtIndex(int Position){
            foreach (int ID in Rails.Keys)
            {
                AddAtIndex(ID,Position);
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
        /// Метод для движения разом всех рельс вперёд на 1
        /// </summary>
        public void MoveForwardAll(){
            int NewID = RailSize-1;
            DeleteStart();
            AddAllAtIndex(NewID);
        }
    }

    /// <summary>
    /// Дочерний класс, содержащий свойства для изменения длины массива
    /// </summary>
    public class ModLengthAdapter: RailLengthAdapter{

        public readonly CollisionCalculator CollMod;
        
        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="Orig">Словарь, который надо обрабатывать</param>
        /// <param name="size">размер рельсы</param>
        /// <param name="timeInterval">временной интервал обработки рельсы</param>
        /// <returns></returns>
        public ModLengthAdapter(Dictionary<int,List<RailPoint>> Orig, int size, RailTimeController timeController) : base(Orig,size,timeController){
            CollMod = new CollisionCalculator(Orig,timeController);
        }
        
        /// <summary>
        /// Метод для вычисления необходимых модификаций на заданном индексе
        /// </summary>
        /// <param name="Position">индекс, на котором надо вычислить модификации</param>
        void CalcModifiers(int Position){
            CollMod.CalculateChanges(Position);
        }

        /// <summary>
        /// Метод для применения необходимых модификаций на заданном индексе
        /// </summary>
        /// <param name="Position">Индекс, на котором надо применить изменения</param>
        void ApplyModifiers(int Position){
            CollMod.ApplyChanges(Position);
        }

        protected override void AddAllAtIndex(int Position)
        {
            CalcModifiers(Position-1);
            ApplyModifiers(Position);
            base.AddAllAtIndex(Position);
        }
    }

    /// <summary>
    /// Класс для хранения и обработки рельс
    /// </summary>
    public class MainRailArray: RailDictOperator{

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
        /// Класс для обработки запросов на изменение списка рельс
        /// </summary>
        public readonly DictBatchLoader Edit;

        /// <summary>
        /// Объект, содержащий буффер чтения для массива рельс
        /// </summary>
        public readonly DrawBuffer RBuffer;

        /// <summary>
        /// Класс для адаптирования и продвижения рельс
        /// </summary>
        public readonly ModLengthAdapter MLAdapter;

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="size">Размер рельс</param>
        /// <param name="TimeInterval">Интервал интерполяции</param>
        public MainRailArray(int size, RailTimeController timeController): base(new Dictionary<int, List<RailPoint>>(),timeController){
            RBuffer = new DrawBuffer(Rails,timeController);
            Edit = new DictBatchLoader(Rails,timeController);
            MLAdapter = new ModLengthAdapter(Rails,size,timeController);
        }

        new public string StringifyRail(int ID){
            string Result = "Rail = \n";
            if(RailExists(ID)){
                foreach (var item in Rails[ID])
                {
                    Result += item.Stringify()+"\n";
                }
            } else {
                Result += "Does ont exist";
            }
            Result += "Buffer rail = \n";
            if(RBuffer.RailBuffered(ID)){
                foreach (var item in RBuffer.RailBuff[ID])
                {
                    Result += item.Stringify()+"\n";
                }
            } else {
                Result += "Is not buffered";
            }
            return Result;
        }

        /// <summary>
        /// Метод для ассинхронного обновления массива рельс
        /// </summary>
        void AsyncUpdate(){
            //GD.Print("Update function has been started");
            //System.Threading.Thread.Sleep(5000);
            Edit.Sync();
            MLAdapter.AdaptCount();
            MLAdapter.MoveForwardAll();
            //GD.Print("Update function has been Finished");
            UpdateLock.Signal();
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
            TimeControl.ServerTick();
            RBuffer.Sync();
            UpdateLock = new CountdownEvent(1);
            UpdateThread = new System.Threading.Thread(AsyncUpdate);
            UpdateThread.Start();
        }

        /// <summary>
        /// Метод, отвечающий за обновление абстрагированных буффером элементов
        /// </summary>
        /// <param name="time">количество времени в миллисекундах, на которое продвинется вперёд обновление</param>
        public void SafeUpdate(int time){
            TimeControl.IncreaseBuffTime(time);
            GD.Print("delta = ",time);
            GD.Print(TimeControl.GetBufferTime());
        }
    }

}
