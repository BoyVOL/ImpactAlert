using Godot;
using RailSystem;
using ForceProjection;
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
    ForceRail[] MassRail;

    /// <summary>
    /// Спрайты для отображения интерполяций рельс на экране
    /// </summary>
    SpriteBufferArray MassRailSpriteArr;

    /// <summary>
    /// Массив следователей по рельсам для тестовых рельс
    /// </summary>
    RailFollower[] MassRailF;

    TestForceProjector TestProjector = new TestForceProjector(new Vector2(500,300),100000);

    ForceProjHandler TestHandler = new ForceProjHandler();
    /// <summary>
    /// Тестовый класс точки рельсы
    /// </summary>

    class TestForceProjector : ForceProjector{
        Vector2 Pos;

        float Potential;

        public TestForceProjector(Vector2 pos, float potential){
            Pos = pos;
            Potential = potential;
        }

        float Force(float r, float potential){
            return potential/(r*r);
        }
        public override Vector2 GetAccelVector(ForceParams forceParams, float T){
            float R = Pos.DistanceTo(forceParams.Pos);
            Vector2 Normal = (Pos - forceParams.Pos);
            Normal.Normalized();
            float module = Potential/(R*R);
            return Normal*module;
        }
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

    void TestProjection(){
        GD.Print(TestProjector.GetAccelVector(new ForceParams(Vector2.Zero,0),0));
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
        TestRail.SetFirstPoint(new AccelPoint(Vector2.Zero,0,new Vector2(3,10),new Vector2(-0.2f,0)));
        TestRail2.SetFirstPoint(new AccelPoint(new Vector2(20,0),0,new Vector2(-3,10),new Vector2(0.2f,0)));
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

    void ForceSetup(){
        TestHandler.AddProjector(TestProjector);
    }

    void PreWritedRail(float TimeInterval = 0.1f, int raillength = 43){
        

        Random Rnd = new Random();
        
        Vector2 newPos = new Vector2(10,10);
        Vector2 newSpeed = new Vector2(0,100);
        Vector2 newAccel = new Vector2(10,10);

        ForceParams par = new ForceParams(Vector2.Zero,10);

        MassRail = new ForceRail[1];

        MassRailF = new RailFollower[1];

        ForceRail Temp;

        MassRail[0] = new ForceRail();
        MassRail[0].Handler = TestHandler;
        MassRail[0].SetInterval(TimeInterval);
        MassRail[0].SetFirstPoint(new AccelPoint(Position,(float)(Rnd.NextDouble()*Math.PI*2),newSpeed,newAccel,(float)(Rnd.NextDouble()*2-1)));
        Temp = (ForceRail)MassRail[0];
        Temp.ExtrapolateForce(raillength,par);
        MassRailF[0] = MassRail[0].GetRailFollower();
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
        int ArraySize = 1000, float posRange = 1000, 
        float SpeedRange = 100, float AccelRange = 100, float TimeInterval = 0.01f, int raillength = 4000){

        ForceRail Temp;
        
        ForceParams par = new ForceParams(Vector2.Zero,10);

        Random Rnd = new Random();

        MassRail = new ForceRail[ArraySize];

        MassRailF = new RailFollower[ArraySize];

        for (int i = 0; i < MassRail.Length; i++)
        {
            Vector2 newPos = new Vector2((float)Rnd.NextDouble()*posRange,(float)Rnd.NextDouble()*posRange);
            //Vector2 newPos = new Vector2(20,20);
            Vector2 newSpeed = new Vector2((float)(Rnd.NextDouble()*SpeedRange*2-SpeedRange),(float)(Rnd.NextDouble()*SpeedRange*2-SpeedRange));
            Vector2 newAccel = new Vector2((float)Rnd.NextDouble()*AccelRange*2-AccelRange,(float)Rnd.NextDouble()*AccelRange*2-AccelRange);
            //Vector2 newAccel = new Vector2(1,0);
            MassRail[i] = new ForceRail();
            MassRail[i].Handler = TestHandler;
            MassRail[i].SetInterval(TimeInterval);
            MassRail[i].SetFirstPoint(new AccelPoint(newPos,(float)(Rnd.NextDouble()*Math.PI*2),newSpeed,newAccel,(float)(Rnd.NextDouble()*2-1)));
            Temp = (ForceRail)MassRail[i];
            Temp.ExtrapolateForce(raillength,par);
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
        ForceRail Temp;
        ForceParams par = new ForceParams(Vector2.Zero,10);
        for (int i = 0; i < MassRail.Length; i++)
        {
            MassRailF[i].TimeShift += delta;
            if(MassRailF[i].CurrentID() > 0){
                int DeletedCount = MassRailF[i].CurrentID();
                MassRailF[i].RemoveBehind(DeletedCount);
                Temp = (ForceRail)MassRailF[i].Current;
                Temp.ExtrapolateForce(DeletedCount,par);
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
        ForceSetup();
        //PreWritedRail();
        MassRailTestSetup();
        SpriteSetup();
        GD.Print("BLA");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        MassRailTestUpdate(delta);
        MassRailSpriteUpdate();
    }
}
