using Godot;
using player;
using System;

public abstract partial class NPC : Life
{
    protected PlayerMain Player;
    protected int FieldOfView;
    protected int MaxDistanceOfView;
    float RotationSpeed = (float)Math.PI;


    public bool IsPlayerVisible()
    {
        if (Position.DistanceTo(Player.Position)<MaxDistanceOfView)
        {
            Vector3 thisDirection = -GlobalTransform.Basis.Z.Normalized();
            Vector3 toTargetDirection = Position.DirectionTo(Player.Position);
            float angle = Mathf.RadToDeg(Mathf.Acos(thisDirection.Dot(toTargetDirection)));
            if (angle<=FieldOfView/2)
            {
                return (GetRayVision(0b00000000_00000000_00000000_00000001, ToLocal(Player.GlobalPosition))) == null;
            }
        }
        return false; 
    }

    public void SlowLookAt(Vector3 NewLook, float delta)
    {
        Vector2 vectorFromTo = new Vector2(Position.Z, Position.X) - new Vector2(NewLook.Z,NewLook.X);
        float angle = vectorFromTo.Angle();
        float r = Rotation.Y;
        float angleDelta = RotationSpeed * delta;
        angle = Mathf.LerpAngle(r, angle, 1);
        angle = Mathf.Clamp(angle,r-angleDelta,r+angleDelta);
        Rotation = new Vector3(Rotation.X,angle,Rotation.Z);
    }
    protected Godot.Collections.Array GetRayVision(uint collisionMask, Vector3 newRayTarget)
    {
        VisionRay.CollisionMask = collisionMask;
        VisionRay.TargetPosition = newRayTarget;
        if (VisionRay.IsColliding() == true)
        {
            return [VisionRay.GetCollider(), VisionRay.GetCollisionPoint(), VisionRay.GetCollisionNormal()];
        }
        else
        {
            return null;
        }
    }
}
