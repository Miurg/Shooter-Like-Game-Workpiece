using Godot;
using System;
using System.Text.RegularExpressions;
using static Godot.Input;

public partial class MainNode : Node
{
	private HUD HUD;
    private Node Objects;
	private Node AllBulletsAndHoles;
    public override void _Ready()
	{
		HUD = GetNode<HUD>("/root/MainNode/HUD");
        Objects = GetNode<Node>("/root/MainNode/Objects");
        AllBulletsAndHoles = GetNode<Node>("/root/MainNode/Objects/AllBulletsAndHoles");
        Input.MouseMode = MouseModeEnum.Captured;
        EmitSignal("MySignal");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey key)
        {
            if (key.Keycode == Key.Escape)
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

    public void CreateRemainsFromWeapon(CollisionObject3D Wall, Vector3 Position, Vector3 Normal, PackedScene Hole)
    {
        Node3D newHole = Hole.Instantiate<Node3D>();
        newHole.Position = Position+Normal/1000;
        AllBulletsAndHoles.AddChild(newHole);
        if (Normal.Y!=1 && Normal.Y != -1) newHole.LookAt(Position+Normal,Vector3.Up);
        else newHole.LookAt(Position+Normal,Vector3.Forward);
        newHole.Rotation = newHole.Rotation with { Z = Mathf.DegToRad(new Random().Next(0, 360)) };
    }
}
