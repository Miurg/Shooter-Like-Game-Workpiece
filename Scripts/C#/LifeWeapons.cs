using Godot;
using System;

public abstract partial class LifeWeapons : Node3D
{
    Weapon CurrentWeapon;
	private bool _CurrentlyShoot = false;
	private float _TimeFromLastShoot = 0f;
	private float _SpreadCurrent = 0f;
	private int _RoundsInPocket = 90;
    private int _RoundsCurrent = 30;

    public bool CurrentlyShoot { get => _CurrentlyShoot; set => _CurrentlyShoot = value; }
    public float TimeFromLastShoot { get => _TimeFromLastShoot; set => _TimeFromLastShoot = value; }
    public float SpreadCurrent { get => _SpreadCurrent; set => _SpreadCurrent = value; }
    public int RoundsInPocket { get => _RoundsInPocket; set => _RoundsInPocket = value; }
    public int RoundsCurrent { get => _RoundsCurrent; set => _RoundsCurrent = value; }

	public void SetWeapon(PackedScene weapon)
	{
		Weapon newWeapon = weapon.Instantiate<Weapon>();
		AddChild(newWeapon);
		CurrentWeapon = newWeapon;
	}
    public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
