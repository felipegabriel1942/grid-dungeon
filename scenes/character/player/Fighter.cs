using Game.Autoload;
using Game.Component;
using Godot;

namespace Game.Character;

public partial class Fighter : Node2D
{

	[Export]
	public CharacterComponent characterComponent { get; private set; }

	[Export]
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		GameEvents.Instance.PlayerAttack += OnPlayerAttack;
	}

	private void OnPlayerAttack()
	{
		animationPlayer.Play("attack");
	}
	
}
