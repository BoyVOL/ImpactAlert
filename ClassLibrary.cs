using Godot;
using System;
using System.Collections;

/// <summary>
/// Пространство имём классов, разработанных для предыдущего проекта
/// </summary>
namespace Legacy {

	/// <summary>
	/// Класс проверки многопоточности и взаимодействия скриптов
	/// </summary>
	public class TestClass {
		/// <summary> Поле данных 1 </summary>
		public int data1 = 0;
		/// <summary> Поле данных 1 </summary>
		public int data2 = 0;
		/// <summary>
		/// Поле данных 3
		/// </summary>
		public int data3 = 0;
		/// <summary>
		/// Метод изменения данных 1. Увеличивает поле на 1 и спит на секунду.
		/// </summary>
		public void ChangeData1(){
			data1 ++;
			System.Threading.Thread.Sleep(100);
			GD.Print("Data1 = "+data1.ToString());
		}
	/// <summary>
		/// Метод изменения данных 2. Увеличивает поле на 1 и спит на секунду.
		/// </summary>
		public void ChangeData2(){
			data2 ++;
			System.Threading.Thread.Sleep(100);
			GD.Print("Data2 = "+data2.ToString());
		}
	/// <summary>
		/// Метод изменения данных 3. Увеличивает поле на 1 и спит на секунду.
		/// </summary>
		public void ChangeData3(){
			data3 ++;
			System.Threading.Thread.Sleep(1000);
			GD.Print("Data3 = "+data3.ToString());
		}
	}


