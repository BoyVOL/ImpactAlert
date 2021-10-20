using Godot;
using System.Collections;
using RailSystem;
using System;

namespace CollisionCalculation{

    public class CollisionHandler{

        ArrayList Colliders = new ArrayList();
    }

    public struct CollisionResults{

        public Vector2 NewPos;

        public Vector2 NewSpeed;

        public Vector2 NewAccel;

        public float T;
    }

    /// <summary>
    /// Класс, который может сталкивать рельсу, которая имеется у него в свойствах, с другими
    /// </summary>
    public abstract class RailCollider{
        
        /// <summary>
        /// Рельса, которая в текущий момент назначенная объекту
        /// </summary>
        public Rail Current = null;

        /// <summary>
        /// Поле, отвечающее за радиус, начиная с которого столкновение будет обрабатываться
        /// </summary>
        public float Radius = 0;

        /// <summary>
        /// Метод, возвращающий все моменты времени, в которых две рельсы сойдутся на расстояние меньше чем радиус столкновения обоих тел
        /// </summary>
        /// <param name="Other"></param>
        /// <param name="From"></param>
        /// <returns></returns>
        public virtual float[] CollisionCheck(RailCollider Other, int From = 0){
            if(Current != null){
                return Current.Approach(Other.Current,Radius+Other.Radius,From,Current.GetCount()-1);
            } else throw new Exception("You're trying to use Collider without assotiated rail");
        }

        /// <summary>
        /// Метод, обрабатывающий столкновение с другой рельсой на протяжении всего пути и возвращает массив коллизий
        /// </summary>
        /// <param name="Other">Рельса, с которой обрабатывается столкновение</param>
        /// <param name="T">Момент времени, в который надо обработать столкновение</param>
        public abstract CollisionResults CollisionRes(Rail Other, float T);

        /// <summary>
        /// Метод для применения результатов коллизии к рельсе коллайдера
        /// </summary>
        /// <param name="Results">Результаты коллизии, которые надо применить</param>
        public abstract void ApplyResults(CollisionResults Results);
    }
}