using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics{

    /// <summary>
    /// Базовый класс для обработки физических взаимодействий
    /// </summary>
    public class ForceModifier<T> : UpdateModifier where T: struct{

        /// <summary>
        /// Словарь, связывающий айди рельсы и данный для нужной силы
        /// </summary>
        readonly Dictionary<int,List<T>> ForceData = new Dictionary<int,List<T>>();

        public ForceModifier(Dictionary<int,List<RailPoint>> rails) : base(rails){
        }

    }

    /// <summary>
    /// Структура, отвечающая за передачу параметров силы гравитации
    /// </summary>
    public struct GravityData{

    }

    public class GravityHandler: ForceModifier<GravityData>{
        public GravityHandler(Dictionary<int,List<RailPoint>> rails) : base(rails){

        }
    }
}