	/// <summary>
	/// Класс для создания и защищённого хранения фиксированного количества элементов заданного класса. 
	/// Родительский класс для множества других дочерних.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class FixedBufferArray<T> where T : class, new(){
		/// <summary>
		/// Массив объектов
		/// </summary>
		protected T[] Items;
		/// <summary>
		/// Конструктор без параметров. По умолчанию количество элементов 100
		/// </summary>
		public FixedBufferArray(){
			Items = new T[100];
			for (int i = 0; i < Items.Length; i++)
			{
				Items[i] = new T();
			}
		}
		/// <summary>
		/// Конструктор с параметром. Принимает целочисленное значение длины массива
		/// </summary>
		/// <param name="length">длина массива</param>
		public FixedBufferArray(int length){
			Items = new T[length];
			for (int i = 0; i < Items.Length; i++)
			{
				Items[i] = new T();
			}
		}
		/// <summary>
		/// Метод для возвращения ссылки на содержащийся объект
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public T GetItem(int i){
			return Items[i];
		}
		/// <summary>
		/// Возврат длины внутреннего массива
		/// </summary>
		/// <returns>целочисленное значение длины</returns>
		public int getLength(){
			return Items.Length;
		}
	}

	/// <summary>
	/// Объект-буффер для хранения предзагруженных спрайтов.
	/// </summary>
	public class SpriteBufferArray: FixedBufferArray<Sprite>{
		/// <summary>
		/// Перегрузка конструктора по умолчанию. Задаёт базовые параметры спрайта
		/// </summary>
		/// <returns></returns>
		public SpriteBufferArray() : base(){
			for (int i = 0; i < Items.Length; i++)
			{
				Items[i].Visible = false;
			}
		}
		/// <summary>
		/// Перегрузка конструктора с параметром длины массива. Задаёт базовые параметры спрайта
		/// </summary>
		/// <param name="length">длина массива</param>
		/// <returns></returns>
		public SpriteBufferArray(int length) : base(length){
			for (int i = 0; i < Items.Length; i++)
			{
				Items[i].Visible = false;
			}
		}
		/// <summary>
		/// Задание текстуры элемента
		/// </summary>
		/// <param name="i">индекс элемента</param>
		/// <param name="texture">текстура, которую надо загрузить спрайту</param>
		public void SetTexture(int i, Texture texture){
			Items[i].Texture = texture;
		}
		/// <summary>
		/// Метод задания текущих координат спрайта
		/// </summary>
		/// <param name="i">индекс спрайта</param>
		/// <param name="Coords">Координаты</param>
		public void SetCoordinates(int i, Vector2 Coords){
			Items[i].Position = Coords;
		}
		/// <summary>
		/// Метод задания видимости спрайта
		/// </summary>
		/// <param name="i">индекс спрайта</param>
		/// <param name="Visibility">Видимость</param>
		public void SetVisible(int i, bool Visibility){
			Items[i].Visible = Visibility;
		}
		/// <summary>
		/// Метод задания размеров спрайта
		/// </summary>
		/// <param name="i">индекс спрайта</param>
		/// <param name="Scale">Размеры</param>
		public void SetSize(int i, Vector2 Scale){
			Items[i].Scale = Scale;
		}
		/// <summary>
		/// Добавляет указанный элемент как ребёнок указанному экземпляру Node2D
		/// </summary>
		/// <param name="i">индекс элемента, который надо добавить</param>
		/// <param name="Parent">Новый Родитель</param>
		public void AddAsChild(int i, Node2D Parent){
			Parent.AddChild(Items[i]);
		}
		/// <summary>
		/// Удаляет выбранный элемент из списка детей выбранного экземпляра Node2D
		/// </summary>
		/// <param name="i">индекс элемента, которого надо удалить из списка детей старого родителя</param>
		/// <param name="Parent">Объект-родитель.</param>
		public void DeleteAsChild(int i, Node2D Parent){
			Parent.RemoveChild(Items[i]);
		}
	}

	/// <summary>
	/// Класс, содержащий в себе функционал для обновления состояния каждого объекта в списке
	/// </summary>
	public class UpdatableList{
		/// <summary>
		/// Класс для определения элемента и метода его обновления
		/// </summary>
		public class Updatable{
			public virtual void Update(){

			}
		}
		/// <summary>
		/// Список Обновляемых элементов
		/// </summary>
		/// <returns></returns>
		ArrayList ListOfObjects = new ArrayList();
		/// <summary>
		/// Добавление элемента в список
		/// </summary>
		/// <param name="Element">элемент, который нужно добавить</param>
		public void AddElement(Updatable Element){
			ListOfObjects.Add(Element);
		}
		/// <summary>
		/// Очистка списка от всех элементов
		/// </summary>
		public void Clear(){
			ListOfObjects = new ArrayList();
		}
		/// <summary>
		/// Метод обновления всех элементов в списке
		/// </summary>
		public void UpdateAll(){
			foreach (Updatable item in ListOfObjects){
				item.Update();
			}
		}
	}

	/// <summary>
	/// Класс, отвечающий за физическое движение какого-либо предмета
	/// </summary>
	public class MovingBody{
		/// <summary>
		/// Текущее положение объекта
		/// </summary>
		public Vector2 Position;
		/// <summary>
		/// Вектор скорости
		/// </summary>
		public Vector2 Speed;
		/// <summary>
		/// Метод обновления координат объекта в соответствии с его текущими скоростью и положением
		/// </summary>
		/// <param name="t">интервал времени, в котором происходит движение</param>
		public void Move(float t){
			Position+= Speed * t;
		}
	}

	/// <summary>
	/// Класс внешних элементов юнита.
	/// </summary>
	public class UnitElement{
		/// <summary>
		/// Спрайт элемента. По умолчанию null. Предполагается задавать его извне, чтобы не загружать процесс созданием нового класса.
		/// </summary>
		public Sprite ElementSprite = null;

		/// <summary>
		/// Ссылка на родителя элемента
		/// </summary>
		Unit Parent = null;
		/// <summary>
		/// Положение элемента относительно родителя
		/// </summary>
		/// <returns></returns>
		public Vector2 shift = new Vector2(0,0);
		/// <summary>
		/// Угол поворота элемента относительно родителя
		/// </summary>
		public float Angle = 0;
		/// <summary>
		/// Конструктор по умолчанию
		/// </summary>
		public UnitElement(){
		}
		/// <summary>
		/// Метод обновления параметров спрайта. 
		/// </summary>
		public void UpdateSprite(){
			if (ElementSprite != null)
			{
				if (Parent != null)
				{
					
				} else throw new Exception("Parent is not set");
			} else throw new Exception("Sprite is not set");
		}
	}

	/// <summary>
	/// Класс оружия юнита. Класс предполагается часто клонировать вместо пересоздания.
	/// </summary>
	public class UnitWeapon : UnitElement
	{
		/// <summary>
		/// Конструктор с параметром.
		/// </summary>
		/// <param name="Parent">Родитель класса</param>
		/// <returns></returns>
		public UnitWeapon() : base(){

		} 
	}

	/// <summary>
	/// Класс колёс/ног/гусениц/прочих частей для движения юнита Класс предполагается часто клонировать вместо пересоздания.
	/// </summary>
	public class UnitChasis : UnitElement
	{
		/// <summary>
		/// Конструктор с параметром.
		/// </summary>
		/// <param name="Parent">Родитель Класса</param>
		/// <returns></returns>
		public UnitChasis() : base(){

		}
	}

	/// <summary>
	/// Класс подконтрольного юнита. Содержит в себе методы для обработки его состояния и поведения.
	/// </summary>
	public class Unit 
	{
		/// <summary>
		/// Спрайт тела юнита. По умолчанию null. Предполагается задавать его извне, чтобы не загружать процесс созданием нового класса.
		/// </summary>
		Sprite BodySprite = null;
		/// <summary>
		/// Массив пушек юнита. 
		/// Максимальная размерность 10. По умолчанию все null;
		/// </summary>
		FixedBufferArray<UnitWeapon> Weapons = new FixedBufferArray<UnitWeapon>(10);
		/// <summary>
		/// Массив спрайтов шасси (частей для передвижения) юнита. По умолчанию все null;
		/// Размерность массива - 10
		/// </summary>
		FixedBufferArray<UnitChasis> Chasis = new FixedBufferArray<UnitChasis>(10);
		/// <summary>
		/// Положение объекта на игровом поле. По умолчанию 0,0.
		/// </summary>
		Vector2 Position = new Vector2(0,0);
		/// <summary>
		/// Угол поворота тела в радианах. По умолчанию 0.
		/// </summary>
		float Rotation = 0;
		/// <summary>
		/// Конструктор по умолчанию. В нём мы задаём начальные значения переменным.
		/// </summary>
		public Unit(){
		}
	
	}

	/// <summary>
	/// Хэш-таблица, распределяющая объекты в очереди по их координатам
	/// </summary>
	public class PosQueueTable<T> {
		/// <summary>
		/// Таблица очередей, содержащая все координаты
		/// </summary>
		/// <returns></returns>
		protected Hashtable HshTable = new Hashtable();
		/// <summary>
		/// Константа смещения второй координаты в хэш индексе;
		/// </summary>
		protected const int SecondCoordShift = 100000;
		/// <summary>
		/// Переменная для извлечения данных из очередей
		/// </summary>
		protected Queue Output;
		/// <summary>
		/// Метод для трансляции двух координат в одно целочисленное значение
		/// </summary>
		/// <param name="x">координата x</param>
		/// <param name="y">координата y</param>
		/// <returns></returns>
		public int TranslateCoords(int x, int y){
			return x+y*SecondCoordShift;
		}
		/// <summary>
		/// Метод инициализирует новую очередь по указанным координатам
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void InitQueue(int x, int y){
			Queue Temp = new Queue();
			HshTable.Add(TranslateCoords(x,y),Temp);
		}
		/// <summary>
		/// Метод добавления элемента в очередь по указанным координатам
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="item">Элемент, который надо добавить</param>
		public void Enqueue(int x, int y, T item){
			Output = (Queue)HshTable[TranslateCoords(x,y)];
			if (Output != null){	
				Output.Enqueue(item);
			} else throw new Exception("Queue is not initialised");
		}
		/// <summary>
		/// Метод изъятия первого элемента из очереди по выбранным координатам
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>первый в очереди элемент</returns>
		public T Dequeue(int x, int y){
			Output = (Queue)HshTable[TranslateCoords(x,y)];
			if (Output != null){
				return (T)Output.Dequeue();
			} else throw new Exception("Queue is not initialised");
		}
		/// <summary>
		/// Проверка на то, что по указанным координатам уже задана очередь
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns>true, если очередь существует. false в обратном случае</returns>
		public bool Exists(int x, int y){
			Output = (Queue)HshTable[TranslateCoords(x,y)];
			return Output != null;
		}
		/// <summary>
		/// Инициализирует набор очередей по координатам в прямоугольной области
		/// </summary>
		/// <param name="MinX">Нижний край области</param>
		/// <param name="MaxX">Верхний край области</param>
		/// <param name="MinY">Левый край области</param>
		/// <param name="MaxY">Правый край области</param>
		public void InitRange(int MinX, int MaxX, int MinY, int MaxY){
			for (int x = MinX; x <= MaxX; x++)
			{
				for (int y = MinY; y <= MaxY; y++)
				{
					InitQueue(x,y);
				}
			}
		}
	}

	/// <summary>
	/// Хэш-таблица, распределяющая объекты в списки по их координатам
	/// </summary>
	public class PosListTable<T> {
	/// <summary>
	/// Таблица очередей, содержащая все координаты
	/// </summary>
	/// <returns></returns>
	protected Hashtable HshTable = new Hashtable();
	/// <summary>
	/// Константа смещения второй координаты в хэш индексе;
	/// </summary>
	protected const int SecondCoordShift = 100000;
	/// <summary>
	/// Переменная-буфер для извлечения идентификатора списка
	/// </summary>
	protected ArrayList Output;
	/// <summary>
	/// Метод для трансляции двух координат в одно целочисленное значение
	/// </summary>
	/// <param name="x">координата x</param>
	/// <param name="y">координата y</param>
	/// <returns></returns>
	public int TranslateCoords(int x, int y){
		return x+y*SecondCoordShift;
	}
	/// <summary>
	/// Метод для инициализации списка по указанным координатам
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public void InitList(int x, int y){
		ArrayList Temp = new ArrayList();
		HshTable.Add(TranslateCoords(x,y),Temp);
	}
	/// <summary>
	/// Метод для добавления элемента в конец списка по указанным координатам
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="item">Элемент, который надо добавить</param>
	public void AddElement(int x, int y, T item){
		Output = (ArrayList)HshTable[TranslateCoords(x,y)];
		if (Output == null) {
			InitList(x,y);
			Output = (ArrayList)HshTable[TranslateCoords(x,y)];
		}
		Output.Add(item);
	}
	/// <summary>
	/// Метод для удаления элемента с указанным индексом из списка по указанным координатам
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="id">индекс элемента в списке</param>
	public void DeleteElement(int x, int y, int id){
		Output = (ArrayList)HshTable[TranslateCoords(x,y)];
		if (Output != null){
			Output.RemoveAt(id);
		} else throw new Exception("Queue is not initialised");
	}
	/// <summary>
	/// Метод получения элемента в определённом индексе
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="i">индекс целевого элемента</param>
	/// <returns>Элемент по указанному индексу</returns>
	public T GetElement(int x, int y, int i){
		Output = (ArrayList)HshTable[TranslateCoords(x,y)];
		if (Output != null){
			return (T)Output[i];
		} else throw new Exception("Queue is not initialised");
	}
	/// <summary>
	/// Проверка на то, что по указанным координатам уже задана очередь
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns>true, если очередь существует. false в обратном случае</returns>
	public bool Exists(int x, int y){
		Output = (ArrayList)HshTable[TranslateCoords(x,y)];
		return Output != null;
	}
	/// <summary>
	/// Инициализирует набор очередей по координатам в прямоугольной области
	/// </summary>
	/// <param name="MinX">Нижний край области</param>
	/// <param name="MaxX">Верхний край области</param>
	/// <param name="MinY">Левый край области</param>
	/// <param name="MaxY">Правый край области</param>
	public void InitRange(int MinX, int MaxX, int MinY, int MaxY){
		for (int x = MinX; x <= MaxX; x++)
		{
			for (int y = MinY; y <= MaxY; y++)
			{
				InitList(x,y);
			}
		}
	}
}

}

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
	public Vector2 Position;

	/// <summary>
	/// Скорость перемещения для интерполяции
	/// </summary>
	Vector2 Speed = new Vector2(0,0);

	/// <summary>
	/// Угол поворота объекта в радианах. Подразумевает зранение значений больше чем 2*Pi.
	/// </summary>
	public float Rotation;

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
/// Класс рельс, по которым объекты должны двигаться
/// </summary>
public class Rail {

	/// <summary>
	/// Массив точек, вдоль которых объект перемещается
	/// </summary>
	ArrayList Points = new ArrayList();

	/// <summary>
	/// Свойство, описывающее, какое количество времени тратится на перемещение между точками
	/// </summary>
	float TimeInterval = 100;

	/// <summary>
	/// Метод установки временного интервала между точками рельсы
	/// </summary>
	/// <param name="newInterval"></param>
	public void SetTimeInterval(float newInterval){
		TimeInterval = newInterval;
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
	/// Метод, возвращающий все точки пересечения между рельсами на заданном расстоянии друг от друга
	/// </summary>
	/// <param name="OtherOne">вторая рельса, с которой надо найти все сближения</param>
	/// <param name="Distance">минимальная дистанция, которая считается как достаточное сближение</param>
	/// <returns>массив всех моментов времени, начиная с начального, в которые обе рельсы сближаются на достаточное расстояние</returns>
	public float[] ApproachToRail(Rail OtherOne, float Distance){

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
	/// Метод, возвращающий количество существующих в рельсе точек
	/// </summary>
	/// <returns></returns>
	public int GetLength(){
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