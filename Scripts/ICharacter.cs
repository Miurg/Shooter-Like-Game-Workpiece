using Godot;
using System;


interface ICharacter: IObject
{
    void ApplyGravityForce(float delta);
}