using Godot;
using player;
using System;

public partial class NPCRonin : NPCAngry
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Player = GetNode<PlayerMain>("Player");
        MasterWeapon = GetNode<LifeWeapons>("MasterWeapon");
        Mesh = GetNode<Node3D>("copskeleton");
        NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        VisionRay = GetNode<RayCast3D>("VisionRay");
        AdditionalMoveSpeed = (int)GetMeta("AdditionalMoveSpeed");
        MaxMoveSpeed = (int)GetMeta("MaxMoveSpeed");
        FieldOfView = (int)GetMeta("FieldOfView");
        HealthPoint = (int)GetMeta("HealthPoint");
        TimeUntilUnsee = (int)GetMeta("TimeUntilUnsee");
        MaxDistanceOfView = (int)GetMeta("MaxDistanceOfView");
        CurrentTimeSee = 0;
        SpaceState = GetWorld3D().DirectSpaceState;
        TryingLookTo = Transform * new Vector3(0, 0, -1);
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
