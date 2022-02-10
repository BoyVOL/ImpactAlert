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
        /// Буфферный массив для хранения очередей, в которые записываются результаты коллизий для каждого объекта
        /// </summary>
        Queue[] ResultsArray;

        public int GetCount(){
            return Colliders.Count;
        }

        /// <summary>
        /// Метод для добавления коллайдера в общий пул
        /// </summary>
        /// <param name="Collider"></param>
        public void AddCollider(RailCollider Collider){
            Colliders.Add(Collider);
        }
     
        struct ThreadData{
            
            public int id;
            
            public int CurrentID;

        }

        /// <summary>
        /// Метод, заполняющий массив очередей результатов коллизий новыми записями.
        /// Безопасен для ассинхронного вызова
        /// </summary>
        /// <param name="ID">Индекс объекта, который считает данные</param>
        /// <param name="Step"></param>
        void CalcResMultAsync(int ID, int Step){
            for (int i = ID+1; i < Colliders.Count; i++)
            {
                if(ID != i){
                    RailCollider Coll1 = (RailCollider)Colliders[ID];
                    RailCollider Coll2 = (RailCollider)Colliders[i];
                    float[] Time = Coll1.CollisionCheck(Coll2,Step,Step);
                    if (Time.Length > 0)
                    {
                        lock(ResultsArray[ID]) lock(ResultsArray[i]){
                            ResultsArray[ID].Enqueue(Coll1.CollisionRes(Coll2,Time[0]));
                            ResultsArray[i].Enqueue(Coll2.CollisionRes(Coll1,Time[0]));
                        }
                    }
                }
            }
        }

        void CalcResMultAsync(object data){
            ThreadData Data = (ThreadData) data;
            CalcResMultAsync(Data.id,Data.CurrentID);
            AllStepsCompleted.Signal();
        }

        void ApplyResultAsync(int ID, int Step){
            RailCollider Coll = (RailCollider)Colliders[ID];
                while(ResultsArray[ID].Count > 0){
                    CollisionResults Result = (CollisionResults)ResultsArray[ID].Dequeue();
                    Coll.ApplyResults(Result);
                    //Coll.Current.Extrapolate(DeleteCount);
                }
        }

        void ApplyResultAsync(object obj){
            ThreadData Data = (ThreadData)obj;
            ApplyResultAsync(Data.id,Data.CurrentID);
            AllStepsCompleted.Signal();
        }

        /// <summary>
        /// Метод, настраивающий размерность буфферных массивов
        /// </summary>
        public void BufferSetup(){
            ResultsArray = new Queue[Colliders.Count];
            for (int i = 0; i < ResultsArray.Length; i++)
            {
                ResultsArray[i] = new Queue();
            }
        }

        public void GlobalCalcResults(int From, int To){
            for (int k = From; k <= To; k++)
            {
                AllStepsCompleted = new CountdownEvent(Colliders.Count);
                for (int i = 0; i < Colliders.Count; i++)
                {
                    ThreadData Data = new ThreadData();
                    Data.id = i;
                    Data.CurrentID = k;
                    ThreadPool.QueueUserWorkItem(CalcResMultAsync,Data);
                }
                AllStepsCompleted.Wait();
            }
        }

        public void GlobalApplyResults(int From, int To){
            for (int k = From; k <= To; k++)
            {
                AllStepsCompleted = new CountdownEvent(Colliders.Count);
                for (int i = 0; i < Colliders.Count; i++)
                {
                    ThreadData Data = new ThreadData();
                    Data.id = i;
                    Data.CurrentID = k;
                    ThreadPool.QueueUserWorkItem(ApplyResultAsync,Data);
                }
                AllStepsCompleted.Wait();
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
        /// Перегрузка для проверки всей рельсы
        /// </summary>
        /// <param name="Other"></param>
        /// <param name="From"></param>
        /// <returns></returns>
        public virtual float[] CollisionCheck(RailCollider Other){
            return CollisionCheck(Other,0);
        }

        /// <summary>
        /// Метод, возвращающий все моменты времени, в которых две рельсы сойдутся на расстояние меньше чем радиус столкновения обоих тел
        /// Перегрузка для проверки до конца рельсы
        /// </summary>
        /// <param name="Other"></param>
        /// <param name="From"></param>
        /// <returns></returns>
        public virtual float[] CollisionCheck(RailCollider Other, int From){
            return CollisionCheck(Other,From,Current.GetCount()-1);
        }

        /// <summary>
        /// Метод, возвращающий все моменты времени, в которых две рельсы сойдутся на расстояние меньше чем радиус столкновения обоих тел
        /// </summary>
        /// <param name="Other">Коллайдер, с которым надо проверить столкновение</param>
        /// <param name="From">Индекс, начиная с которого будет идти проверка</param>
        /// <param name="To">последний проверяемый индекс</param>
        /// <returns></returns>
        public virtual float[] CollisionCheck(RailCollider Other, int From, int To){
            if(Current != null){
                if(From >= Current.GetCount() && From >= Other.Current.GetCount()) throw new Exception("From index is out of bounds");
                if(To >= Current.GetCount() && To >= Other.Current.GetCount()) throw new Exception("To index is out of bounds");
                return Current.Approach(Other.Current,Radius+Other.Radius,From,To);
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
        public abstract bool ApplyResults(CollisionResults Results);
    }
}