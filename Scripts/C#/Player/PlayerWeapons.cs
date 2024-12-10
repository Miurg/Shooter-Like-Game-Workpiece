using Godot;
using System;

namespace player
{
    public partial class PlayerWeapons : LifeWeapons
    {
        CharacterBody3D PlayerMain;

        public new float CurrentSpread { get => base.CurrentSpread; set => base.CurrentSpread = value; }
        public new int RoundsInPocket { get => base.RoundsInPocket; set => base.RoundsInPocket = value; }
        public new bool CurrentlyAttack { get => base.CurrentlyAttack; set => base.CurrentlyAttack = value; }
        public override void _Ready()
        {
            PlayerMain = GetNode<CharacterBody3D>("Player");
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}

