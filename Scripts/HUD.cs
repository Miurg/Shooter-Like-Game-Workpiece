using Godot;
using player;
using System;

public partial class HUD : CanvasLayer
{
	Control IconsSelected;
	Control IconsAim;
	private Label _RoundsPocket;
	private Label _RoundsCurrent;
	private Label _HP;

	private int _NormalPositionX;
	private int _NormalPositionY;
    private int _NormalSizeX;
    private int _NormalSizeY;

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
        _NormalPositionX = (int)((GetViewport().GetVisibleRect().Size.X / 2) - 34 / 2);
		_NormalPositionY = (int)((GetViewport().GetVisibleRect().Size.Y / 2) - 34 / 2);
		_NormalSizeX = 34;
		_NormalSizeY = 34;
        PlayerWeapons = GetNode<PlayerWeapons>("/root/MainNode/Objects/NPC and Player/Player/Weapons");
		PlayerWeapons.CurrentSpreadChange += UpdateSpread;
		PlayerWeapons.CurrentRoundsChange += UpdateCurrentRounds;
        PlayerWeapons.PocketRoundsChange += UpdateRoundsPocket;
        PlayerMain = GetNode<PlayerMain>("/root/MainNode/Objects/NPC and Player/Player");
        PlayerMain.HPChange += UpdateHP;
        IconsAim = GetNode<Control>("IconsAim");
        IconsSelected = GetNode<Control>("IconsSelected");
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
        if (SpreadWeapon > 0)
		{
            IconsAim.Size = new Vector2(_NormalSizeX * (spread + 1), _NormalSizeY * (spread + 1));
            IconsAim.Position = new Vector2((_NormalPositionX + _NormalSizeX / 2) - (IconsAim.Size.X / 2),
                (_NormalPositionY + _NormalSizeY / 2) - (IconsAim.Size.Y / 2));
        }
		else
		{
            IconsAim.Size = new Vector2(_NormalSizeX, _NormalSizeY);
            IconsAim.Position = new Vector2(_NormalPositionX, _NormalPositionY);
        }

    }
	public bool NowSelected = false;
	public void UpdateSelected(int sizeXCurrent, int sizeYCurrent, int positionXCurrent, int positionYCurrent)
	{
		_SelectedSizeX = sizeXCurrent;
		_SelectedSizeY = sizeYCurrent;
		_SelectedPositionX = positionXCurrent;
		_SelectedPositionY = positionYCurrent;
		NowSelected = true;
    }

	public void NormalSelected(float delta)
	{
        IconsSelected.Size = IconsSelected.Size.Lerp(new Vector2(IconsAim.Size.X, IconsAim.Size.Y), 10 * delta);
        IconsSelected.Position = IconsSelected.Position.Lerp(new Vector2(IconsAim.Position.X, IconsAim.Position.Y), 10 * delta);
    }

	public void SelectedApply(float delta)
	{
		IconsSelected.Size = IconsSelected.Size.Lerp(new Vector2(_SelectedSizeX, _SelectedSizeY), 10 * delta);
		IconsSelected.Position = IconsSelected.Position.Lerp(new Vector2(_SelectedPositionX, _SelectedPositionY), 10 * delta);

    }

	public override void _Process(double delta)
	{
		if (NowSelected) SelectedApply((float)delta);
		else NormalSelected((float)delta);

    }
}
