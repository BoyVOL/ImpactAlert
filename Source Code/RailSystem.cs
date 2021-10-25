using Godot;
using System;
using System.Threading;
using System.Collections;

namespace RailSystem{
	
	/// <summary>
	/// Класс, ответственный за глобальную обработку и обновление рельс. Все рельсы с одним временным интервалом
	/// </summary>
	public class GlobalRailController{
		ArrayList Rails = new ArrayList();

		/// <summary>
		/// Свойство, контроллирующее, чтобы все этапы обновления рельсового массива свершились
		/// </summary>
		CountdownEvent AllStepsCompleted;

		/// <summary>
		/// Интервал времени всех рельс в контроллере
		/// </summary>
		float Interval = 1;

		/// <summary>
		/// Глобальное смещение времени всех рельс относительно глобального "нулевого момента времени"
		/// </summary>
		public float ShiftT = 0;

		/// <summary>
		/// Количество точек всех рельс в контроллере
		/// </summary>
		int GlobalCount = 2;

		/// <summary>
		/// Метод, возвращающий RailFollower для выбранной рельсы
		/// </summary>
		/// <param name="rail">выбранная рельса</param>
		/// <returns></returns>
		public RailFollower GetRailFollower(Rail rail){
			RailFollower Result = new RailFollower(this);
			Result.Current = rail;
			return Result;
		}

		/// <summary>
		/// Устанавливается новый интервал для всех рельс
		/// </summary>
		/// <param name="newInterval">интервал, который нужно установить</param>
		public void SetInterval(float newInterval){
			Interval = newInterval;
			foreach (Rail rail in Rails)
			{
				rail.SetInterval(newInterval);
			}
		}

		/// <summary>
		/// Возвращается интервал всех рельс
		/// </summary>
		/// <returns></returns>
		public float GetInterval(){
			return Interval;
		}

		/// <summary>
		/// Метод для подстройки рельсы к общему количеству точек
		/// </summary>
		/// <param name="rail">рельса, которую надо подстроить</param>
		public void AdaptCount(Rail rail){
			int railCount = rail.GetCount();
			if(railCount > GlobalCount){
				rail.RemoveFromEnd(railCount - GlobalCount);
			}
			if(railCount < GlobalCount){
				rail.Extrapolate(GlobalCount - railCount);
			}
		}

		/// <summary>
		/// Метод для изменения глобального количества точек
		/// </summary>
		/// <param name="newCount"></param>
		public void SetGlobalCount(int newCount){
			GlobalCount = newCount;
			foreach (Rail rail in Rails)
			{
				AdaptCount(rail);
			}
		}

		/// <summary>
		/// Возвращает глобальное количество элементов
		/// </summary>
		/// <returns></returns>
		public int GetGlobalCount(){
			return GlobalCount;
		}

		/// <summary>
		/// Метод, добавляющий рельсу в общую структуру и подстраивающий её под остальные
		/// </summary>
		/// <param name="rail"></param>
		public void AddRail(Rail rail){
			Rails.Add(rail);
			rail.SetInterval(Interval);
			AdaptCount(rail);
		}

		/// <summary>
		/// Метод, удаляющий объект
		/// </summary>
		/// <param name="rail">объект, который надо удалить</param>
		public void DeleteRail(Rail rail){
			Rails.Remove(rail);
		}
		
		void AsynqMove(object Rail){
			Rail Temp = (Rail)Rail;
			Temp.Extrapolate(1);
			Temp.RemoveFromStart(1);
			AllStepsCompleted.Signal();
		}
		
		/// <summary>
		/// Передвижение рельсы вперёд с удалением данных элементов с начала рельсы
		/// </summary>
		/// <param name="Count">Количество элементов, на которые надо сдвинуть рельсу</param>
		public void MoveForvard(int Count){
			for (int i = 0; i < Count; i++)
			{
				AllStepsCompleted = new CountdownEvent(Rails.Count);
				foreach (Rail rail in Rails)
				{
					ThreadPool.QueueUserWorkItem(AsynqMove,rail);
				}
				AllStepsCompleted.Wait();
			}
			ShiftT += Interval*Count;
		}
	}
		
	/// <summary>
	/// Класс точек рельсы, который имеет абстрактный метод для порождения дочерних экземпляров для экстраполяции
	/// </summary>
	public class RailPoint{

