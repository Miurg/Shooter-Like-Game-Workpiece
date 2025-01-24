using Godot;
using player;
using System;

public partial class NPCRonin : NPCAngry
{
	public override async void _Ready()
	{
        SetProcess(false);
        SetPhysicsProcess(false);
        await ToSignal(GetTree(), "physics_frame");
        await ToSignal(GetTree(), "physics_frame");
        SetProcess(true);
        SetPhysicsProcess(true);

        CollisionShape = GetNode<CollisionShape3D>("CollisionShape3D");
        TimerIddleForPatrolingPoints = GetNode<Timer>("TimerIddleForPatrolingPoints");
        TimerSeekForDamageApplyer = GetNode<Timer>("TimerSeekForDamageApplyer");
        Player = GetNode<PlayerMain>("../Player");
        MasterWeapon = GetNode<WeaponHolderBase>("MasterWeapon");
        Mesh = GetNode<Node3D>("Ronin");
        NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        VisionRay = GetNode<RayCast3D>("VisionRay");
        MainNode = GetNode<MainNode>("/root/MainNode/");
        WeaponRay = GetNode<RayCast3D>("WeaponRay");

        CurrentTimeSee = 0;
        SpaceState = GetWorld3D().DirectSpaceState;
        TryingLookTo = Transform * new Vector3(0, 0, -1);
    }

}
