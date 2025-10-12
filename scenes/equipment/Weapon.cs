using Godot;
using System;
using Game.Resources.Equipment;

public partial class Weapon : Node2D
{
    [Export]
    public WeaponResource weaponResource;

    public override void _Ready()
    {
        AddChild(weaponResource.WeaponScene.Instantiate());
    }
}
