using Godot;
using System;
using System.Collections.Generic;

namespace CustomPhysics{

    /// <summary>
    /// Структура, содержащая параметры отрисовки спрайта
    /// </summary>
    public struct SpriteParams{

    }

    /// <summary>
    /// Класс для отрисовки спрайтов
    /// </summary>
    public class SpriteDraw: RailDraw<SpriteParams,Sprite>{

        public SpriteDraw(Dictionary<int,List<RailPoint>> rails) : base(rails){
        }

        public override void Redraw(int ID)
        {
            if (DrawElem.ContainsKey(ID))
            {
                DrawElem[ID].Position = Rails[ID][0].Position;
                DrawElem[ID].Rotation = Rails[ID][0].Rotation;
            }
        }
    }
}