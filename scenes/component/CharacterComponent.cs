using Game.Resources.Character;
using Godot;

namespace Game.Component;

public partial class CharacterComponent : Node2D
{
	[Export(PropertyHint.File, "*.tres")]
	public string characterResourcePath;

	public CharacterResource characterResource { get; private set; }

	public bool hasMoved = false;

	public override void _Ready()
	{
		if (characterResourcePath != null)
		{
			characterResource = GD.Load<CharacterResource>(characterResourcePath);
		}

		AddToGroup(nameof(CharacterComponent));
	}

	public Vector2I GetGridCellPosition()
	{
		var gridPosition = GlobalPosition / 16;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
	}

}
