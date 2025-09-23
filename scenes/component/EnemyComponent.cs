using Godot;

public partial class EnemyComponent : Node2D
{
	public override void _Ready()
	{
		AddToGroup(nameof(EnemyComponent));
	}
}
