using Godot;
using RailSystem;
using Legacy;
using System;

/// <summary>
/// Сцена для массового тестирования рельс
/// </summary>
public class TestScene : Node2D
{  
    /// <summary>
    /// класс массив рельс для массового тестирования
    /// </summary>
    Rail[] MassRail;

    /// <summary>
    /// Спрайты для отображения интерполяций рельс на экране
    /// </summary>
    SpriteBufferArray MassRailSpriteArr;

    /// <summary>
    /// Массив следователей по рельсам для тестовых рельс
    /// </summary>
    RailFollower[] MassRailF;
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

    /// <summary>
    /// Метод для проверки работы механизма нахождения точки максимального сближения объектов
    /// </summary>
    /// <param name="T"></param>
    void CPATest(float T){
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
    void RailTest(){
        
        Rail TestRail = new Rail();

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
    class ConstantAccelPoint : KineticPoint{

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
    void RailDistanceTest(){
        
        Rail TestRail = new Rail();
  
        Rail TestRail2 = new Rail();
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

    /// <summary>
    /// Метод для тестирования итератора рельсы
    /// </summary>
    void testRailFollower(){
        Rail Test = new Rail();
        
        RailFollower TestFollow = Test.GetRailFollower();

        Test.SetFirstPoint(new KineticPoint(Vector2.Zero,0,new Vector2(10,10),1));
        Test.SetInterval(1);
        Test.Extrapolate(10);
        
        for (int i = 0; i < 10; i++)
        {
            TestFollow.TimeShift += 0.5f;
            GD.Print(TestFollow.GetInterpolation().toString());
        }
        TestFollow.TimeShift = 2.5f;
        GD.Print("Count = ",Test.GetCount());
            GD.Print(TestFollow.GetInterpolation().toString());
        GD.Print(TestFollow.TimeShift);
        TestFollow.RemoveBehind(2);
        GD.Print("Count = ",Test.GetCount());
            GD.Print(TestFollow.GetInterpolation().toString());
        GD.Print(TestFollow.TimeShift);
    }

    /// <summary>
    /// Метод для настройки тестовой симуляции множества объектов с рельсами
    /// </summary>
    /// <param name="ArraySize"></param>
    /// <param name="posRange"></param>
    /// <param name="SpeedRange"></param>
    /// <param name="AccelRange"></param>
    /// <param name="TimeInterval"></param>
    void MassRailTestSetup(
        int ArraySize = 10000, float posRange = 1000, 
        float SpeedRange = 100, float AccelRange = 100, float TimeInterval = 0.1f, int raillength = 100){

        Random Rnd = new Random();

        MassRail = new Rail[ArraySize];

        MassRailF = new RailFollower[ArraySize];

        for (int i = 0; i < MassRail.Length; i++)
        {
            Vector2 newPos = new Vector2((float)Rnd.NextDouble()*posRange,(float)Rnd.NextDouble()*posRange);
            Vector2 newSpeed = new Vector2((float)(Rnd.NextDouble()*SpeedRange*2-SpeedRange),(float)(Rnd.NextDouble()*SpeedRange*2-SpeedRange));
            Vector2 newAccel = new Vector2((float)Rnd.NextDouble()*AccelRange*2-AccelRange,(float)Rnd.NextDouble()*AccelRange*2-AccelRange);
            //Vector2 newAccel = new Vector2(1,0);
            MassRail[i] = new Rail();
            MassRail[i].SetInterval(TimeInterval);
            MassRail[i].SetFirstPoint(new ConstantAccelPoint(newPos,(float)(Rnd.NextDouble()*Math.PI*2),newSpeed,newAccel,(float)(Rnd.NextDouble()*2-1)));
            MassRail[i].Extrapolate(raillength);
            MassRailF[i] = MassRail[i].GetRailFollower();
        }
    }

    /// <summary>
    /// Метод для задания массива спрайтов
    /// </summary>
    void SpriteSetup(){
        Texture Texture = ResourceLoader.Load<Texture>("res://icon.png");
        MassRailSpriteArr = new SpriteBufferArray(MassRail.Length);
        for (int i = 0; i < MassRailSpriteArr.getLength(); i++)
        {
            MassRailSpriteArr.AddAsChild(i,this);
            MassRailSpriteArr.SetTexture(i,Texture);
            MassRailSpriteArr.SetVisible(i,true);
            MassRailSpriteArr.SetCoordinates(i,MassRail[i].Interpolate(0).Position);
            //MassRailSpriteArr.SetSize(i,new Vector2(1,1));
        }
    }


    /// <summary>
    /// Метод обновления массового теста рельс
    /// </summary>
    /// <param name="delta">интервал времени, который надо обновить</param>
    void MassRailTestUpdate(float delta){
        for (int i = 0; i < MassRail.Length; i++)
        {
            MassRailF[i].TimeShift += delta;
            if(MassRailF[i].CurrentID() > 0){
                int DeletedCount = MassRailF[i].CurrentID();
                MassRailF[i].RemoveBehind(DeletedCount);
                MassRailF[i].Current.Extrapolate(DeletedCount);
            }
        }
    } 

    /// <summary>
    /// Метод для обновления положения спрайтов
    /// </summary>
    void MassRailSpriteUpdate(){
        for (int i = 0; i < MassRailSpriteArr.getLength(); i++)
        {
            MassRailSpriteArr.SetCoordinates(i,MassRailF[i].GetInterpolation().Position);
            MassRailSpriteArr.SetRotation(i,MassRailF[i].GetInterpolation().Rotation);
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("BLA");
        MassRailTestSetup();
        SpriteSetup();
        //RailDistanceTest();
        //RailTest();
        ///CPATest(10);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        MassRailTestUpdate(delta);
        MassRailSpriteUpdate();
    }
}
