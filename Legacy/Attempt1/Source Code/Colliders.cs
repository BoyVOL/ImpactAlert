    using Godot;
    using CollisionCalculation;
    
    /// <summary>
    /// Тестовый класс точки рельсы
    /// </summary>
    public class TestCollider : RailCollider{
        public override CollisionResults CollisionRes(RailCollider Other, float T)
        {
            CollisionResults Result = new CollisionResults();
            //GD.Print(Current.Interpolate(T).Position.DistanceTo(Other.Current.Interpolate(T).Position));
            Result.T = T;
            AccelPoint Point = (AccelPoint)Other.Current.GetPoint(Other.Current.IDFromTime(T));
            Result.NewSpeed = Point.SimSpeed;
            return Result;
        }

        public override bool ApplyResults(CollisionResults Results)
        {
            int CollId = Current.IDFromTime(Results.T);
            AccelPoint Point = (AccelPoint)Current.GetPoint(CollId);
            Point.SimSpeed = Results.NewSpeed;
            return true;
            //GD.Print(Results.T);
        }
    }