using System.Collections;
using System.Collections.Generic;
using Godot;

public class MathExtra
{
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
}
