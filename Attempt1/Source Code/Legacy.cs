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
		/// Метод задания текущего поворота спрайта в радианах
		/// </summary>
		/// <param name="i">индекс спрайта</param>
		/// <param name="Angle">Угол поворота</param>
		public void SetRotation(int i, float Angle){
			Items[i].Rotation = Angle;
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