using Godot;
using player;
using System;

public partial class AK : Weapon
{
    public override void _Ready()
	{
		SpreadMax = 5;
		SpreadMin = 0;
		SpreadSpeedUp = 0.5f;
		SpreadSpeedDown = 3;
		RateOfFire = 0.1f;
		Damage = 1;
		NameOfWeapon = "AK";
		MaxDistanceForNPC = 20;
        Remains = ResourceLoader.Load<PackedScene>("res://Nodes/Objects/Hole.tscn");
		AtackParticle = ResourceLoader.Load<PackedScene>("res://Nodes/Particles/ShootParticles.tscn");
        SceneOfWeapon = ResourceLoader.Load<PackedScene>("res://Nodes/Objects/Weapons/AK.tscn");
        SoundForNPC = ResourceLoader.Load<PackedScene>("res://Nodes/Sounds/AKShootSoundNPC.tscn");
        SoundForPlayer = ResourceLoader.Load<PackedScene>("res://Nodes/Sounds/AKShootSound.tscn");
        ParticlesNode = GetNode<Node3D>("Patricles");
    }
}
