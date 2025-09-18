using Game.Autoload;
using Game.Component;
using Godot;

namespace Game.Character;

public partial class Rat : Node2D
{

	[Export]
	public CharacterComponent characterComponent { get; private set; }

	[Export]
    public int detecctionRadius { get; private set; }

	private Vector2 targetPosition;
	private bool isMoving = false;
	private float speed = 100f;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		if (isMoving)
		{
			GlobalPosition = GlobalPosition.MoveToward(targetPosition, (float)delta * speed);

			if (GlobalPosition.DistanceTo(targetPosition) < 1f)
			{
				GlobalPosition = targetPosition;
				isMoving = false;
				GameEvents.EmitEnemyMoved(this);
			}
		}
	}

	public void MoveTo(Vector2 newTarget)
	{
		targetPosition = newTarget;
		isMoving = true;
	}
}
