using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CustomPhysics
{

    public struct CollisionParams{
        public float Radius;
    }

    public class CollisionCalculator: ParamModifier<CollisionParams>{

        public CollisionCalculator(Dictionary<int,List<RailPoint>> rails) : base(rails){
        }

        
    }
}