using Godot;

    /// <summary>
    /// Структура, отвечающая за состояние точки в каждый момент времени
    /// </summary>
    public class RailPoint
    {   
        /// <summary>
        /// Переменная, отвечающая за время
        /// </summary>
        public float time = 0;

        /// <summary>
        /// Положение точки в пространстве
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Угол поворота точки
        /// </summary>
        public float Rotation;

        /// <summary>
        /// Поле, отвечающее за скорость в данной точке
        /// </summary>
        public Vector2 Speed;

        /// <summary>
        /// Поле, отвечающее за скорость поворота в данной точке
        /// </summary>
        public float RotSpeed;

        /// <summary>
        /// Поле, отвечающее за ускорение в данной точке
        /// </summary>
        public Vector2 Acceleration;

        /// <summary>
        /// Поле, ответственное за угловое ускорение в данной точке рельсы
        /// </summary>
        public float RotAccel;

        /// <summary>
        /// Метод, возвращающий новую точку, основываясь на данных старой.
        /// </summary>
        /// <param name="Point">Предыдущая точка</param>
        /// <param name="T">интервал времени, на котором проходит симуляция</param>
        /// <returns></returns>
        public RailPoint GetNext(float T){
            RailPoint Result = new RailPoint();
            Result.Position = Position+Speed*T+(Acceleration*T*T)/2;
            Result.Rotation = Rotation+RotSpeed*T+(RotAccel*T*T)/2;
            Result.Speed = Speed+Acceleration*T;
            Result.RotSpeed = RotSpeed+RotAccel*T;
            Result.Acceleration = Acceleration;
            Result.RotAccel = RotAccel;
            Result.time = time + T;
            return Result;
        }
        
        /// <summary>
        /// Метод, возвращающий положение на отрезке времени от 0 до maxT, равную T
        /// </summary>
        /// <param name="T"></param>
        /// <param name="maxT"></param>
        /// <returns></returns>
        public Vector2 GetInterPos(float T, float maxT){
            return Position+GetInterSpeed(maxT)*T;
        }
        
        /// <summary>
        /// Метод, возвращающий вращение на отрезке времени от 0 до maxT, равную T
        /// </summary>
        /// <param name="T"></param>
        /// <param name="maxT"></param>
        /// <returns></returns>
        public float GetInterRot(float T, float maxT){
            float InterRt = GetInterRotSpeed(maxT);
            return Rotation+InterRt*T;
        }

		/// <summary>
		/// Возвращает значение времени максимального сближения с указанной точкой. За ноль взят момент времени в текущей точке.
		/// </summary>
		/// <param name="Target">Вторая точка, с которой просчитывается пересечение путей</param>
		/// <returns></returns>
		public float CPA(RailPoint Target, float T, bool clamped = true){
            float Result = MathExtra.cpaTime(Position,Target.Position,GetInterSpeed(T),Target.GetInterSpeed(T));
            if(clamped){
                if(Result < 0) Result = 0;
                if(Result > T) Result = T;
            }
			return Result;
		}

		/// <summary>
		/// Возвращает значение времени пересечения сближения с указанной точкой. За ноль взят момент времени в текущей точке.
        /// Если время выходит за границы отрезка времени - берётся ближайшая точка на этом отрезке.
		/// Перегрузка для 2д вектора
		/// </summary>
		/// <param name="Vector">2Д вектор, с которым просчитывается пересечение путей</param>
        /// <param name="T">интервал времени, на котором проходит проверка</param>
		/// <returns></returns>
		public float CPA(Vector2 Vector, float T, bool clamped = true){
			float Result = MathExtra.cpaTime(Position,Vector,GetInterSpeed(T),new Vector2(0,0));
            if(clamped){
                if(Result < 0) Result = 0;
                if(Result > T) Result = T;
            }
			return Result;
		}

        /// <summary>
        /// Метод, который возвращает среднюю скорость от начала точки до участка времени Т
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public Vector2 GetInterSpeed(float T){
            Vector2 Speed2 = Speed+Acceleration*T;
            return (Speed+Speed2)/2;
        }

        /// <summary>
        /// Метод, который возвращает среднюю скорость вращения от начала точки до участка времени Т
        /// </summary>
        /// <param name="T"></param>
        /// <returns></returns>
        public float GetInterRotSpeed(float T){
            float RotSpeed2 = RotSpeed+RotAccel*T;
            return (RotSpeed+RotSpeed2)/2;
        }

        public RailPoint(){
            
        }

        public RailPoint(RailPoint Other){
            Position = Other.Position;
            Rotation = Other.Rotation;
            Speed = Other.Speed;
            RotSpeed = Other.RotSpeed;
            Acceleration = Other.Acceleration;
            RotAccel = Other.RotAccel;
            time = Other.time;
        }

        /// <summary>
        /// Метод для преобразования контента в стринг
        /// </summary>
        /// <returns></returns>
        public string Stringify(){
            string Result = "";
            Result += "Position = ("+Position.x+";"+Position.y+")";
            return Result;
        }
     
    
    }