		/// <summary>
		/// Структура для передачи данных об интерполяции
		/// </summary>
		public struct InterData{

			/// <summary>
			/// Координаты положения в пространстве
			/// </summary>
			public Vector2 Position;

			/// <summary>
			/// угловое положение в пространстве
			/// </summary>
			public float Rotation;

			public string toString(){
				return "Position = "+Position+", Rotation = "+Rotation;
			}

			public InterData(Vector2 pos, float rot){
				Position = pos;
				Rotation = rot;
			}
		}

		/// <summary>
		/// Положение в данной точке
		/// </summary>
		public Vector2 Position = Vector2.Zero;

		/// <summary>
		/// Скорость перемещения для интерполяции
		/// </summary>
		Vector2 Speed = Vector2.Zero;

		/// <summary>
		/// Угол поворота объекта в радианах. Подразумевает знанение значений больше чем 2*Pi.
		/// </summary>
		public float Rotation = 0;

		/// <summary>
		/// Скорость поворота объекта для интерполяции
		/// </summary>
		float RotSpeed = 0;

		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public RailPoint(){

		}
		
		/// <summary>
		/// Конструктор копирования
		/// </summary>
		/// <param name="Other">Точка, с которой нужно скопировать данные</param>
		public RailPoint(RailPoint Other){
			Position = Other.Position;
			Speed = Other.Speed;
			Rotation = Other.Rotation;
			RotSpeed = Other.RotSpeed;
		}

		/// <summary>
		/// Метод, возвращающий данные о результате интерполяции в виде структуры
		/// </summary>
		/// <param name="T">Момент времени, данные в котором надо вернуть</param>
		/// <returns></returns>
		public virtual InterData GetInterpol(float T){
			return new InterData(
				Position + Speed*T,
				Rotation + RotSpeed*T
			);
		}

		/// <summary>
		/// Возвращает значение времени пересечения с указанной точкой, начиная от текущей точки.
		/// </summary>
		/// <param name="Target">Вторая точка, с которой просчитывается пересечение путей</param>
		/// <returns></returns>
		public float CPA(RailPoint Target){
			return MathExtra.cpaTime(Position,Target.Position,Speed,Target.Speed);
		}

		/// <summary>
		/// Возвращает значение времени пересечения с указанной точкой, начиная от текущей точки.
		/// Перегрузка для 2д вектора
		/// </summary>
		/// <param name="Vector">2Д вектор, с которым просчитывается пересечение путей</param>
		/// <returns></returns>
		public float CPA(Vector2 Vector){
			return MathExtra.cpaTime(Position,Vector,Speed,new Vector2(0,0));
		}

		/// <summary>
		/// Интерполяция через заданную точку в течении заданного промежутка времени
		/// </summary>
		/// <param name="Target"></param>
		/// <param name="T"></param>
		public virtual void InterpolateTo(RailPoint Target, float T){
			Speed = (Target.Position - Position) / T;
			RotSpeed = (Target.Rotation - Rotation) / T;
		}

