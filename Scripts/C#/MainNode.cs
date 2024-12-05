using Godot;
using System;
using System.Text.RegularExpressions;
using static Godot.Input;

public partial class MainNode: Node
{
	private CanvasLayer HUD;
    private Node Objects;
	private Node allBulletsAndHoles;
    public override void _Ready()
	{
		HUD = GetNode<CanvasLayer>("/root/MainNode/HUD");
        Objects = GetNode<Node>("/root/MainNode/Objects");
        allBulletsAndHoles = GetNode<Node>("/root/MainNode/Objects/AllBulletsAndHoles");
        Input.MouseMode = MouseModeEnum.Captured;
        EmitSignal("MySignal");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.GetType() == typeof(InputEventKey))
        {
            if (((InputEventKey)@event).Keycode == Key.Escape)
            {
                GetTree().Quit();
            }
        }
    }

    [Signal]
    public delegate void MySignalEventHandler();

    public void PlaceWeapon(Node3D FromWho, RigidBody3D Weapon, Vector3 Impulse, Vector3 Position)
    {
        Objects.AddChild(Weapon);
        Weapon.Position = Position;
        Weapon.ApplyImpulse(Impulse);
        if (Weapon.Position != FromWho.Position with { Y = Weapon.Position.Y })
        {
            Weapon.LookAt(FromWho.Position with { Y = Weapon.Position.Y });
        }
        Weapon.RotateY(Mathf.DegToRad(-90));
    }

}
