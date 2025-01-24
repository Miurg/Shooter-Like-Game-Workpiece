using Godot;
using player;
using System;

public partial class NPCWeapon : WeaponHolderBase
{
    NPCAngry NPCMain;
    public override async void _Ready()
    {
        NPCMain = GetNode<NPCAngry>("..");
        SetProcess(false);
        SetPhysicsProcess(false); 
        await ToSignal(GetTree(), "physics_frame");
        await ToSignal(GetTree(), "physics_frame");
        SetProcess(true);
        SetPhysicsProcess(true);
        WeaponBase newWeapon = ResourceLoader.Load<PackedScene>("res://Nodes/Objects/Weapons/AK.tscn").Instantiate<WeaponBase>();
        SetCurrentWeapon(newWeapon);
    }

    [Export] private float _TimeForForceReload;
    private float _CurrentTimeForForceReload = 0;
    public override void _PhysicsProcess(double delta)
    {
        if (_CurrentWeapon != null)
        {
            Attack((float)delta);
            if (_CurrentTimeForForceReload >= _TimeForForceReload)
            {
                Reload();
            }
            if (CurrentWeapon.CurrentRounds == 0)
            {
                Reload();
            }
        } 
        if (NPCMain.playerVisible == false && _CurrentTimeForForceReload<_TimeForForceReload)
        {
            _CurrentTimeForForceReload += (float)delta;
        }
        SpreadDown((float)delta);
    }

    override public void SetCurrentWeapon(WeaponBase weapon)
    {
        if (weapon == null)
        {
            _CurrentWeapon = null;
            return;
        }
        WeaponBase newWeapon = ResourceLoader.Load<PackedScene>(weapon.SceneFilePath).Instantiate<WeaponBase>();
        AddChild(newWeapon);
        newWeapon.Freeze = true;
        newWeapon.Rotation = new Vector3(0, 0, 0);
        newWeapon.Position = new Vector3(0.5f, 1.5f, -0.4f);
        _CurrentWeapon = newWeapon;
        _CurrentWeapon.CurrentMasterWeapon = newWeapon.GetParent<WeaponHolderBase>();
        _CurrentWeapon.CurrentOwner = newWeapon.GetParent().GetParent<CharacterBase>();
        weapon.Die();
    }
}
