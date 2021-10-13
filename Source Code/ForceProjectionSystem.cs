using Godot;
using System;

namespace ForceProjection{

    /// <summary>
    /// Базовый класс для всех источников силовых полей
    /// </summary>
    public abstract class ForceProjector{
        public abstract Vector2 GetForceVector(Vector2 Pos, float T, Vector2 Speed);
    }
}