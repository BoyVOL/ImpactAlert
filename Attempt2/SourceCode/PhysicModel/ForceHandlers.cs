using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics{

    /// <summary>
    /// Базовый класс для обработки физических взаимодействий
    /// </summary>
    public class PhysProperty<T> where T: struct{

        /// <summary>
        /// ссылка на данные, с которыми класс будет работать
        /// </summary>
        readonly Dictionary<int,List<RailPoint>> Rails;

        /// <summary>
        /// Словарь, связывающий айди рельсы и данный для нужной силы
        /// </summary>
        readonly Dictionary<int,List<T>> ForceData = new Dictionary<int,List<T>>();

        public PhysProperty(Dictionary<int,List<RailPoint>> rails){
            Rails = rails;
        }

    }

    /// <summary>
    /// Структура, отвечающая за передачу параметров силы гравитации
    /// </summary>
    public struct GravityData{

    }

    public class GravityHandler: PhysProperty<GravityData>{
        public GravityHandler(Dictionary<int,List<RailPoint>> rails):base(rails){

        }
    }
}