		/// <summary>
		/// Определяет логику экстраполирования траектории движения объекта. Обязательно должен перегружаться.
		/// </summary>
		/// <param name="T">интервал времени моделирования до следующей точки</param>
		/// <returns>Новая точка на основе указанных данных</returns>
		public virtual RailPoint CreateNextPoint(float T){
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Класс точки рельсы, отвечающий за её равномерное движение
	/// </summary>
	public class KineticPoint : RailPoint{

		/// <summary>
		/// Скорость симуляции объекта
		/// </summary>
		public Vector2 SimSpeed = Vector2.Zero;

		/// <summary>
		/// Скорость вращения симуляции объекта
		/// </summary>
		public float SimRotSpeed = 0;

		public KineticPoint(){}

		/// <summary>
		/// Перегрузка конструктора копирования
		/// </summary>
		/// <param name="Other"></param>
		public KineticPoint(KineticPoint Other):base(Other){
			SimSpeed = Other.SimSpeed;
			SimRotSpeed = Other.SimRotSpeed;
		}

		public KineticPoint(Vector2 pos, float rot){
			Position = pos;
			Rotation = rot;
		}

		public KineticPoint(Vector2 pos, float rot, Vector2 simSpeed, float simRotSpeed = 0){
			Position = pos;
			Rotation = rot;
			SimSpeed = simSpeed;
			SimRotSpeed = simRotSpeed;
		}

		/// <summary>
		/// Получение следующей точки. В этой перегрузке симулируется равномерное движение объекта с заданной скоростью.
		/// </summary>
		/// <param name="T">время симуляции</param>
		/// <returns></returns>
		public override RailPoint CreateNextPoint(float T)
		{
			return new KineticPoint(Position+SimSpeed*T,Rotation+SimRotSpeed*T,SimSpeed,SimRotSpeed);
		}
	}

	/// <summary>
	/// Класс рельс, по которым объекты должны двигаться
	/// </summary>
	public class Rail {

		/// <summary>
		/// Массив точек, вдоль которых объект перемещается
		/// </summary>
		ArrayList Points = new ArrayList();

		/// <summary>
		/// Лист моментов времени для методов поисчка сближений
		/// </summary>
		/// <returns></returns>
		ArrayList ResultList = new ArrayList();

		/// <summary>
		/// Свойство, описывающее, какое количество времени тратится на перемещение между точками
		/// </summary>
		float TimeInterval = 100;

		/// <summary>
		/// Поле, описывающее смещение рельсы относительно остального мира
		/// </summary>
		public float ShiftT = 0;

		public Rail(){}

		/// <summary>
		/// Конструктор для глубокого копирования рельсы
		/// </summary>
		/// <param name="Other">другая рельса, которую надо скопировать</param>
		public Rail(Rail Other){
			for (int i = 0; i < Other.Points.Count; i++)
			{
				Points.Add(new RailPoint((RailPoint)Other.Points[i]));
			}
			TimeInterval = Other.TimeInterval;
			ShiftT = Other.ShiftT;
		}

		/// <summary>
		/// Метод установки временного интервала между точками рельсы
		/// </summary>
		/// <param name="newInterval"></param>
		public virtual void SetInterval(float newInterval){
			TimeInterval = newInterval;
			ReExtrapolate(Points.Count-1);
		}

		/// <summary>
		/// Возвращает временной интервал между точками
		/// </summary>
		/// <returns></returns>
		public virtual float GetInterval(){
			return TimeInterval;
		}
		
		/// <summary>
		/// Метод для повторного заполнения рельсы массивом точек
		/// </summary>
		/// <param name="Count">количество точек, которые надо добавить</param>
		public virtual void ReExtrapolate(int Count){
			if(Points.Count>1){
				Points.RemoveRange(1,Points.Count-1);
				Extrapolate(Count);
			}
		}
		
		/// <summary>
		/// Возвращает индект точки, которая описывает указанный момент времени
		/// </summary>
		/// <param name="T">момент времени</param>
		/// <returns>индекс соответствующей точки</returns>
		public int IDFromTime(float T){
			return (int)Math.Floor(T/TimeInterval);
		}
		
		/// <summary>
		/// Метод, возвращающий все моменты времени, в которые две рельсы оказываются столь же близко или ближе друг к другу, чем указанное расстояние
		/// </summary>
		/// <param name="OtherOne">вторая рельса, с которой надо найти все сближения</param>
		/// <param name="Distance">минимальная дистанция, которая считается как достаточное сближение</param>
		/// <returns>массив всех моментов времени, начиная с начального, в которые обе рельсы сближаются на достаточное расстояние</returns>
		public virtual float[] Approach(Rail OtherOne, float Distance){
			if(TimeInterval == OtherOne.TimeInterval){
				RailPoint Point1;
				RailPoint Point2;
				float T;
				float InterDist;
				// Выбираем минимальный размер рельсы
				int LowestCount = Math.Min(Points.Count,OtherOne.Points.Count);
				ResultList.Clear();
				//Проходим вдоль рельс, проверяя точки сближения
				for (int i = 0; i < LowestCount; i++)
				{
					Point1 = (RailPoint)Points[i];
					Point2 = (RailPoint)OtherOne.Points[i];
					T = Point1.CPA(Point2);
					if(T<0) T=0;
					if(T>TimeInterval) T=TimeInterval;
					//Момент времени Т меньше интервала рельсы и больше или равен нулю.
					//Считаем расстояние и записываем момент времени, если схождение <= Distance
					InterDist = Point1.GetInterpol(T).Position.DistanceTo(Point2.GetInterpol(T).Position);
					if(InterDist <= Distance) ResultList.Add(i*TimeInterval+T);
				}
				//преобразовываем в массив моментов времени
				float[] result = (float[])ResultList.ToArray(typeof(float));
				return result;
			}
			else {
				throw new Exception("Rail intervals are not similar");
			}
		}

		/// <summary>
		/// Метод, возвращающий все моменты времени, в которые две рельсы оказываются столь же близко или ближе друг к другу, чем указанное расстояние
		/// Перегрузка для проверки конкретной точки
		/// </summary>
		/// <param name="OtherOne">вторая рельса, с которой надо найти все сближения</param>
		/// <param name="Distance">минимальная дистанция, которая считается как достаточное сближение</param>
		/// <param name="Id">Индекс точки, которую надо проверить</param>
		/// <returns>массив всех моментов времени, начиная с начального, в которые обе рельсы сближаются на достаточное расстояние</returns>
		public virtual float[] Approach(Rail OtherOne, float Distance, int Id){
			if(TimeInterval == OtherOne.TimeInterval){
				RailPoint Point1;
				RailPoint Point2;
				float T;
				// Выбираем минимальный размер рельсы
				int LowestCount = Math.Min(Points.Count,OtherOne.Points.Count);
				ResultList.Clear();
				//Проходим вдоль рельс, проверяя точки сближения
				Point1 = (RailPoint)Points[Id];
				Point2 = (RailPoint)OtherOne.Points[Id];
				T = Point1.CPA(Point2);
				if(T<0) T=0;
				if(T>TimeInterval) T=TimeInterval;
				//Момент времени Т меньше интервала рельсы и больше или равен нулю.
				//Считаем расстояние и записываем момент времени, если схождение <= Distance
				if(Point1.GetInterpol(T).Position.DistanceTo(Point2.GetInterpol(T).Position) <= Distance) ResultList.Add(Id*TimeInterval+T);
				//преобразовываем в массив моментов времени
				float[] result = (float[])ResultList.ToArray(typeof(float));
				return result;
			}
			else {
				throw new Exception("Rail intervals are not similar");
			}
		}

		/// <summary>
		/// Метод, возвращающий все моменты времени, в которые две рельсы оказываются столь же близко или ближе друг к другу, чем указанное расстояние
		/// Перегрузка для проверки только определённого участка рельсы
		/// </summary>
		/// <param name="OtherOne">вторая рельса, с которой надо найти все сближения</param>
		/// <param name="Distance">минимальная дистанция, которая считается как достаточное сближение</param>
		/// <param name="startId">Начальный индекс интервала</param>
		/// <param name="EndId">Конечный индекс интервала</param>
		/// <returns></returns>
		public virtual float[] Approach(Rail OtherOne, float Distance, int startId, int EndId){
			if(TimeInterval == OtherOne.TimeInterval){
				RailPoint Point1;
				RailPoint Point2;
				float T;
				float InterDist;
				// Выбираем минимальный размер рельсы
				int LowestCount = Math.Min(Points.Count,OtherOne.Points.Count);
				int TempStartId = Math.Min(startId,LowestCount-2);
				int TempEndId = Math.Min(EndId,LowestCount-1);
				ResultList.Clear();
				//Проходим вдоль рельс, проверяя точки сближения
				for (int i = TempStartId; i <= TempEndId; i++)
				{
					Point1 = (RailPoint)Points[i];
					Point2 = (RailPoint)OtherOne.Points[i];
					T = Point1.CPA(Point2);
					if(T<0) T=0;
					if(T>TimeInterval) T=TimeInterval;
					//Момент времени Т меньше интервала рельсы и больше или равен нулю.
					//Считаем расстояние и записываем момент времени, если схождение <= Distance
					InterDist = Point1.GetInterpol(T).Position.DistanceTo(Point2.GetInterpol(T).Position);
					if(InterDist <= Distance) ResultList.Add(i*TimeInterval+T);
				}
				//преобразовываем в массив моментов времени
				float[] result = (float[])ResultList.ToArray(typeof(float));
				return result;
			}
			else {
				throw new Exception("Rail intervals are not similar");
			}
		}

		/// <summary>
		/// Метод, возвращающий все моменты времени, в которые рельса сближается с указанной позицией на то же или более короткое расстояние, чем указано во вотором параметре
		/// </summary>
		/// <param name="Pos">Положение в пространстве, с которой проверяется сближение</param>
		/// <param name="Distance">расстояния, ближе которой должнга оказаться рельса</param>
		/// <returns>все моменты времени, в которые цель сближается</returns>
		public virtual float[] Approach(Vector2 Pos, float Distance){
			RailPoint Point;
			float T;
			float InterDist;
			ResultList.Clear();
			//Проходим вдоль рельсы, проверяя точки сближения
			for (int i = 0; i < Points.Count; i++)
			{
				Point = (RailPoint)Points[i];
				T = Point.CPA(Pos);
				if(T<0) T=0;
				if(T < TimeInterval){
					//Момент времени Т меньше интервала рельсы и больше или равен нулю.
					//Считаем расстояние и записываем момент времени, если схождение <= Distance
					InterDist = Point.GetInterpol(T).Position.DistanceTo(Pos);
					if(InterDist <= Distance) ResultList.Add(T);
				}
			}
			//преобразовываем в массив моментов времени
			float[] result = (float[])ResultList.ToArray(typeof(float));
			return result;
		}
		
		/// <summary>
		/// Метод для нахождения ближайшей точки с переданной в качестве параметра рельсой
		/// </summary>
		/// <param name="OtherOne">Рельса, с которой надо найти CPA</param>
		/// <returns></returns>
		public virtual float ClosestApproach(Rail OtherOne){
			if(TimeInterval == OtherOne.TimeInterval){
				RailPoint Point1 = (RailPoint)Points[0];
				RailPoint Point2 = (RailPoint)OtherOne.Points[0];
				float MinDist = Point1.Position.DistanceTo(Point2.Position);
				float MinT = 0;
				float T = 0;
				float InterDist;
				// Выбираем минимальный размер рельсы
				int LowestCount = Math.Min(Points.Count,OtherOne.Points.Count);
				ResultList.Clear();
				//Проходим вдоль рельс, проверяя точки сближения
				for (int i = 0; i < LowestCount; i++)
				{
					Point1 = (RailPoint)Points[i];
					Point2 = (RailPoint)OtherOne.Points[i];
					T = Point1.CPA(Point2);
					if(T<0) T=0;
					if(T>TimeInterval) T=TimeInterval;
					//Момент времени Т меньше интервала рельсы и больше или равен нулю.
					//Считаем расстояние и записываем момент времени, если схождение < минимального
					InterDist = Point1.GetInterpol(T).Position.DistanceTo(Point2.GetInterpol(T).Position);
					if(InterDist < MinDist) {
						MinDist = InterDist;
						MinT = i*TimeInterval+T;
					}
				}
				return MinT;
			}
			else {
				throw new Exception("Rail intervals are not similar");
			}
		}

		/// <summary>
		/// Метод для нахождения ближайшей точки с переданной в качестве параметра рельсой
		/// Перегрузка для проверки одной точки
		/// </summary>
		/// <param name="OtherOne">Рельса, с которой надо найти CPA</param>
		/// <param name="Id">Индекс точки, которую надо проверить</param>
		/// <returns></returns>
		public virtual float ClosestApproach(Rail OtherOne, int Id){
			if(TimeInterval == OtherOne.TimeInterval){
				RailPoint Point1 = (RailPoint)Points[0];
				RailPoint Point2 = (RailPoint)OtherOne.Points[0];
				float MinDist = Point1.Position.DistanceTo(Point2.Position);
				float MinT = 0;
				float T = 0;
				float InterDist;
				// Выбираем минимальный размер рельсы
				int LowestCount = Math.Min(Points.Count,OtherOne.Points.Count);
				ResultList.Clear();
				//Проходим вдоль рельс, проверяя точки сближения
				Point1 = (RailPoint)Points[Id];
				Point2 = (RailPoint)OtherOne.Points[Id];
				T = Point1.CPA(Point2);
				if(T<0) T=0;
				if(T>TimeInterval) T=TimeInterval;
				//Момент времени Т меньше интервала рельсы и больше или равен нулю.
				//Считаем расстояние и записываем момент времени, если схождение < минимального
				InterDist = Point1.GetInterpol(T).Position.DistanceTo(Point2.GetInterpol(T).Position);
				if(InterDist < MinDist) {
					MinDist = InterDist;
					MinT = Id*TimeInterval+T;
				}
				return MinT;
			}
			else {
				throw new Exception("Rail intervals are not similar");
			}
		}

		/// <summary>
		/// Метод для нахождения ближайшей точки с переданной в качестве параметра рельсой
		/// Перегрузка для проверки конкретного отрезка
		/// </summary>
		/// <param name="OtherOne">Рельса, с которой надо найти CPA</param>
		/// <returns></returns>
		public virtual float ClosestApproach(Rail OtherOne, int startId, int EndId){
			if(TimeInterval == OtherOne.TimeInterval){
				RailPoint Point1 = (RailPoint)Points[0];
				RailPoint Point2 = (RailPoint)OtherOne.Points[0];
				float MinDist = Point1.Position.DistanceTo(Point2.Position);
				float MinT = 0;
				float T = 0;
				float InterDist;
				// Выбираем минимальный размер рельсы
				int LowestCount = Math.Min(Points.Count,OtherOne.Points.Count);
				int TempStartId = Math.Min(startId,LowestCount-2);
				int TempEndId = Math.Min(EndId,LowestCount-1);
				ResultList.Clear();
				//Проходим вдоль рельс, проверяя точки сближения
				for (int i = TempStartId; i < TempEndId; i++)
				{
					Point1 = (RailPoint)Points[i];
					Point2 = (RailPoint)OtherOne.Points[i];
					T = Point1.CPA(Point2);
					if(T<0) T=0;
					if(T>TimeInterval) T=TimeInterval;
					//Момент времени Т меньше интервала рельсы и больше или равен нулю.
					//Считаем расстояние и записываем момент времени, если схождение < минимального
					InterDist = Point1.GetInterpol(T).Position.DistanceTo(Point2.GetInterpol(T).Position);
					if(InterDist < MinDist) {
						MinDist = InterDist;
						MinT = i*TimeInterval+T;
					}
				}
				return MinT;
			}
			else {
				throw new Exception("Rail intervals are not similar");
			}
		}
		
		/// <summary>
		/// Метод для нахождения ближайшей точки с переданным в качестве параметра вектором
		/// </summary>
		/// <param name="Pos">Вектор, с которым надо найти CPA</param>
		/// <returns></returns>
		public virtual float ClosestApproach(Vector2 Pos){
			RailPoint Point1 = (RailPoint)Points[0];
			float MinDist = Point1.Position.DistanceTo(Pos);
			float MinT = 0;
			float T = 0;
			float InterDist;
			ResultList.Clear();
			//Проходим вдоль рельс, проверяя точки сближения
			for (int i = 0; i < Points.Count; i++)
			{
				Point1 = (RailPoint)Points[i];
				T = Point1.CPA(Pos);
				if(T<0) T=0;
				if(T < TimeInterval){
					//Момент времени Т меньше интервала рельсы и больше или равен нулю.
					//Считаем расстояние и записываем момент времени, если схождение < минимального
					InterDist = Point1.GetInterpol(T).Position.DistanceTo(Pos);
					if(InterDist < MinDist) {
						MinDist = InterDist;
						MinT = T;
					}
				}
			}
			return MinT;
		}
		
		/// <summary>
		/// Моделирование и добавление новых точек к концу рельсы, а так же связывание их через интерполяцию методами точек
		/// </summary>
		/// <param name="Count">количество точек, которые надо добавить</param>
		public virtual void Extrapolate(int Count){
			RailPoint Temp = (RailPoint)Points[Points.Count-1];
			RailPoint Temp2;
			for (int i = 0; i < Count; i++)
			{
				Temp2 = Temp.CreateNextPoint(TimeInterval);
				Points.Add(Temp2);
				Temp.InterpolateTo(Temp2,TimeInterval);
				Temp = Temp2;
			}
		}

		/// <summary>
		/// Устанавливает первую точку рельсы
		/// </summary>
		/// <param name="Point"></param>
		public void SetFirstPoint(RailPoint Point){
			if(Points.Count < 1){
				Points.Add(Point);
			} else {
				Points[0] = Point;
			}
		}

		/// <summary>
		/// Возвращает точку на указанном индексе
		/// </summary>
		/// <param name="i">индекс точки рельсы, которую надо вернуть</param>
		/// <returns></returns>
		public RailPoint GetPoint(int i){
			return (RailPoint)Points[i];
		}

		/// <summary>
		/// Метод для удаления точек с начала общего списка
		/// </summary>
		/// <param name="Count">количество точек для удаления</param>
		public void RemoveFromStart(int Count){
			if(Points.Count>0){
				Points.RemoveRange(0, Count);
			}
		}

		/// <summary>
		/// Метод для удаления точек с конца общего списка
		/// </summary>
		/// <param name="Count">количество точек для удаления</param>
		public void RemoveFromEnd(int Count){
			if(Points.Count>0){
				Points.RemoveRange(Points.Count - Count, Count);
			}
		}
		
		/// <summary>
		/// Метод, возвращающий количество существующих в рельсе точек
		/// </summary>
		/// <returns></returns>
		public int GetCount(){
			return Points.Count;
		}
		
		/// <summary>
		/// Метод, отвечающий за возврат значения положения на рельсе в соответствии с заданным моментом времени.
		/// Если момент времени находится за пределами смоделлированного точками участка времени, метод вернёт точки на соответствующей границе рельсы
		/// За нулевой момент времени взята первая точка рельсы
		/// </summary>
		/// <param name="T">заданный момент времени</param>
		/// <returns>структура, содержащая в себе все необходимые данные для описания текущего положения объекта</returns>
		public RailPoint.InterData Interpolate(float T){
			RailPoint Temp;
			if (T > 0)
			{
				if (T < TimeInterval*Points.Count){
					//Возвращает нужную точку
					int Id = (int)Math.Floor(T/TimeInterval);
					float tau = T%TimeInterval;
					Temp = (RailPoint)Points[Id];
					return Temp.GetInterpol(tau);
				} else {
					//Возвращает точку в последнем моменте времени, описанном рельсой
					Temp = (RailPoint)Points[Points.Count-1];
					return Temp.GetInterpol(TimeInterval);
				}
			} else {
				//Возвращает начальную позицию рельсы
				Temp = (RailPoint)Points[0];
				return Temp.GetInterpol(0);
			}
		}

		/// <summary>
		/// Метод для возврата последнего момента времени, описанного данной ерльсой
		/// </summary>
		/// <returns></returns>
		public float GetLastT(){
			return Points.Count*TimeInterval;
		}
		
		/// <summary>
		/// Метод, проверяющий, содержится ли указанный момент времени Т в промежутке, описываемом рельсой
		/// </summary>
		/// <param name="T"></param>
		/// <returns></returns>
		public bool TIsOnRail(float T){
			return (T > 0 && T <= TimeInterval*Points.Count);
		}
	}

	/// <summary>
	/// Передвигающийся по рельсе объект с сохранением положения. Порождается GlobalRailController
	/// </summary>
	public class RailFollower{

		/// <summary>
		/// GlobalRailController данного адаптера
		/// </summary>
		GlobalRailController Parent;

		/// <summary>
		/// Рельса, которая связана с данным экземпляром
		/// </summary>
		public Rail Current = null;

		/// <summary>
		/// Текущее смещение времени относительно стартового положения рельсы
		/// </summary>
		public float Shift = 0;

		public RailFollower(GlobalRailController parent){
			Parent = parent;
		}

		/// <summary>
		/// Метод для получения текущей интерполяционной точки на рельсе
		/// </summary>
		/// <returns></returns>
		public RailPoint.InterData GetInterpolation(){
			if(Current != null){
				if (Shift >= Parent.ShiftT && Shift <= Current.GetLastT()+Parent.ShiftT)
				{
					return Current.Interpolate(Shift-Parent.ShiftT);
				} else throw new Exception("Shift is out of rail");
			} else throw new Exception("Rail is not set");
		}

		/// <summary>
		/// Возвращает текущий айди рельсы
		/// </summary>
		/// <returns></returns>
		public int CurrentID(){
			if(Current != null){
				if (Shift >= Parent.ShiftT && Shift <= Current.GetLastT()+Parent.ShiftT)
				{
					return Current.IDFromTime(Shift-Parent.ShiftT);
				} else throw new Exception("Shift is out of rail");
			} else throw new Exception("Rail is not set");
		}
	}
}