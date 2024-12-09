using Godot;
using System;

public partial class HUD : Node
{
	Control IcondsSelected;
	Label RoundsPocket;
	Label RoundsCurrent;

	int SelectedPositionX;
	int SelectedPositionY;
	int SelectedSizeX;
	int SelectedSizeY;

	float SpreadWeapon { get => SpreadWeapon;
			set
			{
				SpreadWeapon = value;
				SpreadApply();
			}
		}

	public override void _Ready()
	{
        IcondsSelected = GetNode<Control>("IconsSelected");
        RoundsPocket = GetNode<Label>("HBoxContainer/RoundsPocket");
        RoundsCurrent = GetNode<Label>("HBoxContainer/RoundsCurrent");
    }

	public void setRoundsPocket(int value)
	{
        RoundsPocket.Text = value.ToString();

    }

	public void setRoundsCurrent(int value)
	{
		RoundsCurrent.Text = value.ToString();
	}

	public void UpdateSelected(int sizeXCurrent, int sizeYCurrent, int positionXCurrent, int positionYCurrent)
	{
		SelectedSizeX = sizeXCurrent;
		SelectedSizeY = sizeYCurrent;
		SelectedPositionX = positionXCurrent;
		SelectedPositionY = positionYCurrent;
	}

	public void NormalSelected()
	{
		SelectedSizeX = 34;
		SelectedSizeY = 34;
		SelectedPositionX = (int)((GetViewport().GetVisibleRect().Size.X/2)- SelectedSizeX/2);
		SelectedPositionY = (int)((GetViewport().GetVisibleRect().Size.Y / 2) - SelectedSizeY / 2);
	}

	public void SelectedApply(float delta)
	{
		IcondsSelected.Size = IcondsSelected.Size.Lerp(new Vector2(SelectedSizeX, SelectedSizeY), 10 * delta);
		IcondsSelected.Position = IcondsSelected.Position.Lerp(new Vector2(SelectedPositionX, SelectedPositionY), 10 * delta);

    }

	public void SpreadApply()
	{
		IcondsSelected.Size = new Vector2(SelectedSizeX * (SpreadWeapon + 1), SelectedSizeY * (SpreadWeapon + 1));
		IcondsSelected.Position = new Vector2((SelectedPositionX+SelectedSizeX/2)-(IcondsSelected.Size.X/2),
			(SelectedPositionY + SelectedSizeY / 2) - (IcondsSelected.Size.Y / 2));

    }

	public override void _Process(double delta)
	{
		if (SpreadWeapon <= 0)
		{
			SelectedApply((float)delta);
		}
	}
}
