using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics{

    /// <summary>
    /// Базовый класс для обработки физических взаимодействий
    /// </summary>
    public class ForceHandler<T> where T: struct{

        /// <summary>
        /// ссылка на данные, с которыми класс будет работать
        /// </summary>
        readonly Dictionary<int,List<RailPoint>> Rails;

        /// <summary>
        /// Словарь, связывающий айди рельсы и данный для нужной силы
        /// </summary>
        readonly Dictionary<int,T> ForceData = new Dictionary<int, T>();

        public ForceHandler(Dictionary<int,List<RailPoint>> rails){
            Rails = rails;
        }
        
        /// <summary>
        /// Добавление данных силового взаимодействия. Нет проверки на наличие записи в рельсах.
        /// </summary>
        /// <param name="ID">Идентификатор нужной рельсы</param>
        /// <param name="forceData">Данные, которыве надо добавить</param>
        public void AddForceData(int ID, T forceData){
            ForceData.Add(ID,forceData);
        }

        /// <summary>
        /// Удаление данных силового взаимодействия с заданным идентификатором
        /// </summary>
        /// <param name="ID"></param>
        public void RemoveForceData(int ID){
            ForceData.Remove(ID);
        }

        /// <summary>
        /// Проверка, если рельса с таким айди существует.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RailExists(int ID){
            return Rails.ContainsKey(ID);
        }

        /// <summary>
        /// Проверка, если для данного айди существуют данные силового взаимодействия
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool DataExists(int ID){
            return ForceData.ContainsKey(ID);
        }

        /// <summary>
        /// Метод для очистки неиспользуемых записей словаря
        /// </summary>
        public void ClearUnusedData(){
            foreach (var ID in ForceData.Keys){
                if(!RailExists(ID)){
                    RemoveForceData(ID);
                }
            }
        }
    }

    /// <summary>
    /// Структура, отвечающая за передачу параметров силы гравитации
    /// </summary>
    public struct GravityData{

    }

    public class GravityHandler: ForceHandler<GravityData>{
        public GravityHandler(Dictionary<int,List<RailPoint>> rails):base(rails){

        }
    }
}