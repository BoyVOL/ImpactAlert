using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CustomPhysics
{   
    /// <summary>
    /// Класс, хранящий данные по общему времени жизни рельсы
    /// </summary>
    public class RailTimeController{
        /// <summary>
        /// Временной интервал между точками в миллисекундах
        /// </summary>
        public readonly int TimeInterval;

        /// <summary>
        /// Время в миллисекундах
        /// </summary>
        long RailTime = 0;

        /// <summary>
        /// Время буффера рельсы в миллисекундах
        /// </summary>
        long BufferTime = 0;

        /// <summary>
        /// Конструктор с параметром, задающий начальные параметры контроллера
        /// </summary>
        /// <param name="timeInterval"></param>
        public RailTimeController(int timeInterval){
            TimeInterval = timeInterval;
        }

        /// <summary>
        /// Метод, увеличивающий серверное время на величину интервала
        /// </summary>
        public void ServerTick(){
            RailTime += TimeInterval;
        }

        /// <summary>
        /// Метод, увеличивающий клиентскую часть на выбранную величину
        /// </summary>
        /// <param name="amount">величина увеличения времени в миллисекундах</param>
        public void IncreaseBuffTime(int amount){
            BufferTime += amount;
        }

        /// <summary>
        /// Метод, возвращающий время внутри рельс в миллисекундах
        /// </summary>
        /// <returns></returns>
        public long GetServerTime(){
            return RailTime;
        }

        /// <summary>
        /// Метод, возвращающий время буффера в миллисекундах
        /// </summary>
        /// <returns></returns>
        public long GetBufferTime(){
            return BufferTime;
        }
                  
        /// <summary>
        /// Метод, возвращающий индекс точки в выбранном моменте времени Т
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public int IDFromTime(float T){
            return (int)(T/TimeInterval);
        }
    }

    /// <summary>
    /// Родительский класс для серии классов, обрабатывающих рельсовый массив
    /// </summary>
    public class RailDictOperator{
        
        /// <summary>
        /// Объект, отвечающий за обработку времени рельсы
        /// </summary>
        public readonly RailTimeController TimeControl;
        
        /// <summary>
        /// Ссылка на словарь рельс, с которым класс работает
        /// </summary>
        protected readonly Dictionary<int,List<RailPoint>> Rails;
                
        /// <summary>
        /// Метод для проверки рельсы на наличие в системе
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RailExists(int ID){
            return Rails.ContainsKey(ID);
        }

        public RailDictOperator(Dictionary<int,List<RailPoint>> Orig, RailTimeController timeControl){
            TimeControl = timeControl;
            Rails = Orig;
        }
        
        /// <summary>
        /// Метод для отображения рельсы с выбранным индексом
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public string StringifyRail(int ID){
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
            return Result;
        }

        /// <summary>
        /// Отображение всех рельс в массиве
        /// </summary>
        public void StringifyAllRails(){
            foreach (var ID in Rails.Keys)
            {
                GD.Print("Rail ID = ",ID);
                GD.Print(StringifyRail(ID));
            }
        }
    }

    /// <summary>
    /// Базовый класс для модификации рельсы с параметрами
    /// </summary>
    public class ParamModifier<T> : RailDictOperator where T: class{

        /// <summary>
        /// Словарь, связывающий айди рельсы и данный для нужной силы
        /// </summary>
        protected readonly Dictionary<int,List<T>> Params = new Dictionary<int,List<T>>();

        public ParamModifier(Dictionary<int,List<RailPoint>> rails,RailTimeController timeController) : base(rails,timeController){
        }

        /// <summary>
        /// Метод для вычисления изменений
        /// </summary>
        /// <param name="Position"></param>
        public virtual void CalculateChanges(int Position){
            GD.Print("Calculating Modifier +++++");
        }

        /// <summary>
        /// Метод для применения изменений рельсы
        /// </summary>
        /// <param name="Position"></param>
        public virtual void ApplyChanges(int Position){
            GD.Print("Applying Modifier -----");
        }
                
        /// <summary>
        /// Метод для определения, имеется ли для заданного объекта запись параметров взаимодействия
        /// </summary>
        /// <param name="ID">индекс рельсы, которую надо проверить</param>
        /// <returns></returns>
        public bool HasParams(int ID){
            return Params.ContainsKey(ID);
        }
    }

    /// <summary>
    /// Родительский класс для семейства классов, отрисовывающих в соответствии с рельсами информацию на экране
    /// </summary>
    /// <typeparam name="T">структура параметров отрисовки</typeparam>
    /// <typeparam name="N">класс типа Node2D, который требуется отрисовывать с рельсой</typeparam>
    public class RailDraw<T>: RailDictOperator where T: class{

        public readonly Dictionary<int,T> DrawParams = new Dictionary<int,T>();

        public RailDraw(Dictionary<int,List<RailPoint>> rails,RailTimeController timeController) : base(rails,timeController){
        }
        
        public virtual void Redraw(int ID){

        }

        /// <summary>
        /// Метод для перерисовки графических объектов в соответствии с рельсой
        /// </summary>
        public virtual void Redraw(){
            foreach (int ID in Rails.Keys)
            {
                Redraw(ID);
            }
        }
                        
        /// <summary>
        /// Метод для определения, имеется ли для заданного объекта запись параметров взаимодействия
        /// </summary>
        /// <param name="ID">индекс рельсы, которую надо проверить</param>
        /// <returns></returns>
        public bool HasParams(int ID){
            return DrawParams.ContainsKey(ID);
        }
    }

}
