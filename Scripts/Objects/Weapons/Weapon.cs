using Godot;
using Godot.Collections;
using player;
using System;

public abstract partial class Weapon : RigidBody3D, IWeapon
{
    public PackedScene SceneOfWeapon;
    protected PackedScene Remains;
    protected PackedScene SoundForNPC;
    protected PackedScene SoundForPlayer;
    protected PackedScene AtackParticle;
    protected Node3D ParticlesNode;
    public Life CurrentOwner;
    public LifeWeapons CurrentMasterWeapon;
    public float RateOfFire;
    public int MaxDistanceForNPC;
    public int Damage;
    public string NameOfWeapon;
    public float SpreadMax;
    public float SpreadMin;
    public float SpreadSpeedUp;
    public float SpreadSpeedDown;
    private int _RoundsTotal = 30;
    private int _CurrentRounds = 30;

    public int RoundsTotal { get => _RoundsTotal; set => _RoundsTotal = value; }
    public int CurrentRounds { 
        get => _CurrentRounds; 
        set 
        {
            if (CurrentOwner.Name == "Player")
            {
                ((PlayerMain)CurrentOwner).HUDRoundsCurrent(value);
            }
            _CurrentRounds = value; 
        } 
    }

    public void Attack(float spread)
    {
        if (CurrentOwner != null)
        {
            CurrentRounds -= 1;
            ParticlesNode.AddChild(AtackParticle.Instantiate());
            Vector3 spreadVector = new(0, 0, -1);
            if (spread>0)
            {
                Vector2 dotsForSpread = new Vector2((float)new Random().Next((int)(-spread * 10), (int)(spread * 10)) / 10,
                    (float)new Random().Next((int)(-spread * 10), (int)(spread * 10)) / 10);
                while (Math.Pow(dotsForSpread.X,2)+ Math.Pow(dotsForSpread.Y, 2)>spread)
                {
                    dotsForSpread = new Vector2((float)new Random().Next((int)(-spread * 10), (int)(spread * 10)) / 10,
                        (float)new Random().Next((int)(-spread * 10), (int)(spread * 10)) / 10);
                }
                spreadVector = new Vector3(dotsForSpread.X,dotsForSpread.Y,-10).Normalized();
            }
            Dictionary targetOfAtack;
            if (CurrentOwner.Name == "Player")
            {
                AddChild(SoundForPlayer.Instantiate());
                targetOfAtack = CurrentOwner.GetWeaponRay(0b00000000_00000000_00000000_00000011, spreadVector * 100);
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
                    ((Life)targetOfAtack["collider"]).ChangeHealth(Damage, CurrentOwner);
                }
            }
        }
    }

    public void Die()
    {
        if (CurrentMasterWeapon is LifeWeapons)
        {
            CurrentMasterWeapon.SetCurrentWeapon(null);
        }
        QueueFree();
    }
}
