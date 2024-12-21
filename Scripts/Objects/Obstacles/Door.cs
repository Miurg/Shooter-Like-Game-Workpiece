using Godot;
using System;

public partial class Door : StaticBody3D, IObject
{

	[Export] private float AngleToOpen;
	[Export] private float SpeedOpening;
	private bool _IsOpen = true;
	private bool _IsMove = false;
	private float _MinAngle;
	private float _MaxAngle;
    public override void _Ready()
	{
		_MinAngle = Rotation.Y;
		_MaxAngle = Rotation.Y + Mathf.DegToRad(AngleToOpen);
        GD.Print(_MinAngle, _MaxAngle);
    }

	public override void _Process(double delta)
	{
		if (_IsMove)
		{
			if (!_IsOpen && _MaxAngle > Rotation.Y)
			{
				Rotation = Rotation with { Y = Rotation.Y + Mathf.DegToRad(SpeedOpening) * (float)delta };
			}
			else if (_IsOpen && _MinAngle < Rotation.Y)
			{
                Rotation = Rotation with { Y = Rotation.Y - Mathf.DegToRad(SpeedOpening) * (float)delta };
            }
			else _IsMove = false;
		}

	}

	public void Activate()
	{
        _IsMove = true;
		if (_IsOpen) _IsOpen= false;
		else if (!_IsOpen) _IsOpen = true;
    }

    public void Die()
    {
        throw new NotImplementedException();
    }
}
