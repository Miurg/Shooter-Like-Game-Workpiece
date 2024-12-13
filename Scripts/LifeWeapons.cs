using Godot;
using System;

public abstract partial class LifeWeapons : Node3D
{
    protected Weapon _CurrentWeapon;
	private bool _CurrentlyAttack;
	private float _TimeFromLastAtack = 0f;
	private float _CurrentSpread = 0f;
	private int _RoundsPocket = 90;

    public virtual bool CurrentlyAttack { get => _CurrentlyAttack; set => _CurrentlyAttack = value; }
    public virtual float TimeFromLastAtack { get => _TimeFromLastAtack; set => _TimeFromLastAtack = value; }
    public virtual float CurrentSpread { get => _CurrentSpread; set => _CurrentSpread = value; }
    public virtual int RoundsPocket { get => _RoundsPocket; set => _RoundsPocket = value; }
    public virtual Weapon CurrentWeapon { get => _CurrentWeapon;  }

    virtual public void SetCurrentWeapon(Weapon weapon)
	{
        if (weapon == null)
        {
            _CurrentWeapon = null;
            return;
        }
		Weapon newWeapon = ResourceLoader.Load<PackedScene>(weapon.SceneFilePath).Instantiate<Weapon>();
        AddChild(newWeapon);
        _CurrentWeapon = newWeapon;
        _CurrentWeapon.CurrentOwner = newWeapon.GetParent().GetParent<Life>();
        _CurrentWeapon.CurrentMasterWeapon = newWeapon.GetParent<LifeWeapons>();
        weapon.Die();
    }

    virtual public void DeleteWeaponInHeands()
    {
        CurrentWeapon.Die();
    }


    public void Attack(float delta)
    {
        if (CurrentWeapon != null)
        {
            if (CurrentlyAttack && TimeFromLastAtack>CurrentWeapon.RateOfFire && CurrentWeapon.CurrentRounds > 0)
            {
                CurrentWeapon.Attack(CurrentSpread);
                TimeFromLastAtack = 0;
                if (CurrentSpread <= CurrentWeapon.SpreadMax)
                {
                    CurrentSpread += CurrentWeapon.SpreadSpeedUp;
                }
            }
            else if (TimeFromLastAtack<=CurrentWeapon.RateOfFire) TimeFromLastAtack += delta;

            if (CurrentSpread>CurrentWeapon.RateOfFire)
            {
                CurrentSpread -= CurrentWeapon.SpreadSpeedDown*delta;
            }    
        }
        else if (CurrentSpread!=0)
        {
            CurrentSpread = 0;
        }
    }

    public void Reload()
    {
        if (CurrentWeapon != null)
        {
            if (RoundsPocket>CurrentWeapon.RoundsTotal || RoundsPocket-(CurrentWeapon.RoundsTotal - CurrentWeapon.CurrentRounds) >=0)
            {
                RoundsPocket -= CurrentWeapon.RoundsTotal - CurrentWeapon.CurrentRounds;
                CurrentWeapon.CurrentRounds = CurrentWeapon.RoundsTotal;
            }
            else
            {
                CurrentWeapon.CurrentRounds = CurrentWeapon.CurrentRounds + RoundsPocket;
                RoundsPocket = 0;
            }
        }
    }

}
