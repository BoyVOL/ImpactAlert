using Godot;
using System.Collections;
using RailSystem;
using System;
using System.Threading;

namespace CollisionCalculation{

    /// <summary>
    /// Класс для глобальной обработки коллизий всех добавленных коллайдеров
    /// </summary>
    public class GlobalCollider{
        struct MultithreadedData{
            public int id1;

            public int id2;

            public int From;
        }
        /// <summary>
        /// Массив коллайдеров, которые обрабатывает данный объект
        /// </summary>
        /// <returns></returns>
        ArrayList Colliders = new ArrayList();

		/// <summary>
		/// Свойство, контроллирующее, чтобы все этапы обновления рельсового массива свершились
		/// </summary>
		CountdownEvent AllStepsCompleted;

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

        void ProcessCollisions(int ID1, int ID2 , int From){
            lock(Colliders[ID1]) lock(Colliders[ID2]){
                int CollId = From;
                RailCollider Coll1 = (RailCollider)Colliders[ID1];
                RailCollider Coll2 = (RailCollider)Colliders[ID2];
                    float[] Time = Coll1.CollisionCheck(Coll2,CollId);
                    while (Time.Length > 0 && CollId < Coll1.Current.GetCount() && CollId < Coll2.Current.GetCount())
                    {
                        //Проверяем, если индексы совпадают
                        if(Coll1.Current.IDFromTime(Time[0]) > CollId){  
                            CollId = Coll1.Current.IDFromTime(Time[0]);
                        } else CollId++;
                        //Сохраняем индекс обработанного столкновения
                        CollisionResults Result1 = Coll1.CollisionRes(Coll2,Time[0]);
                        CollisionResults Result2 = Coll2.CollisionRes(Coll1,Time[0]);
                        Coll1.ApplyResults(Result1);
                        Coll2.ApplyResults(Result2);

                        //Обрабатываем начиная с сохранённого индекса
                        if(CollId < Coll1.Current.GetCount()-1 && CollId < Coll2.Current.GetCount()-1){
                            Time = Coll1.CollisionCheck(Coll2,CollId);
                        }
                        else Time = new float[0];
                    }
            }
        }

        void MultithreadedCollidionHandle(object Data){
            MultithreadedData Temp = (MultithreadedData)Data;
            ProcessCollisions(Temp.id1,Temp.id2,Temp.From);
            AllStepsCompleted.Signal();
        }

        public void GlobalCollProcess(int From){ 
            int ThreadCount = 0;
            for (int i = 0; i < Colliders.Count; i++)
            {
                for (int j = i+1; j < Colliders.Count; j++)
                {
                    ThreadCount++;
                }
            }
            AllStepsCompleted = new CountdownEvent(ThreadCount);
            for (int i = 0; i < Colliders.Count; i++)
            {
                for (int j = i+1; j < Colliders.Count; j++)
                {
                    MultithreadedData Temp = new MultithreadedData();
                    Temp.id1 = i;
                    Temp.id2 = j;
                    Temp.From = From;
                    MultithreadedCollidionHandle(Temp);
                }
            }
			AllStepsCompleted.Wait();
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
                if(From >= Current.GetCount() && From >= Other.Current.GetCount()) throw new Exception("From index is out of bounds");
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