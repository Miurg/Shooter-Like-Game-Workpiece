using Godot;
using System;

public partial class NPCWeapon : LifeWeapons
{
    public override async void _Ready()
    {
        SetProcess(false);
        SetPhysicsProcess(false); 
        await ToSignal(GetTree(), "physics_frame");
        await ToSignal(GetTree(), "physics_frame");
        SetProcess(true);
        SetPhysicsProcess(true);
        Weapon newWeapon = ResourceLoader.Load<PackedScene>("res://Nodes/Objects/Weapons/AK.tscn").Instantiate<Weapon>();
        SetCurrentWeapon(newWeapon);
    }

    public override void _PhysicsProcess(double delta)
    {
        Attack((float)delta);
        if (CurrentWeapon != null)
        {
            if (CurrentWeapon.CurrentRounds == 0)
            {
                Reload();
            }
        }
    }

    override public void SetCurrentWeapon(Weapon weapon)
    {
        if (weapon == null)
        {
            _CurrentWeapon = null;
            return;
        }
        Weapon newWeapon = ResourceLoader.Load<PackedScene>(weapon.SceneFilePath).Instantiate<Weapon>();
        AddChild(newWeapon);
        newWeapon.Freeze = true;
        newWeapon.Rotation = new Vector3(0, 0, 0);
        newWeapon.Position = new Vector3(0.5f, 1.5f, -0.4f);
        _CurrentWeapon = newWeapon;
        _CurrentWeapon.CurrentMasterWeapon = newWeapon.GetParent<LifeWeapons>();
        _CurrentWeapon.CurrentOwner = newWeapon.GetParent().GetParent<Life>();
        weapon.Die();
    }
}
