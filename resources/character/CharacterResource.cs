using Godot;

namespace Game.Resources.Character;

[GlobalClass]
public partial class CharacterResource : Resource
{
    [Export]
    public int MovementRadius { get; private set; }

    [Export]
    public PackedScene CharacterScene { get; private set; }

    [Export]
    public PackedScene CharacterSprite { get; private set; }
}
