using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CustomPhysics
{   
    /// <summary>
    /// Родительский класс для серии классов, обрабатывающих рельсовый массив
    /// </summary>
    public class RailDictOperator{
        
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

        public RailDictOperator(Dictionary<int,List<RailPoint>> Orig){
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

        public ParamModifier(Dictionary<int,List<RailPoint>> rails,float timeInterval) : base(rails){
            TimeInterval = timeInterval;
        }

        
        /// <summary>
        /// Временной интервал между точками
        /// </summary>
        public readonly float TimeInterval;

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

        public RailDraw(Dictionary<int,List<RailPoint>> rails) : base(rails){
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
    }

}
