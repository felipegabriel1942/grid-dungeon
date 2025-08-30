using Game.Component;
using Godot;

namespace Game.Character;

public partial class Fighter : CharacterBody2D
{
    
    [Export]
    public CharacterComponent characterComponent { get; private set; }
}
