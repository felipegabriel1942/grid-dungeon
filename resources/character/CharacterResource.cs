using Godot;

namespace Game.Resources.Character;

public enum CharacterType {
    player,
    enemy
}

[GlobalClass]
public partial class CharacterResource : Resource
{
    [Export]
    public int MovementRadius { get; private set; }

    [Export]
    public int viewRadius { get; private set;  }

    [Export]
    public Vector2I Dimensions { get; private set; } = Vector2I.One;

    [Export]
    public PackedScene CharacterScene { get; private set; }

    [Export]
    public PackedScene CharacterSprite { get; private set; }

    [Export]
    public CharacterType characterType { get; private set; }
}
