using Godot;
using System;

public partial class Katana : Weapon
{
    public override void _Ready()
    {
        NameOfWeapon = eNameOfWeapon.DeutchGun;
        Remains = ResourceLoader.Load<PackedScene>("res://Nodes/Objects/Scratch.tscn");
        SceneOfWeapon = ResourceLoader.Load<PackedScene>("res://Nodes/Objects/Weapons/Katana.tscn");
        SoundForNPC = ResourceLoader.Load<PackedScene>("res://Nodes/Sounds/KatanaAttackSoundNPC.tscn");
        SoundForPlayer = ResourceLoader.Load<PackedScene>("res://Nodes/Sounds/KatanaAttackSound.tscn");
        ParticlesNode = GetNode<Node3D>("Particles");
    }
}
