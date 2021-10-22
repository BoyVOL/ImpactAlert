using Godot;
using System.Collections;
using RailSystem;
using System;

namespace CollisionCalculation{

    /// <summary>
    /// Класс для глобальной обработки коллизий всех добавленных коллайдеров
    /// </summary>
    public class GlobalCollider{
        /// <summary>
        /// Массив коллайдеров, которые обрабатывает данный объект
        /// </summary>
        /// <returns></returns>
        ArrayList Colliders = new ArrayList();

        /// <summary>
        /// Метод для добавления коллайдера в общий пул
        /// </summary>
        /// <param name="Collider"></param>
        public void AddCollider(RailCollider Collider){
            Colliders.Add(Collider);
        }

        /// <summary>
        /// Метод для обработки столкновений между двумя коллайдерами
        /// </summary>
        /// <param name="Coll1">Первый Коллайдер</param>
        /// <param name="Coll2">Второй Коллайдер</param>
        /// <param name="From"></param>
        void ProcessCollisions(RailCollider Coll1, RailCollider Coll2, int From){
            int CollId = From;
            float[] Time = Coll1.CollisionCheck(Coll2,CollId);
            while (Time.Length > 0)
            {
                //Сохраняем индекс обработанного столкновения
                CollId = Coll1.Current.IDFromTime(Time[0]);
                CollisionResults Result1 = Coll1.CollisionRes(Coll2,Time[0]);
                CollisionResults Result2 = Coll2.CollisionRes(Coll1,Time[0]);
                Coll1.ApplyResults(Result1);
                Coll2.ApplyResults(Result2);
                //Обрабатываем начиная с сохранённого индекса
                Time = Coll1.CollisionCheck(Coll2,CollId);
            }
        }

        void GlobalCollProcess(int From){
            for (int i = 0; i < Colliders.Count; i++)
            {
                for (int j = i+1; j < Colliders.Count; j++)
                {
                    ProcessCollisions((RailCollider)Colliders[i],(RailCollider)Colliders[j],From);
                }
            }
        }
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
        /// Метод, обрабатывающий столкновение с другой рельсой в указанном моменте времени и возвращающий результат для данного коллайдера
        /// </summary>
        /// <param name="Other">Рельса, с которой обрабатывается столкновение</param>
        /// <param name="T">Момент времени, в который надо обработать столкновение</param>
        public abstract CollisionResults CollisionRes(RailCollider Other, float T);

        /// <summary>
        /// Метод для применения результатов коллизии к данному коллайдеру
        /// </summary>
        /// <param name="Results">Результаты коллизии, которые надо применить</param>
        public abstract void ApplyResults(CollisionResults Results);
    }
}