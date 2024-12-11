using Godot;
using System;

namespace player
{
    public partial class PlayerWeapons : LifeWeapons
    {
        PlayerMain PlayerMain;

        public override float CurrentSpread { 
            get => base.CurrentSpread;
            set
            {
                PlayerMain.HUDUpdateSpread(value);
                base.CurrentSpread = value; 
            }
        }
        public override int RoundsPocket
        {
            get => base.RoundsPocket; 
            set
            {
                PlayerMain.HUDRoundsPocket(value);
                base.RoundsPocket = value;
            }
        }
        public override void _Ready()
        {
            PlayerMain = GetNode<PlayerMain>("..");
        }

        public override void SetCurrentWeapon(Weapon weapon)
        {
            if (weapon == null)
            {
                _CurrentWeapon = null;
                return;
            }
            if (CurrentWeapon != null) DeleteWeaponInHeands();
            Weapon newWeapon = ResourceLoader.Load<PackedScene>(weapon.SceneFilePath).Instantiate<Weapon>();
            AddChild(newWeapon);
            weapon.QueueFree();
            newWeapon.Rotation = new Vector3(0, 0, 0);
            newWeapon.Position = new Vector3(0.331f, 0, -0.419f);
            _CurrentWeapon = newWeapon;
            _CurrentWeapon.CurrentOwner = newWeapon.GetParent().GetParent<Life>();
            _CurrentWeapon.CurrentMasterWeapon = newWeapon.GetParent<LifeWeapons>();
            CurrentSpread = _CurrentWeapon.SpreadMin;
        }

        public override void DeleteWeaponInHeands()
        {
            if (CurrentWeapon != null)
            {
                Weapon newWeapon = ResourceLoader.Load<PackedScene>(CurrentWeapon.SceneFilePath).Instantiate<Weapon>();
                PlayerMain.MainPlaceWeapon(newWeapon);
            }
            CurrentWeapon.QueueFree();
            SetCurrentWeapon(null);
        }

        public override void _PhysicsProcess(double delta)
        {
            Attack((float)delta);
        }
    }
}

