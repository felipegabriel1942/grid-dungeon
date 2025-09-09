using Game.Component;
using Godot;

namespace Game.Character;

public partial class Fighter : Node2D
{
	
	[Export]
	public CharacterComponent characterComponent { get; private set; }
	
}
