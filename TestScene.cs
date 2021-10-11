using Godot;
using System;

public class TestScene : Node2D
{
    /// <summary>
    /// Тестовый класс точки рельсы
    /// </summary>
    class TestPoint : RailPoint{

        public TestPoint(Vector2 pos, float rot = 0){
            Position = pos;
            Rotation = rot;
        }
        public override RailPoint CreateNextPoint(float T)
        {
            return new TestPoint(Position,Rotation);
        }
    }

    Rail TestRail = new Rail();

    Rail TestRail2 = new Rail();

    /// <summary>
    /// Метод для проверки работы механизма нахождения точки максимального сближения объектов
    /// </summary>
    /// <param name="T"></param>
    public void CPATest(float T){
        TestPoint A1 = new TestPoint(new Vector2(0,0));
        TestPoint A2 = new TestPoint(new Vector2(0,10));
        TestPoint B1 = new TestPoint(new Vector2(20,0));
        TestPoint B2 = new TestPoint(new Vector2(30,0));
        A1.InterpolateTo(A1,T);
        B1.InterpolateTo(B2,T);
        float ClosestT = A1.CPA(B1);
        GD.Print("Время пересечения точек - ",ClosestT);
        GD.Print(A1.GetInterpol(ClosestT).Position);
        GD.Print(B1.GetInterpol(ClosestT).Position);
    }

    /// <summary>
    /// метод для проверки работы класса рельсы
    /// </summary>
    public void RailTest(){
        TestRail.SetFirstPoint(new KineticPoint(Vector2.Zero,0,new Vector2(10,10)));
        TestRail.SetInterval(1);
        TestRail.Extrapolate(10);
        for (int i = 0; i < 10; i++)
        {
            GD.Print(i);
            GD.Print(TestRail.GetPoint(i).Position);
        }
        GD.Print("Смена интервала на 10");
        TestRail.SetInterval(10);
        for (int i = 0; i < 10; i++)
        {
            GD.Print(i);
            GD.Print(TestRail.GetPoint(i).Position);
        }
        GD.Print(TestRail.Interpolate(55).Position);
    }

    /// <summary>
    /// Метод для проверки столкновения двух рельс
    /// </summary>
    public void RailCollisionTest(){
        TestRail.SetFirstPoint(new KineticPoint(Vector2.Zero,0,new Vector2(10,10)));
        TestRail.SetFirstPoint(new KineticPoint(Vector2.Zero,0,new Vector2(10,10)));
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("BLA");
        RailTest();
        ///CPATest(10);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
