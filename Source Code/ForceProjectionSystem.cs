using Godot;
using System.Collections;
using System;

namespace ForceProjection{


    /// <summary>
        /// Структура для задания параметров сил, которые будут накладываться с помощью данного проектора
        /// </summary>
    public struct ForceParams{
        public Vector2 Pos;
        public Vector2 Speed;

        public float Mass;
        public ForceParams(Vector2 pos, float mass){
            Pos = pos;
            Speed = Vector2.Zero;
            Mass = mass;
        }
            
        public ForceParams(Vector2 pos, Vector2 speed, float mass){
            Pos = pos;
            Speed = speed;
            Mass = mass;
        }
    }
    
    /// <summary>
    /// Базовый класс для всех источников силовых полей
    /// </summary>
    public abstract class ForceProjector{
        /// <summary>
        /// Возвращает вектор ускорения, соответствующий результату приложения силы в данной точке
        /// </summary>
        /// <param name="Params"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public abstract Vector2 GetAccelVector(ForceParams Params, float T = 0);
    }

    /// <summary>
    /// Класс, осуществляющий сведение всех взаимодействий в нужной точке в единый вектор
    /// </summary>
    public class ForceProjHandler{
        ArrayList MainArray = new ArrayList();

        /// <summary>
        /// Добавляет проектор силы в систему
        /// </summary>
        /// <param name="New"></param>
        public void AddProjector(ForceProjector New){
            MainArray.Add(New);
        }

        /// <summary>
        /// Убирает указанный проектор силы из системы
        /// </summary>
        /// <param name="Old"></param>
        public void RemoveProjector(ForceProjector Old){
            MainArray.Remove(Old);
        }

        /// <summary>
        /// Убирает проектор силы из системы по указанному индексу
        /// </summary>
        /// <param name="i"></param>
        public void RemoveProjector(int i){
            MainArray.RemoveAt(i);
        }

        /// <summary>
        /// Метод сведения всех силовых влияний в единый вектор, воздействующий ускорению объекта в указанной точке
        /// </summary>
        /// <param name="forceParams"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public Vector2 GetResultAccel(ForceParams forceParams, float T = 0){
            Vector2 Result = Vector2.Zero;
            foreach (ForceProjector item in MainArray)
            {
                Result += item.GetAccelVector(forceParams,T);
            }
            return Result;
        }
    }
}