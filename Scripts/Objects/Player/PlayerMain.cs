using Godot;
using Godot.Collections;
using System;
namespace player
{
    public partial class PlayerMain : CharacterBase
    {
        PlayerCamera PlayerCamera;
        PlayerWeapons PlayerWeapons;
        public HUD HUD;
        public Vector3 PlayerVelocity = Vector3.Zero;
        [Export] private int _JumpForce = 10;
        [Export] private int _NumberOfJumps = 1;
        public bool ShiftHold = false;

        [Signal]
        public delegate void HPChangeEventHandler(int HP);

        public override void _Ready()
        {
            MainNode = GetNode<MainNode>("/root/MainNode/");
            PlayerCamera = GetNode<PlayerCamera>("PlayerCameraMain");
            PlayerWeapons = GetNode<PlayerWeapons>("Weapons");
            HUD = GetNode<HUD>("/root/MainNode/HUD");
        }

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
                    if (PlayerCamera.InstanceWeapon != null) 
                    {
                        PlayerWeapons.SetCurrentWeapon((WeaponBase)PlayerCamera.InstanceWeapon);
                    }
                    if (PlayerCamera.GeneralRay.IsColliding())
                    {
                        ((Door)PlayerCamera.GeneralRay.GetCollider()).Activate();
                    }
                }
                if (key.Keycode == Key.G && @event.IsPressed())
                {
                    PlayerWeapons.DeleteWeaponInHeands();
                }
                if (key.Keycode == Key.R && @event.IsPressed())
                {
                    PlayerWeapons.Reload();
                }
                if (key.Keycode == Key.Shift && @event.IsPressed())
                {
                    ShiftHold = true;
                }
                else if (key.Keycode == Key.Shift)
                {
                    ShiftHold = false;
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
            
            PlayerVelocity = direction.Normalized().Rotated(new Vector3(0, 1, 0), Rotation.Y);
            
            Vector2 maxVelocity = new Vector2(MaxMoveSpeed, MaxMoveSpeed).Normalized();
            if (ShiftHold)
            {
                PlayerVelocity *= MaxMoveSpeedRun;
                maxVelocity *= MaxMoveSpeedRun;
            }
            else
            {
                PlayerVelocity *= MaxMoveSpeed;
                maxVelocity *= MaxMoveSpeed;
            }
            PlayerVelocity = PlayerVelocity with { Y = Velocity.Y };

            
            if (Math.Abs(Velocity.X) < maxVelocity.X
                && Math.Abs(Velocity.Z) < maxVelocity.Y
                && PlayerVelocity != new Vector3(0, Velocity.Y, 0))
            {
                Velocity = Velocity.Lerp(PlayerVelocity, (float)delta * (NormalMoveSpeed + AdditionalMoveSpeed));
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

        public override void ChangeHealth(int value, CharacterBase fromWho)
        {
            HealthPoint -= value;
            EmitSignal("HPChange", HealthPoint);
        }

        public override void Die()
        {
            throw new NotImplementedException();
        }


        public override void MainPlaceWeapon(RigidBody3D Weapon)
        {
            Vector3 weaponPosition = new Vector3(Position.X, Position.Y + PlayerCamera.Position.Y, Position.Z) + 
                new Vector3(0, 0, -1).Rotated(new Vector3(1, 0, 0), PlayerCamera.Rotation.X).Rotated(new Vector3(0, 1, 0), Rotation.Y);
            Vector3 weaponImpulse = new Vector3(0, 0, -10).Rotated(new Vector3(1, 0, 0), PlayerCamera.Rotation.X).Rotated(new Vector3(0, 1, 0), Rotation.Y);
            MainNode.PlaceWeapon(this, Weapon, weaponImpulse, weaponPosition);
        }


    }

}
