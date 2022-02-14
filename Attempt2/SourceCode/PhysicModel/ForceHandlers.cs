using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics{
    /// <summary>
    /// Класс для обработки физических взаимодействий
    /// </summary>
    class ForceHandler<T> where T: struct{

        /// <summary>
        /// ссылка на данные, с которыми класс будет работать
        /// </summary>
        readonly Dictionary<Guid,List<RailPoint>> Rails;

        /// <summary>
        /// Словарь, связывающий айди рельсы и данный для нужной силы
        /// </summary>
        readonly Dictionary<Guid,T> ForceData = new Dictionary<Guid, T>();

        public ForceHandler(Dictionary<Guid,List<RailPoint>> rails){
            Rails = rails;
        }
        
        /// <summary>
        /// Добавление данных силового взаимодействия. Нет проверки на наличие записи в рельсах.
        /// </summary>
        /// <param name="ID">Идентификатор нужной рельсы</param>
        /// <param name="forceData">Данные, которыве надо добавить</param>
        public void AddForceData(Guid ID, T forceData){
            ForceData.Add(ID,forceData);
        }

        /// <summary>
        /// Проверка, если рельса с таким айди существует.
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool RailExists(Guid ID){
            return Rails.ContainsKey(ID);
        }
    }
}