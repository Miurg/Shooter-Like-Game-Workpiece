using Godot;
using player;
using System;

public partial class HUD : CanvasLayer
{
	Control IcondsSelected;
	private Label _RoundsPocket;
	private Label _RoundsCurrent;
	private Label _HP;

	private int _SelectedPositionX;
    private int _SelectedPositionY;
	private int _SelectedSizeX;
	private int _SelectedSizeY;

	private float _SpreadWeapon;

	[Export] PlayerWeapons PlayerWeapons;
	[Export] PlayerMain PlayerMain;

    public float SpreadWeapon { get => _SpreadWeapon;
			set
			{
				_SpreadWeapon = value;
			}
		}

    public int RoundsPocket { 
		get 
		{
            if (int.TryParse(_RoundsPocket.Text, out int result)) return result;
            else return 0;
        }
		set => _RoundsPocket.Text = value.ToString(); }
    public int RoundsCurrent
    {
        get
        {
            if (int.TryParse(_RoundsCurrent.Text, out int result)) return result;
			else return 0;
        }
        set => _RoundsCurrent.Text = value.ToString();
    }

    public int HP { 
		set
		{
			_HP.Text = "HP:" + value.ToString();
		}
	}

    public override void _Ready()
	{
        PlayerWeapons = GetNode<PlayerWeapons>("/root/MainNode/Objects/Player/Weapons");
		PlayerWeapons.CurrentSpreadChange += UpdateSpread;
		PlayerWeapons.CurrentRoundsChange += UpdateCurrentRounds;
        PlayerWeapons.PocketRoundsChange += UpdateRoundsPocket;
        PlayerMain = GetNode<PlayerMain>("/root/MainNode/Objects/Player");
        PlayerMain.HPChange += UpdateHP;
        IcondsSelected = GetNode<Control>("IconsSelected");
        _RoundsPocket = GetNode<Label>("HBoxContainer/RoundsPocket");
        _RoundsCurrent = GetNode<Label>("HBoxContainer/RoundsCurrent");
		_HP = GetNode<Label>("HBoxContainer/HP");
    }

    public void UpdateRoundsPocket(int value)
    {
        RoundsPocket = value;

    }

    public void UpdateCurrentRounds(int value)
	{
		RoundsCurrent = value;

    }

	public void UpdateHP(int value)
	{
		HP = value;
	}


    public void UpdateSpread(float spread)
	{
		SpreadWeapon = spread;
        IcondsSelected.Size = new Vector2(_SelectedSizeX * (SpreadWeapon + 1), _SelectedSizeY * (SpreadWeapon + 1));
        IcondsSelected.Position = new Vector2((_SelectedPositionX + _SelectedSizeX / 2) - (IcondsSelected.Size.X / 2),
            (_SelectedPositionY + _SelectedSizeY / 2) - (IcondsSelected.Size.Y / 2));
    }

	public void UpdateSelected(int sizeXCurrent, int sizeYCurrent, int positionXCurrent, int positionYCurrent)
	{
		_SelectedSizeX = sizeXCurrent;
		_SelectedSizeY = sizeYCurrent;
		_SelectedPositionX = positionXCurrent;
		_SelectedPositionY = positionYCurrent;
	}

	public void NormalSelected()
	{
		_SelectedSizeX = 34;
		_SelectedSizeY = 34;
		_SelectedPositionX = (int)((GetViewport().GetVisibleRect().Size.X/2)- _SelectedSizeX/2);
		_SelectedPositionY = (int)((GetViewport().GetVisibleRect().Size.Y / 2) - _SelectedSizeY / 2);
	}

	public void SelectedApply(float delta)
	{
		IcondsSelected.Size = IcondsSelected.Size.Lerp(new Vector2(_SelectedSizeX, _SelectedSizeY), 10 * delta);
		IcondsSelected.Position = IcondsSelected.Position.Lerp(new Vector2(_SelectedPositionX, _SelectedPositionY), 10 * delta);

    }

	public override void _Process(double delta)
	{
		if (SpreadWeapon <= 0)
		{
			SelectedApply((float)delta);
		}
	}
}
