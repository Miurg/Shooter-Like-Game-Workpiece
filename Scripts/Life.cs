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
    [Export]  public int HealthPoint;
    [Export] protected int AdditionalMoveSpeed;
    [Export] protected int AdditionalGravity;
    [Export] protected int MaxMoveSpeed;
    protected PhysicsDirectSpaceState3D SpaceState;
    protected MainNode MainNode;
    public abstract Dictionary GetWeaponRay(uint CollisionMask, Vector3 NewRayTarget);
    public abstract void ChangeHealth(int value, Life fromWho);
    public abstract void Die();
    public void ApplyGravityForce(float delta)
    {
        if (Velocity.Y >= -NormalMaxSpeedDown)
        {
            Velocity = Velocity with { Y = (float)(Velocity.Y - ((NormalGravityForce+AdditionalGravity) * delta)) };
        }
        else
        {
            Velocity = Velocity with { Y = -NormalMaxSpeedDown };
        }
    }

    public virtual void MainPlaceWeapon(RigidBody3D Weapon)
    {
        Vector3 weaponPosition = this.GlobalPosition;
        Vector3 weaponImpulse = new Vector3 (0,0,0);
        MainNode.PlaceWeapon(this, Weapon, weaponImpulse, weaponPosition);
    }

    public void MainCreateRemainsFromWeapon(CollisionObject3D CollisionObject, Vector3 PositionOfHole, Vector3 NormalOfHole, PackedScene Remains)
    {
        MainNode.CreateRemainsFromWeapon(CollisionObject, PositionOfHole, NormalOfHole, Remains);
    }


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
