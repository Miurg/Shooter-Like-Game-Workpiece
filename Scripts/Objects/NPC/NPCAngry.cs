using Godot;
using Godot.Collections;
using player;
using System;
using System.Security;
using System.Text.RegularExpressions;

public abstract partial class NPCAngry : NPCBase
{
    [Export] protected Path3D PatrolPath;
    protected int NumberOfPatrolPointCurrent = 0;

    protected Timer TimerIddleForPatrolingPoints;
    protected Timer TimerSeekForDamageApplyer;
    protected RayCast3D WeaponRay;
    protected WeaponHolderBase MasterWeapon;
    protected NavigationAgent3D NavAgent;
    protected Vector3 TargetToMove;
    [Export] protected float TimeUntilUnsee;
    protected float CurrentTimeSee;
    protected Vector3 NextPath;
    protected Vector3 PreviousPath;
    protected Vector3 TryingLookTo;
    public bool playerVisible = false;
    public bool playerVisibleFromTime = false;

    protected override void BehaviorController(float delta)
    {
        if ((MasterWeapon.CurrentWeapon != null) &&
            (Position.DistanceTo(Player.Position) < MasterWeapon.CurrentWeapon.MaxDistanceForNPC) &&
            playerVisible)
        {
            BehaviorState = eBehaviorState.Attack;
        }
        else if (!NavAgent.IsNavigationFinished() && playerVisibleFromTime || playerVisible)
        {
            BehaviorState = eBehaviorState.Chase;
        }
        else if (BehaviorState == eBehaviorState.SeekForPlayer)
        {
            return;
        }
        else if (PatrolPath != null)
        {
            if (!(BehaviorState == eBehaviorState.Patrol))
            { 
                BehaviorState = eBehaviorState.Patrol;
                FindClosestPatrolPoint();
            }
            if (NavAgent.IsNavigationFinished())
            {
                if (NumberOfPatrolPointCurrent == (PatrolPath.Curve.PointCount - 1)) NumberOfPatrolPointCurrent = 0;
                else NumberOfPatrolPointCurrent++;
                NavAgent.TargetPosition = PatrolPath.Curve.GetPointPosition(NumberOfPatrolPointCurrent) + PatrolPath.GlobalPosition;
                TimerIddleForPatrolingPoints.Start();
                BehaviorState = eBehaviorState.SeekForPlayer;
            }
        }
        else
        {
            BehaviorState = eBehaviorState.Idle;
        }
    }

