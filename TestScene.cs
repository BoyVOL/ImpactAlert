using Godot;
using RailSystem;
using ForceProjection;
using CollisionCalculation;
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

    ForceProjector[] Projectors;

    TestCollider[] Colliders;

    /// <summary>
    /// Спрайты для отображения интерполяций рельс на экране
    /// </summary>
    SpriteBufferArray MassRailSpriteArr;

    GlobalRailController RailController = new GlobalRailController();

    GlobalCollider Collider = new GlobalCollider();

    /// <summary>
    /// Массив следователей по рельсам для тестовых рельс
    /// </summary>
    RailFollower[] MassRailF;

    ForceProjHandler TestHandler = new ForceProjHandler();
    
    /// <summary>
    /// Тестовый класс точки рельсы
    /// </summary>

    class TestForceProjector : ForceProjector{
        Rail Rail;

        float Potential;

        public TestForceProjector(Rail rail, float potential){
            Rail = rail;
            Potential = potential;
        }

        float Force(float r, float potential){
            return potential/(r*r);
        }
        public override Vector2 GetAccelVector(ForceParams forceParams, float T){
            Vector2 Pos = Rail.Interpolate(T).Position;
            float R = Pos.DistanceTo(forceParams.Pos);
            Vector2 Normal = (Pos - forceParams.Pos);
            Normal.Normalized();
            float module = Potential/(R*R);
            return Normal*module;
        }
    }

    class TestCollider : RailCollider{
        public override CollisionResults CollisionRes(RailCollider Other, float T)
        {
            CollisionResults Result = new CollisionResults();
            //GD.Print(Current.Interpolate(T).Position.DistanceTo(Other.Current.Interpolate(T).Position));
            Result.T = T;
            AccelPoint Point = (AccelPoint)Other.Current.GetPoint(Other.Current.IDFromTime(T));
            Result.NewSpeed = Point.SimSpeed;
            return Result;
        }

        public override void ApplyResults(CollisionResults Results)
        {
            int CollId = Current.IDFromTime(Results.T);
            AccelPoint Point = (AccelPoint)Current.GetPoint(CollId);
            Point.SimSpeed = Results.NewSpeed;
            int ExtrapolateCount = Current.GetCount()-CollId-1;
            Current.RemoveFromEnd(ExtrapolateCount);
            Current.Extrapolate(ExtrapolateCount);
            //GD.Print(Results.T);
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

    /*void TestProjection(){
        

        TestForceProjector TestProjector = new TestForceProjector(new Vector2(500,300),100000);

        GD.Print(TestProjector.GetAccelVector(new ForceParams(Vector2.Zero,0),0));
    }*/
    
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

        GlobalRailController TestContr = new GlobalRailController();

        TestContr.AddRail(Test);
        
        RailFollower TestFollow = TestContr.GetRailFollower(Test);

        Test.SetFirstPoint(new KineticPoint(Vector2.Zero,0,new Vector2(10,10),1));
        Test.SetInterval(1);
        Test.Extrapolate(10);
        
        for (int i = 0; i < 10; i++)
        {
            TestFollow.Shift += 0.5f;
            GD.Print(TestFollow.GetInterpolation().toString());
        }
        TestContr.ShiftT = 2.5f;
        GD.Print("Count = ",Test.GetCount());
            GD.Print(TestFollow.GetInterpolation().toString());
        GD.Print(TestContr.ShiftT);
        TestContr.MoveForvard(2);
        GD.Print("Count = ",Test.GetCount());
            GD.Print(TestFollow.GetInterpolation().toString());
        GD.Print(TestContr.ShiftT);
    }

    void ForceSetup(){        
        Rail Rail = new Rail();
        Rail.SetFirstPoint(new KineticPoint(new Vector2(0,0),0,new Vector2(0,0),0));
        RailController.AddRail(Rail);
        TestForceProjector TestProjector = new TestForceProjector(Rail,100000);
        TestHandler.AddProjector(TestProjector);
    }

    void MassForceSetup(){
        Projectors = new ForceProjector[MassRail.Length];
        for (int i = 0; i < Projectors.Length; i++)
        {
            Projectors[i] = new TestForceProjector(MassRail[i],100000);
            TestHandler.AddProjector(Projectors[i]);
            MassRail[i].Params.Exclude = new ForceProjector[1];
            MassRail[i].Params.Exclude[0] = Projectors[i];
        }
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
        Temp.Extrapolate(raillength);
        RailController.AddRail(MassRail[0]);
        MassRailF[0] = RailController.GetRailFollower(MassRail[0]);
    }
    
    void GlobalRailUpdaterSetup(){
        RailController.SetGlobalCount(100);
        RailController.SetInterval(0.01f);
    }

    /// <summary>
    /// Метод для установки классов обработки коллизий в тестовых рельсах
    /// </summary>
    /// <param name="radius">Рабиус коллайдеров</param>
    void ColliderSetup(float radius = 100){
        Colliders = new TestCollider[MassRail.Length];
        for (int i = 0; i < Colliders.Length; i++)
        {
            Colliders[i] = new TestCollider();
            Colliders[i].Current = MassRail[i];
            Colliders[i].Radius = radius;
            Collider.AddCollider(Colliders[i]);
        }
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
        int ArraySize = 3, float posRange = 1000, 
        float SpeedRange = 100, float AccelRange = 100){
        
        ForceParams par = new ForceParams(Vector2.Zero,10);

        Random Rnd = new Random();

        MassRail = new ForceRail[ArraySize];

        MassRailF = new RailFollower[ArraySize];

        for (int i = 0; i < MassRail.Length; i++)
        {
            Vector2 newPos = new Vector2((float)Rnd.NextDouble()*posRange*2-posRange,(float)Rnd.NextDouble()*posRange*2-posRange);
            //Vector2 newPos = new Vector2(20,20);
            Vector2 newSpeed = new Vector2((float)(Rnd.NextDouble()*SpeedRange*2-SpeedRange),(float)(Rnd.NextDouble()*SpeedRange*2-SpeedRange));
            Vector2 newAccel = new Vector2((float)Rnd.NextDouble()*AccelRange*2-AccelRange,(float)Rnd.NextDouble()*AccelRange*2-AccelRange);
            //Vector2 newAccel = new Vector2(1,0);
            MassRail[i] = new ForceRail();
            MassRail[i].Handler = TestHandler;
            MassRail[i].SetFirstPoint(new AccelPoint(newPos,(float)(Rnd.NextDouble()*Math.PI*2),newSpeed,newAccel,(float)(Rnd.NextDouble()*2-1)));
        }
        RailController.AddRail(MassRail);
        for (int i = 0; i < MassRail.Length; i++)
        {
            MassRailF[i] = RailController.GetRailFollower(MassRail[i]);
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
        ForceParams par = new ForceParams(Vector2.Zero,100);
        for (int i = 0; i < MassRail.Length; i++)
        {
            MassRailF[i].Shift += delta;
        }
        int DeletedCount = MassRailF[0].CurrentID();
        RailController.MoveForvard(DeletedCount);
    } 

    /// <summary>
    /// Метод для обновления положения спрайтов
    /// </summary>
    void MassRailSpriteUpdate(){
        for (int i = 0; i < MassRailSpriteArr.getLength(); i++)
        {
            MassRailSpriteArr.SetCoordinates(i,MassRailF[i].GetInterpolation().Position);
            MassRailSpriteArr.SetRotation(i,MassRailF[i].GetInterpolation().Rotation);
            MassRailSpriteArr.SetSize(i,new Vector2(2.5f,2.5f));
        }
    }

    void CollidersUpdate(int startID = 0){
        Collider.GlobalCollProcess(1);
    }
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        System.Threading.ThreadPool.SetMinThreads(100,100);
        GlobalRailUpdaterSetup();
        MassRailTestSetup();
        MassForceSetup();
        ForceSetup();
        //PreWritedRail();
        SpriteSetup();
        ColliderSetup();
        GD.Print("BLA");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        MassRailTestUpdate(delta);
        MassRailSpriteUpdate();
        CollidersUpdate();
    }
}
