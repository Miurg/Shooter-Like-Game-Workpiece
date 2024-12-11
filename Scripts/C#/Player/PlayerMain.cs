using Godot;
using Godot.Collections;
using System;
namespace player
{
    public partial class PlayerMain : Life
    {
        PlayerCamera PlayerCamera;
        PlayerWeapons PlayerWeapons;
        HUD HUD;
        private Vector3 _PlayerVelocity = Vector3.Zero;
        [Export] private int _JumpForce = 10;
        [Export] private int _NumberOfJumps = 1;


        public override void _Ready()
        {
            MainNode = GetNode<MainNode>("/root/MainNode/");
            PlayerCamera = GetNode<PlayerCamera>("PlayerCameraMain");
            PlayerWeapons = GetNode<PlayerWeapons>("Weapons");
            HUD = GetNode<HUD>("/root/MainNode/HUD");
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
            Movement(delta);
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey key)
            {
                if (key.Keycode == Key.E && @event.IsPressed())
                {
                    PlayerWeapons.SetCurrentWeapon(((Weapon)PlayerCamera.InstanceWeapon));
                }
                if (key.Keycode == Key.G && @event.IsPressed())
                {
                    PlayerWeapons.DeleteWeaponInHeands();
                }
                if (key.Keycode == Key.R && @event.IsPressed())
                {
                    PlayerWeapons.Reload();
                }
            }
            if (@event is InputEventMouseButton button)
            {
                if (button.ButtonIndex == MouseButton.Left && @event.IsPressed())
                {
                    PlayerWeapons.CurrentlyAttack = true;
                }
                if (button.ButtonIndex == MouseButton.Left && !@event.IsPressed())
                {
                    PlayerWeapons.CurrentlyAttack = false;
                }
            }
        }

        const float TimeInAirForJump = 0.2f;
        private bool _FirstJumpHappend = false;
        private float _InAirTime = 0f;
        private int _JumpButtonClicks = 0;

        private void Movement(double delta)
        {
            Vector3 direction = new();
            if (Input.IsActionPressed("left"))
            {
                direction.X = -1;
            }
            if (Input.IsActionPressed("right"))
            {
                direction.X = 1;
            }
            if (Input.IsActionPressed("backward"))
            {
                direction.Z = 1;
            }
            if (Input.IsActionPressed("forward"))
            {
                direction.Z = -1;
            }
            if (Input.IsActionJustPressed("jump"))
            {
                if (_InAirTime < TimeInAirForJump || _JumpButtonClicks < _NumberOfJumps)
                {
                    Velocity = Velocity with { Y = _JumpForce };
                    _InAirTime = TimeInAirForJump;
                }
                _JumpButtonClicks++;
                _FirstJumpHappend = true;
            }
      
            _PlayerVelocity = direction.Normalized().Rotated(new Vector3(0, 1, 0), Rotation.Y) * MaxMoveSpeed;
            _PlayerVelocity = _PlayerVelocity with { Y = Velocity.Y };
            Vector2 maxVelocity = new Vector2(MaxMoveSpeed, MaxMoveSpeed).Normalized() * MaxMoveSpeed;
            
            if (Math.Abs(Velocity.X) < maxVelocity.X
                && Math.Abs(Velocity.Z) < MaxMoveSpeed
                && _PlayerVelocity != new Vector3(0, Velocity.Y, 0))
            {
                Velocity = Velocity.Lerp(_PlayerVelocity, (float)delta * NormalMoveSpeed);
            }
            else
            {
                Velocity = Velocity.Lerp(new Vector3(0, Velocity.Y, 0), (float)delta * NormalStopSpeed);
            }

            if (!IsOnFloor())
            {
                _InAirTime += (float)delta;
                if (_FirstJumpHappend == false && _InAirTime > TimeInAirForJump && _JumpButtonClicks == 0) _JumpButtonClicks += 1;
                ApplyGravityForce((float)delta);
            }
            MoveAndSlide();
            if (IsOnFloor())
            {
                _FirstJumpHappend = false;
                _JumpButtonClicks = 0;
                _InAirTime = 0;
            }
        }

        public override Dictionary GetWeaponRay(uint CollisionMask, Vector3 NewRayTarget)
        {
            return PlayerCamera.RayFromCamera(CollisionMask, NewRayTarget);
        }

        public override void ChangeHealth(int value, Life fromWho)
        {
            GD.Print("Papali");
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }

        public void HUDNormalSelected()
        {
            HUD.NormalSelected();
        }

        public void HUDUpdateSelected(int sizeXCurrent, int sizeYCurrent, int positionXCurrent, int positionYCurrent)
        {
            HUD.UpdateSelected(sizeXCurrent, sizeYCurrent, positionXCurrent, positionYCurrent);
        }

        public void HUDUpdateSpread(float spread)
        {
            HUD.SpreadWeapon = spread;
        }

        public void HUDRoundsPocket(int RoundsPocket)
        {
            HUD.RoundsPocket = RoundsPocket;
        }
        public void HUDRoundsCurrent(int RoundsCurrent)
        {
            HUD.RoundsCurrent = RoundsCurrent;
        }

        public void MainPlaceWeapon(RigidBody3D Weapon)
        {
            Vector3 weaponPosition = new Vector3(Position.X, Position.Y + PlayerCamera.Position.Y, Position.Z) + 
                new Vector3(0, 0, -1).Rotated(new Vector3(1, 0, 0), PlayerCamera.Rotation.X).Rotated(new Vector3(0, 1, 0), Rotation.Y);
            Vector3 weaponImpulse = new Vector3(0, 0, -10).Rotated(new Vector3(1, 0, 0), PlayerCamera.Rotation.X).Rotated(new Vector3(0, 1, 0), Rotation.Y);
            MainNode.PlaceWeapon(this, Weapon, weaponImpulse, weaponPosition);
        }


    }

}
