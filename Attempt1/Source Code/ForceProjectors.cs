using Godot;
using RailSystem;
using ForceProjection;

public class GravityProjector : ForceProjector{
        Vector2 Pos;

        float Potential;

        public GravityProjector(Vector2 pos, float potential){
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

public class GravityRailProjector : ForceProjector{
        Rail Rail;

        float Potential;

        public GravityRailProjector(Rail rail, float potential){
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