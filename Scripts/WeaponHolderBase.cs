using Godot;
using System;

public abstract partial class WeaponHolderBase : Node3D
{
    protected WeaponBase _CurrentWeapon;
	private bool _CurrentlyAttack;
	private float _TimeFromLastAtack = 0f;
	private float _CurrentSpread = 0f;
    [Export] private int _CurrentPocketRounds = 90;
    [Export] protected int[] PocketRounds = new int[3];

    [Signal]
    public delegate void OnAttackEventHandler();

    public virtual bool CurrentlyAttack { get => _CurrentlyAttack; set => _CurrentlyAttack = value; }
    public virtual float TimeFromLastAtack { get => _TimeFromLastAtack; set => _TimeFromLastAtack = value; }
    public virtual float CurrentSpread { get => _CurrentSpread; set => _CurrentSpread = value; }
    public virtual int CurrentPocketRounds { get => _CurrentPocketRounds; set => _CurrentPocketRounds = value; }
    public virtual WeaponBase CurrentWeapon { get => _CurrentWeapon;  }


    virtual public void SetCurrentWeapon(WeaponBase weapon)
	{
        if (weapon == null)
        {
            _CurrentWeapon = null;
            return;
        }
		WeaponBase newWeapon = ResourceLoader.Load<PackedScene>(weapon.SceneFilePath).Instantiate<WeaponBase>();
        AddChild(newWeapon);
        _CurrentWeapon = newWeapon;
        _CurrentWeapon.CurrentOwner = newWeapon.GetParent().GetParent<CharacterBase>();
        _CurrentWeapon.CurrentMasterWeapon = newWeapon.GetParent<WeaponHolderBase>();
        weapon.Die();
    }

    virtual public void DeleteWeaponInHeands()
    {
        if (_CurrentWeapon != null)
        {
            CurrentWeapon.Die();
        }
    }


    public void Attack(float delta)
    {
        if (CurrentlyAttack && TimeFromLastAtack>CurrentWeapon.RateOfFire && CurrentWeapon.CurrentRounds > 0)
        {
            EmitSignal("OnAttack");
            CurrentWeapon.Attack(CurrentSpread);
            TimeFromLastAtack = 0;
            SpreadUp();
        }
        else if (TimeFromLastAtack<=CurrentWeapon.RateOfFire) TimeFromLastAtack += delta;
    }

    public void SpreadUp()
    {
        if (CurrentSpread <= CurrentWeapon.SpreadMax)
        {
            CurrentSpread += CurrentWeapon.SpreadSpeedUp;
        }
    }

    public void SpreadDown(float delta)
    {
        if (CurrentWeapon != null)
        {
            if (CurrentSpread > CurrentWeapon.SpreadMin) 
                CurrentSpread -= CurrentWeapon.SpreadSpeedDown * delta;
        }
        else if (CurrentSpread != 0) CurrentSpread = 0;
    }

    public void Reload()
    {
        if (CurrentWeapon != null)
        {
            if (CurrentPocketRounds>CurrentWeapon.RoundsTotal || CurrentPocketRounds-(CurrentWeapon.RoundsTotal - CurrentWeapon.CurrentRounds) >=0)
            {
                CurrentPocketRounds -= CurrentWeapon.RoundsTotal - CurrentWeapon.CurrentRounds;
                CurrentWeapon.CurrentRounds = CurrentWeapon.RoundsTotal;
            }
            else
            {
                CurrentWeapon.CurrentRounds = CurrentWeapon.CurrentRounds + CurrentPocketRounds;
                CurrentPocketRounds = 0;
            }
        }
    }

}
