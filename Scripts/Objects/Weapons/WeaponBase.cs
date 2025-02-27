using Godot;
using Godot.Collections;
using player;
using System;

public abstract partial class WeaponBase : RigidBody3D, IWeapon
{
    public PackedScene SceneOfWeapon;
    protected PackedScene Remains;
    protected PackedScene SoundForNPC;
    protected PackedScene SoundForPlayer;
    protected PackedScene AtackParticle = null;
    protected Node3D ParticlesNode;
    public CharacterBase CurrentOwner;
    public WeaponHolderBase CurrentMasterWeapon;
    public eNameOfWeapon NameOfWeapon;
    [Export] public float RecoilStrength = 1;
    [Export] public float RateOfFire = 0.1f;
    [Export] public int MaxDistanceForNPC = 20;
    [Export] public int MaxDistanceForPlayer = 100;
    [Export] public int Damage = 1;
    [Export] public float SpreadMax = 5;
    [Export] public float SpreadMin = 0;
    [Export] public float SpreadSpeedUp = 0.5f;
    [Export] public float SpreadSpeedDown = 3;
    [Export] protected int _RoundsTotal = 30;
    [Export] protected int _CurrentRounds = 30;

    public int RoundsTotal { get => _RoundsTotal; set => _RoundsTotal = value; }
    public int CurrentRounds { 
        get => _CurrentRounds; 
        set 
        {
            _CurrentRounds = value; 
        } 
    }

    public void Attack(float spread)
    {
        if (CurrentOwner != null)
        {
            CurrentRounds -= 1;
            if (AtackParticle != null) ParticlesNode.AddChild(AtackParticle.Instantiate());
            Vector3 spreadVector = new(0, 0, -1);
            if (spread>0)
            {
                Vector2 dotsForSpread = new Vector2((float)new Random().Next((int)(-spread * 100), (int)(spread * 100)) / 100,
                    (float)new Random().Next((int)(-spread * 100), (int)(spread * 100)) / 100);
                while (Math.Pow(dotsForSpread.X,2)+ Math.Pow(dotsForSpread.Y, 2)>spread)
                {
                    dotsForSpread = new Vector2((float)new Random().Next((int)(-spread * 100), (int)(spread * 100)) / 100,
                        (float)new Random().Next((int)(-spread * 100), (int)(spread * 100)) / 100);
                }
                spreadVector = new Vector3(dotsForSpread.X,dotsForSpread.Y,-10).Normalized();
            }
            Dictionary targetOfAtack;
            if (CurrentOwner.Name == "Player")
            {
                AddChild(SoundForPlayer.Instantiate());
                targetOfAtack = CurrentOwner.GetWeaponRay(0b00000000_00000000_00000000_00000011, spreadVector * MaxDistanceForPlayer);
            }
            else
            {
                AddChild(SoundForNPC.Instantiate());
                targetOfAtack = CurrentOwner.GetWeaponRay(0b00000000_00000000_00000000_00010011, spreadVector * MaxDistanceForNPC);
            }
            if (targetOfAtack!=null)
            {
                CollisionObject3D newObj = (CollisionObject3D)targetOfAtack["collider"];
                if (newObj.GetCollisionLayerValue(1))
                {
                    CurrentOwner.MainCreateRemainsFromWeapon(newObj, (Vector3)targetOfAtack["position"], (Vector3)targetOfAtack["normal"], Remains);
                }
                else
                {
                    ((CharacterBase)targetOfAtack["collider"]).ChangeHealth(Damage, CurrentOwner);
                }
            }
        }
    }

    public void Die()
    {
        if (CurrentMasterWeapon is WeaponHolderBase)
        {
            CurrentMasterWeapon.SetCurrentWeapon(null);
        }
        QueueFree();
    }
}
