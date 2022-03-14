using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics{

    /// <summary>
    /// Структура, отвечающая за передачу параметров силы гравитации
    /// </summary>
    public struct GravityData{

    }

    public class GravityHandler: ParamModifier<GravityData>{
        public GravityHandler(Dictionary<int,List<RailPoint>> rails, float timeInterval) : base(rails, timeInterval){
        }
    }
}