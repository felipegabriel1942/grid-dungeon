using Godot;

namespace Game.Resources.Equipment;

[GlobalClass]
public partial class WeaponResource : Resource
{
    [Export]
    public int damage { get; private set; }

    [Export]
    public PackedScene WeaponScene { get; private set; }
}
