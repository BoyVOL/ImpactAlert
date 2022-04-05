using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics{

    /// <summary>
    /// Структура, содержащая параметры отрисовки спрайта
    /// </summary>
    public class SpriteParams{
        public Sprite Sprite;
    }

    /// <summary>
    /// Класс для отрисовки спрайтов
    /// </summary>
    public class SpriteDraw: RailDraw<SpriteParams>{

        public SpriteDraw(Dictionary<int,List<RailPoint>> rails,RailTimeController timeController) : base(rails,timeController){
        }

        public override void Redraw(int ID)
        {
            if (DrawParams.ContainsKey(ID))
            {
                DrawParams[ID].Sprite.Position = Rails[ID][0].Position;
                GD.Print(DrawParams[ID].Sprite.Position);
                DrawParams[ID].Sprite.Rotation = Rails[ID][0].Rotation;
            }
        }
    }
}