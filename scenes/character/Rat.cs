using Game.Component;
using Godot;

namespace Game.Character;

public partial class Rat : Node2D
{
	[Export]
	public CharacterComponent characterComponent { get; private set; }
}
