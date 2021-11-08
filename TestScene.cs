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
    public GlobalPhysUpdater Updater = new GlobalPhysUpdater();

    PackedScene NewObject = (PackedScene)ResourceLoader.Load("res://TestObjectRes.tscn");

    PackedScene GravCenter = (PackedScene)ResourceLoader.Load("res://GravityCenter.tscn");

    Camera2D Camera;

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
    
    void GlobalRailUpdaterSetup(){
        Updater.RailController.SetGlobalCount(100);
        Updater.RailController.SetInterval(0.05f);
    }

    /// <summary>
    /// Метод обновления массового теста рельс
    /// </summary>
    /// <param name="delta">интервал времени, который надо обновить</param>
    void MassRailTestUpdate(){
        Updater.MoveToWatcher();
    } 

    void AddTestResource(){
        GD.Print("Adding");
        Node2D Object = (Node2D)NewObject.Instance();
        this.AddChild(Object);
        Camera = GetNode<Camera2D>("TestCamera");
        Camera.SmoothingEnabled = false;
        this.RemoveChild(Camera);
        Object.AddChild(Camera);
        GD.Print("Added");
    }

    void AddTestResourceArray(){
        Node2D[] Array = new Node2D[2];
        for (int i = 0; i < Array.Length; i++)
        {
            Array[i] = (Node2D)NewObject.Instance();
            this.AddChild(Array[i]);
        }
    }

    void AddObjects(){
        Node2D[] Array = new Node2D[2];
        for (int i = 0; i < Array.Length; i++)
        {
            Array[i] = (Node2D)NewObject.Instance();
            this.AddChild(Array[i]);
        }
    }

    void AddGravityCenter(){
        Node2D Object = (Node2D)GravCenter.Instance();
        this.AddChild(Object);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddTestResource();
        AddGravityCenter();
        AddTestResourceArray();
        System.Threading.ThreadPool.SetMinThreads(100,100);
        GlobalRailUpdaterSetup();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Updater.Watcher.Shift+=delta;
    }

    public override void _PhysicsProcess(float delta)
    {
        MassRailTestUpdate();
    }

    public override void _UnhandledInput(InputEvent @event){
        if (@event is InputEventKey){
            InputEventKey emb = (InputEventKey)@event;
            if (emb.IsPressed()){
                if (emb.Unicode == (int)KeyList.Space){
                    AddObjects();
                }
            }
        }
    }
}
