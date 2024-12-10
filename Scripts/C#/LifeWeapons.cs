using Godot;
using System;

public abstract partial class LifeWeapons : Node3D
{
    private Weapon _CurrentWeapon;
	private bool _CurrentlyAttack = false;
	private float _TimeFromLastAtack = 0f;
	private float _CurrentSpread = 0f;
	private int _RoundsInPocket = 0;

    public bool CurrentlyAttack { get => _CurrentlyAttack; set => _CurrentlyAttack = value; }
    public float TimeFromLastAtack { get => _TimeFromLastAtack; set => _TimeFromLastAtack = value; }
    public float CurrentSpread { get => _CurrentSpread; set => _CurrentSpread = value; }
    public int RoundsInPocket { get => _RoundsInPocket; set => _RoundsInPocket = value; }
    public Weapon CurrentWeapon { get => _CurrentWeapon;  }

    public void SetCurrentWeapon(PackedScene weapon)
	{
        if (weapon == null)
        {
            _CurrentWeapon = null;
            return;
        }
		Weapon newWeapon = weapon.Instantiate<Weapon>();
		AddChild(newWeapon);
		_CurrentWeapon = newWeapon;
        _CurrentWeapon.CurrentOwner = GetParent().GetParent<Life>();
        _CurrentWeapon.CurrentMasterWeapon = GetParent<LifeWeapons>();
    }

    public void DeleteWeaponInHeands()
    {
        CurrentWeapon.QueueFree();
        SetCurrentWeapon(null);
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
            else if (TimeFromLastAtack<CurrentWeapon.RateOfFire) TimeFromLastAtack += delta;

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
            if (RoundsInPocket>CurrentWeapon.RoundsTotal || RoundsInPocket-(CurrentWeapon.RoundsTotal - CurrentWeapon.CurrentRounds) >=0)
            {
                RoundsInPocket -= CurrentWeapon.RoundsTotal - CurrentWeapon.CurrentRounds;
                CurrentWeapon.CurrentRounds = CurrentWeapon.RoundsTotal;
            }
            else
            {
                CurrentWeapon.CurrentRounds = CurrentWeapon.CurrentRounds + RoundsInPocket;
                RoundsInPocket = 0;
            }
        }
    }

}
