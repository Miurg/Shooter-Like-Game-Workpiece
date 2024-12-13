using Godot;
using System;

public interface IWeapon: IObject
{
    public void Attack(float spread);
}