    protected override void BehaviorApplyer(float delta)
    {
        Vector3 newLook;
        switch (BehaviorState)
        {
            case eBehaviorState.Idle:
                Velocity = Velocity.Lerp(new Vector3(0, Velocity.Y, 0), delta * NormalStopSpeed);
                Mesh.Set("curAnim", 0);
                break;

            case eBehaviorState.Chase:
                Mesh.Set("curAnim", 2);
                MasterWeapon.CurrentlyAttack = false;
                Velocity = Velocity.Lerp((NextPath - GlobalPosition).Normalized() * MaxMoveSpeedRun,
                    delta * (NormalMoveSpeed + AdditionalMoveSpeed)) with { Y = Velocity.Y };
                newLook = new Vector3(NextPath.X, Position.Y, NextPath.Z);
                if (newLook != Position)
                {
                    TryingLookTo = newLook;
                }
                break;

            case eBehaviorState.Attack:
                MasterWeapon.CurrentlyAttack = true;
                Velocity = Velocity.Lerp(new Vector3(0, Velocity.Y, 0), delta * NormalStopSpeed);
                LookAt(Player.Position);
                if (MasterWeapon.CurrentWeapon.CurrentRounds == 0 && MasterWeapon.CurrentPocketRounds == 0)
                    MasterWeapon.DeleteWeaponInHeands();
                break;

            case eBehaviorState.Patrol:
                Mesh.Set("curAnim", 1);
                MasterWeapon.CurrentlyAttack = false;
                Velocity = Velocity.Lerp((NextPath - GlobalPosition).Normalized() * MaxMoveSpeed,
                     delta * (NormalMoveSpeed + AdditionalMoveSpeed)) with { Y = Velocity.Y };
                newLook = new Vector3(NextPath.X, Position.Y, NextPath.Z);
                if (newLook != Position)
                {
                     TryingLookTo = newLook;
                }
                break;

            case eBehaviorState.SeekForPlayer:
                Mesh.Set("curAnim", 0);
                Velocity = Velocity.Lerp(new Vector3(0, Velocity.Y, 0), delta * NormalStopSpeed);
                break;
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
    public override void MainPlaceWeapon(RigidBody3D Weapon)
    {
        Vector3 weaponPosition = 
            new Vector3(MasterWeapon.CurrentWeapon.Position.X, MasterWeapon.CurrentWeapon.Position.Y, MasterWeapon.CurrentWeapon.Position.Z)
            .Rotated(new Vector3(1, 0, 0), Rotation.X).Rotated(new Vector3(0, 1, 0), Rotation.Y) +
            GlobalPosition;
        Vector3 weaponImpulse = new Vector3(0, 0, 0);
        MainNode.PlaceWeapon(this, Weapon, weaponImpulse, weaponPosition);
    }

    public override void ChangeHealth(int value, CharacterBase fromWho)
    {
        HealthPoint -= value;
        SeekForDamageDealer(fromWho);
        if (HealthPoint < 0)
        {
            Die();
        }
    }

    protected void SeekForDamageDealer(CharacterBase FromWho)
    {
        if (FromWho.Name == "Player")
        {
            TimerSeekForDamageApplyer.Start();
            BehaviorState = eBehaviorState.SeekForPlayer;
            TryingLookTo = new Vector3(FromWho.Position.X, Position.Y, FromWho.Position.Z);
        }
    }

    public override void Die()
    {
        if (MasterWeapon.CurrentWeapon!=null)
        {
            WeaponBase newWeapon = ResourceLoader.Load<PackedScene>(MasterWeapon.CurrentWeapon.SceneFilePath).Instantiate<WeaponBase>();
            newWeapon.CurrentRounds = MasterWeapon.CurrentWeapon.CurrentRounds;
            MainPlaceWeapon(newWeapon);
            MasterWeapon.DeleteWeaponInHeands();
        }
        Mesh.Set("curAnim", 4);
        Alive = false;
        CollisionShape.Disabled = true;
    }

    protected void FindClosestPatrolPoint()
    {
        NavAgent.TargetPosition = PatrolPath.Curve.GetPointPosition(0) + PatrolPath.GlobalPosition;
        float distance = NavAgent.DistanceToTarget();
        Vector3 point = PatrolPath.Curve.GetPointPosition(0);
        int currentPointNumber = 0;
        for (int i = 1; i < PatrolPath.Curve.PointCount - 1; i++)
        {
            NavAgent.TargetPosition = PatrolPath.Curve.GetPointPosition(i) + PatrolPath.GlobalPosition;
            float newDistance = NavAgent.DistanceToTarget();
            if (distance > newDistance)
            {
                distance = newDistance;
                currentPointNumber = i;
                point = PatrolPath.Curve.GetPointPosition(i);
            }
        }
        NavAgent.TargetPosition = point + PatrolPath.GlobalPosition;
        NumberOfPatrolPointCurrent = currentPointNumber;
    }

    public void OnTimerSeekForDamageApplyerTimeout()
    {
        BehaviorState = eBehaviorState.Idle;
    }

    public void OnTimerIddleForPatrolingPointsTimeout()
    {
        if (TimerSeekForDamageApplyer.IsStopped()) BehaviorState = eBehaviorState.Patrol;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Alive)
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
    }

    public override void _Process(double delta)
    {
        if (!IsOnFloor())
        {
            ApplyGravityForce((float)delta);
        }
        if (Alive)
        {
            SlowLookAt(TryingLookTo, (float)delta);
            BehaviorController((float)delta);
            BehaviorApplyer((float)delta);
            MoveAndSlide();
        }
    }
}
