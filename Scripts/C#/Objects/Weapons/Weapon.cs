using Godot;
using Godot.Collections;
using System;

public abstract partial class Weapon : Node
{
    public PackedScene SceneOfWeapon;
    public PackedScene Remains;
    public PackedScene SoundForNPC;
    public PackedScene SoundForPlayer;
    public PackedScene AtackParticle;
    public Node3D ParticlesNode;
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
    public int RoundsTotal;

    public void Atack(float spread)
    {
        if (CurrentOwner != null)
        {
            CurrentMasterWeapon.RoundsCurrent -= 1;
            ParticlesNode.AddChild(AtackParticle.Instantiate());
            Vector3 spreadVector = new Vector3(0, 0, -1);
            if (spread>0)
            {
                Vector2 dotsForSpread = new Vector2(new Random().Next((int)(-spread * 10), (int)(spread * 10)) / 10,
                    new Random().Next((int)(-spread * 10), (int)(spread * 10)) / 10);
                while (Math.Pow(dotsForSpread.X,2)+ Math.Pow(dotsForSpread.Y, 2)>spread)
                {
                    dotsForSpread = new Vector2(new Random().Next((int)(-spread * 10), (int)(spread * 10)) / 10,
                        new Random().Next((int)(-spread * 10), (int)(spread * 10)) / 10);
                }
                spreadVector = new Vector3(dotsForSpread.X,dotsForSpread.Y,-10).Normalized();
            }
            Dictionary targetOfAtack = null;
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
                    CurrentOwner.MainCreateRemains(newObj, (Vector3)targetOfAtack["position"], (Vector3)targetOfAtack["normal"], Remains);
                }
                else
                {
                    ((Life)targetOfAtack["collider"]).ChangeHealth(Damage, CurrentOwner);
                }
            }
        }
    }
}
