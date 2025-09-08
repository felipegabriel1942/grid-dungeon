using Godot;

namespace Game.Character;

public partial class Rat : Area2D
{

	private EnemyMovementComponent enemyMovementComponent;

	public override void _Ready()
	{
		enemyMovementComponent = GetNode<EnemyMovementComponent>("EnemyMovementComponent");

		GD.Print(enemyMovementComponent);
	}

}
