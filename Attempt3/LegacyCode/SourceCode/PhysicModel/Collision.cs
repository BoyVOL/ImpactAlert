using Godot;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CustomPhysics
{

    public class CollisionParams{
        public float Radius;
    }
    
    /// <summary>
    /// Класс для обработки коллизий
    /// </summary>
    public class CollisionCalculator: ParamModifier<CollisionParams>{
        
        /// <summary>
        /// Поле для хранения результатов коллизий
        /// </summary>
        /// <typeparam name="int"></typeparam>
        /// <typeparam name="RailPoint"></typeparam>
        /// <returns></returns>
        readonly Dictionary<int,RailPoint> Results = new Dictionary<int, RailPoint>();

        public CollisionCalculator(Dictionary<int,List<RailPoint>> rails, RailTimeController timeController) : base(rails,timeController){
        }

        /// <summary>
        /// Метод вычисления максимального сближения рельс на заданной позиции
        /// </summary>
        /// <param name="ID1">Идентификатор первой рельсы</param>
        /// <param name="ID2">Идентификатор второй рельсы</param>
        /// <param name="Position">положение индекса, на котором надо произвести проверку</param>
        /// <returns></returns>
        float CheckDistance(int ID1, int ID2, int Position){
            float CPA = Rails[ID1][Position].CPA(Rails[ID2][Position],TimeControl.TimeInterval);
            float Distance = Rails[ID1][Position].GetInterPos(CPA,TimeControl.TimeInterval).DistanceTo(Rails[ID2][Position].GetInterPos(CPA,TimeControl.TimeInterval));
            return Distance;            
        }

        public override void CalculateChanges(int Position)
        {
            foreach (int ID1 in Rails.Keys)
            {
                foreach (int ID2 in Rails.Keys)
                {
                    if(ID1<ID2 && HasParams(ID1) && HasParams(ID2)){
                        GD.Print("Distance "+Position+" = "+CheckDistance(ID1,ID2,Position));
                    }
                }
            }
        }
    }
}