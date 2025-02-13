using System.Collections;
using System.Collections.Generic;
using Godot;
public partial class MathExtra
{   
    [Export]
    public float Test = 0;

    /// <summary>
    /// Метод, возвращающий момент времени максимального сближения двух объектов с заданными скоростями и начальными точками
    /// </summary>
    /// <param name="start1">Начальная точка объекта 1</param>
    /// <param name="start2">Начальная точка объекта 2</param>
    /// <param name="speed1">Начальная скорость объекта 1</param>
    /// <param name="speed2">Начальная скорость объекта 2</param>
    /// <returns>искомый момент времени</returns>
    public static float cpaTime(Vector3 start1, Vector3 start2, Vector3 speed1, Vector3 speed2)
    {
        Vector3 dv = speed1 - speed2;

        float dv2 = dv.Dot(dv);
        if (dv2 < Mathf.Epsilon)      // the  tracks are almost parallel
            return 0;             // any time is ok.  Use time 0.

        Vector3 w0 = start1 - start2;
        float cpatime = -w0.Dot(dv) / dv2;

        return cpatime;             // time of CPA
    }

    /// <summary>
    /// Метод, возвращающий момент времени максимального сближения двух объектов с заданными скоростями и начальными точками
    /// Перегрузка для двухмерных пространств
    /// </summary>
    /// <param name="start1">Начальная точка объекта 1</param>
    /// <param name="start2">Начальная точка объекта 2</param>
    /// <param name="speed1">Начальная скорость объекта 1</param>
    /// <param name="speed2">Начальная скорость объекта 2</param>
    /// <returns>искомый момент времени</returns>
    public static float cpaTime(Vector2 start1, Vector2 start2, Vector2 speed1, Vector2 speed2)
    {
        Vector2 dv = speed1 - speed2;

        float dv2 = dv.Dot(dv);
        if (dv2 < Mathf.Epsilon)      // the  tracks are almost parallel
            return 0;             // any time is ok.  Use time 0.

        Vector2 w0 = start1 - start2;
        float cpatime = -w0.Dot(dv) / dv2;

        return cpatime;             // time of CPA
    }
    
    /// <summary>
    /// Преобразование параболы в контрольные точки квадратичной кривой безье (не реализовано)
    /// </summary>
    /// <param name="A">параметр а параболы</param>
    /// <param name="B">параметр б параболы</param>
    /// <param name="C">константа параболы</param>
    /// <returns>массив точек кривой безье</returns>
    public Vector2[] ParabolaToBezier(Vector2 A, Vector2 B, Vector2 C){
        Vector2[] Result = new Vector2[3];
        return Result;
    }
    
    /// <summary>
    /// Метод, возвращающий расстояние максимального сближения двух объектов с заданными скоростями и начальными точками
    /// </summary>
    /// <param name="start1">Начальная точка объекта 1</param>
    /// <param name="start2">Начальная точка объекта 2</param>
    /// <param name="speed1">Начальная скорость объекта 1</param>
    /// <param name="speed2">Начальная скорость объекта 2</param>
    /// <returns>Искомое расстояние</returns>
    public static float cpaDistance(Vector3 start1, Vector3 start2, Vector3 speed1, Vector3 speed2)
    {
        float ctime = cpaTime(start1, start2, speed1, speed2);
        Vector3 P1 = start1 + (ctime * speed1);
        Vector3 P2 = start2 + (ctime * speed2);
        Vector3 Temp = P1-P2;

        return Temp.Length();
    }

    /// <summary>
    /// Функция, возвращающая модуль гравитационной силы на основе заданных параметров
    /// </summary>
    /// <param name="m1">Масса первого объекта</param>
    /// <param name="m2">Масса второго объекта</param>
    /// <param name="RQuad">Квадрат радиуса между объектами</param>
    /// <param name="G">Гравитационная постоянная</param>
    /// <returns></returns>
    public static float GravityForce(float m1, float m2, float RQuad, float G){
        return G*(m1*m2)/RQuad;
    }

    /// <summary>
    /// Возвращает обмен импуьсами между объектами 
    /// </summary>
    /// <param name="mass"></param>
    /// <param name="Speed2"></param>
    /// <param name="mass2"></param>
    /// <returns></returns>
    public static Vector2 SpeedExchange(float mass, Vector2 Speed2, float mass2){
        Vector2 Impulse2 = Speed2*mass2;
        return Impulse2/mass;
    }

    /// <summary>
    /// Функция для нахождения вектора скорости, необходимой для перемещения из точки 1 в точку 2 за заданное время
    /// </summary>
    /// <param name="Point1">Начальная точка</param>
    /// <param name="Point2">Конечная точка</param>
    /// <param name="T">Период времени для вычисления</param>
    /// <returns></returns>
    public static Vector2 SpeedBetweenPoints(Vector2 Point1, Vector2 Point2, float T){
        Vector2 Result = (Point2-Point1)/T;
        return Result;
    }
}
