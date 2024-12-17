using Godot;
using Godot.Collections;
using player;
using System;

public abstract partial class NPCAngry : NPC
{
    protected LifeWeapons MasterWeapon;
    protected Node3D Mesh;
    protected NavigationAgent3D NavAgent;
    protected Vector3 TargetToMove;
    [Export] protected float TimeUntilUnsee;
    protected float CurrentTimeSee;
    protected Vector3 NextPath;
    protected Vector3 PreviousPath;
    protected Vector3 TryingLookTo;
    public bool playerVisible = false;
    public bool playerVisibleFromTime = false;

    protected void Movement(float delta)
    {
        if ((MasterWeapon.CurrentWeapon != null) && (Position.DistanceTo(Player.Position) < MasterWeapon.CurrentWeapon.MaxDistanceForNPC) && IsPlayerVisible())
        {
            MasterWeapon.CurrentlyAttack = true;
            Velocity = Velocity.Lerp(new Vector3(0, Velocity.Y, 0), delta * NormalStopSpeed);
            LookAt(Player.Position);
        }
        else if (!NavAgent.IsNavigationFinished() || IsPlayerVisible())
        {
            MasterWeapon.CurrentlyAttack = false;
            Velocity = Velocity.Lerp((NextPath - GlobalPosition).Normalized() * MaxMoveSpeed, delta * (NormalMoveSpeed + AdditionalMoveSpeed)) with { Y = Velocity.Y };
            Vector3 newLook = new Vector3(NextPath.X,Position.Y, NextPath.Z);
            if (newLook!=Position)
            {
                TryingLookTo = newLook;
            }
        }
        else
        {
            Velocity = Velocity.Lerp(new Vector3(0,Velocity.Y,0), delta * NormalStopSpeed);
        }
    }



    public override Dictionary GetWeaponRay(uint CollisionMask, Vector3 NewRayTarget)
    {
        Vector3 rayStart = VisionRay.GlobalPosition;
        Vector3 rayEnd = rayStart + NewRayTarget.Rotated(new Vector3(1, 0, 0), Rotation.X).Rotated(new Vector3(0, 1, 0), Rotation.Y);
        Dictionary newGeneralRay = SpaceState.IntersectRay(PhysicsRayQueryParameters3D.Create(rayStart, rayEnd, CollisionMask));
        if (newGeneralRay.ContainsKey("collider"))
        {
            return newGeneralRay;
        }
        else
        {
            return null;
        }
    }

    public override void ChangeHealth(int value, Life fromWho)
    {
        HealthPoint -= value;
        SeekForDamageDealer(fromWho);
        if (HealthPoint < 0)
        {
            Die();
        }
    }

    protected void SeekForDamageDealer(Life FromWho)
    {
        if (FromWho.Name == "Player")
        {
            TryingLookTo = new Vector3(FromWho.Position.X, Position.Y, FromWho.Position.Z);
        }
    }
    public override void Die()
    {
        if (MasterWeapon.CurrentWeapon!=null)
        {
            Weapon newWeapon = ResourceLoader.Load<PackedScene>(MasterWeapon.CurrentWeapon.SceneFilePath).Instantiate<Weapon>();
            newWeapon.CurrentRounds = MasterWeapon.CurrentWeapon.CurrentRounds;
            MainPlaceWeapon(newWeapon);
        }
        this.QueueFree();
    }
    public override void _PhysicsProcess(double delta)
    {
        if (IsPlayerVisible())
        {
            playerVisible = true;
            CurrentTimeSee = TimeUntilUnsee;
        }
        else playerVisible = false;

        if (CurrentTimeSee > 0)
        {
            playerVisibleFromTime = true;
            NavAgent.TargetPosition = Player.Position;
            CurrentTimeSee -= (float)delta;
        }
        else playerVisibleFromTime = false;

        if (!NavAgent.IsNavigationFinished()) NextPath = NavAgent.GetNextPathPosition();
    }

    public override void _Process(double delta)
    {
        SlowLookAt(TryingLookTo, (float)delta);
        Movement((float)delta);
        if (!IsOnFloor())
        {
            ApplyGravityForce((float)delta);
        }
        MoveAndSlide();
    }
}
