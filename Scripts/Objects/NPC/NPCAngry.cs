using Godot;
using Godot.Collections;
using player;
using System;
using System.Security;

public abstract partial class NPCAngry : NPC
{
    [Export] protected Path3D PatrolPath;
    protected Vector3 NextPatrolPoint;
    protected bool IsInPatrol = true;
    protected int NumberOfPatrolPointCurrent = 0;
    protected RayCast3D WeaponRay;
    protected LifeWeapons MasterWeapon;
    protected NavigationAgent3D NavAgent;
    protected Vector3 TargetToMove;
    [Export] protected float TimeUntilUnsee;
    protected float CurrentTimeSee;
    protected Vector3 NextPath;
    protected Vector3 PreviousPath;
    protected Vector3 TryingLookTo;
    public bool playerVisible = false;
    public bool playerVisibleFromTime = false;

    protected override void Movement(float delta)
    {
        if (IsInPatrol)
        {
            if (!NavAgent.IsNavigationFinished())
            {
                Mesh.Set("curAnim", 1);
                MasterWeapon.CurrentlyAttack = false;
                Velocity = Velocity.Lerp((NextPath - GlobalPosition).Normalized() * MaxMoveSpeed/2,
                    delta * (NormalMoveSpeed + AdditionalMoveSpeed)) with { Y = Velocity.Y };
                Vector3 newLook = new Vector3(NextPath.X, Position.Y, NextPath.Z);
                if (newLook != Position)
                {
                    TryingLookTo = newLook;
                }
            }
            else
            {
                if (NumberOfPatrolPointCurrent == (PatrolPath.Curve.PointCount - 1)) NumberOfPatrolPointCurrent = 0;
                else NumberOfPatrolPointCurrent++;
                NavAgent.TargetPosition = PatrolPath.Curve.GetPointPosition(NumberOfPatrolPointCurrent) + PatrolPath.GlobalPosition;
            }
        }
        if ((MasterWeapon.CurrentWeapon != null) &&
            (Position.DistanceTo(Player.Position) < MasterWeapon.CurrentWeapon.MaxDistanceForNPC) &&
            playerVisible)
        {
            MasterWeapon.CurrentlyAttack = true;
            Velocity = Velocity.Lerp(new Vector3(0, Velocity.Y, 0), delta * NormalStopSpeed);
            LookAt(Player.Position);
        }
        else if (!NavAgent.IsNavigationFinished() || playerVisible)
        {
            Mesh.Set("curAnim", 2);
            MasterWeapon.CurrentlyAttack = false;
            Velocity = Velocity.Lerp((NextPath - GlobalPosition).Normalized() * MaxMoveSpeed,
                delta * (NormalMoveSpeed + AdditionalMoveSpeed)) with { Y = Velocity.Y };
            Vector3 newLook = new Vector3(NextPath.X,Position.Y, NextPath.Z);
            if (newLook!=Position)
            {
                TryingLookTo = newLook;
            }
        }
        else
        {
            Mesh.Set("curAnim", 0);
            Velocity = Velocity.Lerp(new Vector3(0,Velocity.Y,0), delta * NormalStopSpeed);
        }
    }



    public override Dictionary GetWeaponRay(uint CollisionMask, Vector3 NewRayTarget)
    {
        Vector3 rayStart = WeaponRay.GlobalPosition;
        Vector3 directionToPlayer = Position.DirectionTo(Player.Position);
        Vector3 rayEnd = rayStart + NewRayTarget
            .Rotated(new Vector3(1, 0, 0),
            Mathf.Atan2(directionToPlayer.Y,
            Mathf.Sqrt(directionToPlayer.X * directionToPlayer.X + directionToPlayer.Z * directionToPlayer.Z)))
            .Rotated(new Vector3(0, 1, 0),
            Mathf.Atan2(directionToPlayer.X,
            directionToPlayer.Z) + Mathf.Pi);
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

    protected Vector3 FindClosestPatrolPoint()
    {
        NavAgent.TargetPosition = PatrolPath.Curve.GetPointPosition(0);
        float distance = NavAgent.DistanceToTarget();
        Vector3 point = PatrolPath.Curve.GetPointPosition(0);
        for (int i = 1; i < PatrolPath.Curve.PointCount-1; i++)
        {
            NavAgent.TargetPosition = PatrolPath.Curve.GetPointPosition(i);
            float newDistance = NavAgent.DistanceToTarget();
            if (distance>newDistance)
            {
                distance = newDistance;
            }
                
        }
        return point;
    }

    public override void _Process(double delta)
    {
        if (PatrolPath == null) IsInPatrol = false;
        SlowLookAt(TryingLookTo, (float)delta);
        Movement((float)delta);
        if (!IsOnFloor())
        {
            ApplyGravityForce((float)delta);
        }
        MoveAndSlide();
    }
}
