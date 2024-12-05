using Godot;
using Godot.Collections;
using System;

public abstract partial class Life : CharacterBody3D, ICharacter
{
    const int normalGravityForce = 40;
    const int normalMaxSpeedDown = 20;
    const int normalStopSpeed = 20;
    const int normalSpeed = 4;
    int additionalSpeed;
    int additionalGravity;

    public void ApplyGravityForce(float delta)
    {
        if (Velocity.Y >= -normalMaxSpeedDown)
        {
            Velocity = Velocity with { Y = (float)(Velocity.Y - (normalMaxSpeedDown * delta)) };
        }
        else
        {
            Velocity = Velocity with { Y = -normalMaxSpeedDown };
        }
    }
    public int HealthPoint { get => HealthPoint; set => HealthPoint = value; }

    public void ChangeHealth(int value, int fromWho)
    {
        HealthPoint += value;
    }

    public void Die()
    {
        QueueFree();
    }

    RayCast3D VisionRay;
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
