using Godot;
using System;

public partial class AK : WeaponBase
{
    public override void _Ready()
	{
		NameOfWeapon = eNameOfWeapon.AK;
        Remains = ResourceLoader.Load<PackedScene>("res://Nodes/Objects/Hole.tscn");
		AtackParticle = ResourceLoader.Load<PackedScene>("res://Nodes/Particles/ShootParticles.tscn");
        SceneOfWeapon = ResourceLoader.Load<PackedScene>("res://Nodes/Objects/Weapons/AK.tscn");
        SoundForNPC = ResourceLoader.Load<PackedScene>("res://Nodes/Sounds/AKShootSoundNPC.tscn");
        SoundForPlayer = ResourceLoader.Load<PackedScene>("res://Nodes/Sounds/AKShootSound.tscn");
        ParticlesNode = GetNode<Node3D>("Particles");
    }
}
