using Godot;
using player;
using System;

public partial class NPCRonin : NPCAngry
{
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
        SetProcess(false);
        SetPhysicsProcess(false);
        await ToSignal(GetTree(), "physics_frame");
        await ToSignal(GetTree(), "physics_frame");
        SetProcess(true);
        SetPhysicsProcess(true);

        Player = GetNode<PlayerMain>("../Player");
        MasterWeapon = GetNode<LifeWeapons>("MasterWeapon");
        Mesh = GetNode<Node3D>("Ronin");
        NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        VisionRay = GetNode<RayCast3D>("VisionRay");
        MainNode = GetNode<MainNode>("/root/MainNode/");

        AdditionalMoveSpeed = (int)GetMeta("additionalMoveSpeed");
        MaxMoveSpeed = (int)GetMeta("maxMoveSpeed");
        FieldOfView = (int)GetMeta("fieldOfView");
        HealthPoint = (int)GetMeta("healthPoint");
        TimeUntilUnsee = (int)GetMeta("timeUntilUnsee");
        MaxDistanceOfView = (int)GetMeta("maxDistanceOfView");

        CurrentTimeSee = 0;
        SpaceState = GetWorld3D().DirectSpaceState;
        TryingLookTo = Transform * new Vector3(0, 0, -1);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsPlayerVisible())
        {
            playerVisible = true;
            CurrentTimeSee= TimeUntilUnsee;
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
        if (IsOnFloor())
        {
            ApplyGravityForce((float)delta);
        }
        MoveAndSlide();
	}
}
