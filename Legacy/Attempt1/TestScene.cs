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

    System.Threading.Thread Thread1;
    
    System.Threading.Thread Thread2;

    public GlobalPhysUpdater Updater2 = new GlobalPhysUpdater();

    Node2D FocusObject = null;

    PackedScene NewObject = (PackedScene)ResourceLoader.Load("res://TestScenes/TestObjectRes.tscn");

    PackedScene GravCenter = (PackedScene)ResourceLoader.Load("res://TestScenes/GravityCenter.tscn");

    PackedScene GravCenter2 = (PackedScene)ResourceLoader.Load("res://TestScenes/GravityCenter2.tscn");

    PackedScene Arrow = (PackedScene)ResourceLoader.Load("res://TestScenes/TestArrow.tscn");

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
        Updater2.RailController.SetGlobalCount(100);
        Updater2.RailController.SetInterval(0.05f);
    }

    /// <summary>
    /// Метод обновления массового теста рельс
    /// </summary>
    /// <param name="delta">интервал времени, который надо обновить</param>
    void MassRailTestUpdate(){
        Updater.MoveToWatcher();
        Updater2.MoveToWatcher();
    } 



    void AddTestResource(){
        GD.Print("Adding");
        FocusObject = (Node2D)NewObject.Instance();
        this.AddChild(FocusObject);
        Camera = GetNode<Camera2D>("TestCamera");
        Camera.SmoothingEnabled = false;
        this.RemoveChild(Camera);
        FocusObject.AddChild(Camera);
        GD.Print("Added");
    }

    void AddTestResourceArray(){
        Node2D[] Array = new Node2D[50];
        for (int i = 0; i < Array.Length; i++)
        {
            Array[i] = (Node2D)Arrow.Instance();
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
        Node2D Object2 = (Node2D)GravCenter2.Instance();
        this.AddChild(Object2);
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
        Updater2.Watcher.Shift=Updater.Watcher.Shift;
    }

    public override void _PhysicsProcess(float delta)
    {
        Thread1 = new System.Threading.Thread(Updater.MoveToWatcher);
        Thread1.Start();
        Thread2 = new System.Threading.Thread(Updater2.MoveToWatcher);
        Thread2.Start();
        Thread1.Join();
        Thread2.Join();
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
