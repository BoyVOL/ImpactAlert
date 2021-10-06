using Godot;
using System;
using System.Collections;

/// <summary>
/// Родительский класс всех космических объектов
/// </summary>
public class SpaceObject {

}

/// <summary>
/// Класс точек рельсы, который имеет абстрактный метод для порождения дочерних экземпляров для экстраполяции
/// </summary>
public abstract class RailPoint{

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
		GD.Print(Speed);
		RotSpeed = (Target.Rotation - Rotation) / T;
	}

	/// <summary>
	/// Определяет логику экстраполирования траектории движения объекта.
	/// </summary>
	/// <param name="T">интервал времени моделирования до следующей точки</param>
	/// <returns>Новая точка на основе указанных данных</returns>
	public abstract RailPoint CreateNextPoint(float T);
}

/// <summary>
/// Класс точки рельсы, отвечающий за её равномерное движение
/// </summary>
public class KineticPoint : RailPoint{

	/// <summary>
	/// Скорость симуляции объекта
	/// </summary>
	public Vector2 SimSpeed;

	/// <summary>
	/// Скорость вращения симуляции объекта
	/// </summary>
	public float SimRotSpeed;

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
        return new KineticPoint(Position+SimSpeed*T,Rotation+SimRotSpeed*T);
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
	/// Метод установки временного интервала между точками рельсы
	/// </summary>
	/// <param name="newInterval"></param>
	public void SetInterval(float newInterval){
		TimeInterval = newInterval;
		ReExtrapolate(Points.Count-1);
	}

	public float GetInterval(){
		return TimeInterval;
	}
	
	/// <summary>
	/// Метод для повторного заполнения рельсы массивом точек
	/// </summary>
	/// <param name="Count">количество точек, которые надо добавить</param>
	public void ReExtrapolate(int Count){
		Points.RemoveRange(1,Points.Count-1);
		Extrapolate(Count);
	}
	
	/// <summary>
	/// Метод, возвращающий все моменты времени, в которые две рельсы оказываются столь же близко или ближе друг к другу, чем указанное расстояние
	/// </summary>
	/// <param name="OtherOne">вторая рельса, с которой надо найти все сближения</param>
	/// <param name="Distance">минимальная дистанция, которая считается как достаточное сближение</param>
	/// <returns>массив всех моментов времени, начиная с начального, в которые обе рельсы сближаются на достаточное расстояние</returns>
	public float[] ApprToRail(Rail OtherOne, float Distance){
		RailPoint Point1;
		RailPoint Point2;
		float T;
		float InterDist;
		// Выбираем минимальный размер рельсы
		int LowestCount;
		if(Points.Count <= OtherOne.Points.Count){
			LowestCount = Points.Count;
		} else {
			LowestCount = OtherOne.Points.Count;
		}
		ResultList.Clear();
		//Проходим вдоль рельс, проверяя точки сближения
		for (int i = 0; i < LowestCount; i++)
		{
			Point1 = (RailPoint)Points[i];
			Point2 = (RailPoint)OtherOne.Points[i];
			T = Point1.CPA(Point2);
			if(T<0) T=0;
			if(T < TimeInterval){
				//Момент времени Т меньше интервала рельсы и больше или равен нулю.
				//Считаем расстояние и записываем момент времени, если схождение <= Distance
				InterDist = Point1.GetInterpol(T).Position.DistanceTo(Point2.GetInterpol(T).Position);
				if(InterDist <= Distance) ResultList.Add(T);
			}
		}
		//преобразовываем в массив моментов времени
		float[] result = (float[])ResultList.ToArray(typeof(float));
		return result;
	}

	/// <summary>
	/// Метод, возвращающий все моменты времени, в которые рельса сближается с указанной позицией на то же или более короткое расстояние, чем указано во вотором параметре
	/// </summary>
	/// <param name="Pos">Положение в пространстве, с которой проверяется сближение</param>
	/// <param name="Distance">расстояния, ближе которой должнга оказаться рельса</param>
	/// <returns>все моменты времени, в которые цель сближается</returns>
	public float[] ApprToPos(Vector2 Pos, float Distance){
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
	/// Моделирование и добавление новых точек к концу рельсы, а так же связывание их через интерполяцию методами точек
	/// </summary>
	/// <param name="Count">количество точек, которые надо добавить</param>
	public void Extrapolate(int Count){
		RailPoint Temp = (RailPoint)Points[Points.Count-1];
		RailPoint Temp2;
		for (int i = 0; i < Count; i++)
		{
			Temp2 = Temp.CreateNextPoint(TimeInterval);
			Points.Add(Temp2);
			Temp.InterpolateTo(Temp2,TimeInterval);
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
		Points.RemoveRange(0, Count);
	}

	/// <summary>
	/// Метод для удаления точек с конца общего списка
	/// </summary>
	/// <param name="Count">количество точек для удаления</param>
	public void RemoveFromEnd(int Count){
		Points.RemoveRange(Points.Count - Count, Count);
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
				int Id = (int)Math.Floor(T/TimeInterval);
				float tau = T%TimeInterval;
				Temp = (RailPoint)Points[Id];
				return Temp.GetInterpol(tau);
			} else {
				Temp = (RailPoint)Points[Points.Count-1];
				return Temp.GetInterpol(TimeInterval);
			}
		} else {
			Temp = (RailPoint)Points[0];
			return Temp.GetInterpol(0);
		}
	}
}