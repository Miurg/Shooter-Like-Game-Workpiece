using Godot;
using System;

namespace player
{
    public partial class PlayerWeapons : LifeWeapons
    {
        PlayerMain PlayerMain;
        [Signal]
        public delegate void CurrentSpreadChangeEventHandler (float CurrentSpread);

        [Signal]
        public delegate void PocketRoundsChangeEventHandler (int RoundsPocket);

        [Signal]
        public delegate void CurrentRoundsChangeEventHandler (int CurrentRounds);

        public override float CurrentSpread { 
            get => base.CurrentSpread;
            set
            {
                EmitSignal("CurrentSpreadChange", value);
                base.CurrentSpread = value; 
            }
        }
        public override int CurrentPocketRounds
        {
            get => base.CurrentPocketRounds; 
            set
            {
                EmitSignal("PocketRoundsChange", value);
                base.CurrentPocketRounds = value;
            }
        }
        public override async void _Ready()
        {
            PlayerMain = GetNode<PlayerMain>("..");
            SetProcess(false);
            SetPhysicsProcess(false);
            await ToSignal(GetTree(), "physics_frame");
            await ToSignal(GetTree(), "physics_frame");
            SetProcess(true);
            SetPhysicsProcess(true);
            EmitSignal("CurrentSpreadChange", CurrentSpread);
            EmitSignal("PocketRoundsChange", CurrentPocketRounds);
        }

        public override void SetCurrentWeapon(Weapon weapon)
        {
            if (weapon == null)
            {
                _CurrentWeapon = weapon;
                return;
            }
            DeleteWeaponInHeands();
            Weapon newWeapon = ResourceLoader.Load<PackedScene>(weapon.SceneFilePath).Instantiate<Weapon>();
            AddChild(newWeapon);
            newWeapon.CurrentRounds = weapon.CurrentRounds;
            newWeapon.Rotation = new Vector3(0, 0, 0);
            newWeapon.Position = new Vector3(0.331f, 0, -0.419f);
            newWeapon.Freeze = true;
            CurrentPocketRounds = PocketRounds[(int)newWeapon.NameOfWeapon];
            _CurrentWeapon = newWeapon;
            _CurrentWeapon.CurrentOwner = newWeapon.GetParent().GetParent<Life>();
            _CurrentWeapon.CurrentMasterWeapon = newWeapon.GetParent<LifeWeapons>();
            CurrentSpread = _CurrentWeapon.SpreadMin;
            weapon.Die();
        }

        public override void DeleteWeaponInHeands()
        {
            if (CurrentWeapon != null)
            {
                PocketRounds[(int)_CurrentWeapon.NameOfWeapon] = CurrentPocketRounds;
                Weapon newWeapon = ResourceLoader.Load<PackedScene>(CurrentWeapon.SceneFilePath).Instantiate<Weapon>();
                newWeapon.CurrentRounds = CurrentWeapon.CurrentRounds;
                PlayerMain.MainPlaceWeapon(newWeapon);
                CurrentWeapon.Die();
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (_CurrentWeapon != null) Attack((float)delta);
            SpreadDown((float)delta);
            if (CurrentWeapon != null) 
            {
                EmitSignal("CurrentRoundsChange", CurrentWeapon.CurrentRounds);
            }
        }
    }
}

