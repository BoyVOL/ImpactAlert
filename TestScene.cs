using Godot;
using RailSystem;
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
    /// Класс для моделирования движения с постоянным ускорением
    /// </summary>
    public class ConstantAccelPoint : KineticPoint{

        public Vector2 Accel;
        
        public ConstantAccelPoint(Vector2 pos, float rot) : base(pos,rot){
            Accel = Vector2.Zero;
        }

        public ConstantAccelPoint(Vector2 pos, float rot, Vector2 simSpeed, Vector2 accel, float simRotSpeed = 0) : base(pos,rot,simSpeed,simRotSpeed){
            Accel = accel;
        }

        public override RailPoint CreateNextPoint(float T)
        {
            return new ConstantAccelPoint(Position+SimSpeed*T+(Accel*T*T)/2,Rotation+SimRotSpeed*T,SimSpeed+Accel*T,Accel,SimRotSpeed);
        }
    }
    
    /// <summary>
    /// Метод для проверки столкновения двух рельс
    /// </summary>
    public void RailDistanceTest(){
        //Блаблаблабла
        TestRail.SetInterval((float)0.5);
        TestRail2.SetInterval((float)0.5);
        TestRail.SetFirstPoint(new ConstantAccelPoint(Vector2.Zero,0,new Vector2(3,10),new Vector2(-0.2f,0)));
        TestRail2.SetFirstPoint(new ConstantAccelPoint(new Vector2(20,0),0,new Vector2(-3,10),new Vector2(0.2f,0)));
        TestRail.Extrapolate(100);
        TestRail2.Extrapolate(100);
        float[] Below1 = TestRail.Approach(TestRail2,1);
        foreach (var item in Below1)
        {
            GD.Print(item,TestRail.Interpolate(item).Position,TestRail2.Interpolate(item).Position);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("BLA");
        RailDistanceTest();
        //RailTest();
        ///CPATest(10);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
