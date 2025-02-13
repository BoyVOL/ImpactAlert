using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics{

    /// <summary>
    /// Структура, отвечающая за передачу параметров силы гравитации
    /// </summary>
    public class GravityData{

    }

    public class GravityHandler: ParamModifier<GravityData>{
        public GravityHandler(Dictionary<int,List<RailPoint>> rails, RailTimeController timeController) : base(rails, timeController){
        }
    }
}