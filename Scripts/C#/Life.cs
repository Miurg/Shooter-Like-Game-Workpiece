using Godot;
using Godot.Collections;
using player;
using System;

public abstract partial class Life : CharacterBody3D, ICharacter
{
    public const int NormalGravityForce = 40;
    public const int NormalMaxSpeedDown = 20;
    public const int NormalStopSpeed = 20;
    public const int NormalMoveSpeed = 4;
    protected int AdditionalMoveSpeed;
    protected int AdditionalGravity;
    protected int MaxMoveSpeed;
    private PhysicsDirectSpaceState3D SpaceState;
    protected MainNode MainNode;
    public abstract Dictionary GetWeaponRay(uint CollisionMask, Vector3 NewRayTarget);
    public abstract void ChangeHealth(int value, Life fromWho);
    public abstract void Die();
    public void ApplyGravityForce(float delta)
    {
        if (Velocity.Y >= -NormalMaxSpeedDown)
        {
            Velocity = Velocity with { Y = (float)(Velocity.Y - (NormalGravityForce+AdditionalGravity * delta)) };
        }
        else
        {
            Velocity = Velocity with { Y = -NormalMaxSpeedDown };
        }
    }

    public void MainCreateRemains(CollisionObject3D CollisionObject, Vector3 PositionOfHole, Vector3 NormalOfHole, PackedScene Remains)
    {
        MainNode.CreateRemains(CollisionObject, PositionOfHole, NormalOfHole, Remains);
    }
    public int HealthPoint { get => HealthPoint; set => HealthPoint = value; }


    protected RayCast3D VisionRay;
    //protected Godot.Collections.Array GetRayVision(uint collisionMask, Vector3 newRayTarget)
    //{
    //    VisionRay.CollisionMask = collisionMask;
    //    VisionRay.TargetPosition = newRayTarget;
    //    if (VisionRay.IsColliding() == true)
    //    {
    //        return [VisionRay.GetCollider(), VisionRay.GetCollisionPoint(), VisionRay.GetCollisionNormal()];
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

}
