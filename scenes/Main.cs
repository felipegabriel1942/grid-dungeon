using Game.Character;
using Game.Manager;
using Game.Resources.Character;
using Godot;

namespace Game;

public partial class Main : Node
{
	private GridManager gridManager;
	private Sprite2D cursor;
	private CharacterResource fighterResource;
	private Vector2I startingPosition = new Vector2I(2, 4) * 16;
	private Vector2I? hoveredGridCell;
	private Fighter? fighter;

	public override void _Ready()
	{
		gridManager = GetNode<GridManager>("GridManager");
		cursor = GetNode<Sprite2D>("Cursor");
		fighterResource = GD.Load<CharacterResource>("res://resources/character/fighter.tres");
		fighter = fighterResource.CharacterScene.Instantiate<Fighter>();
		fighter.GlobalPosition = startingPosition;

		AddChild(fighter);
	}

	public override void _Process(double delta)
	{
		var mouseGridPosition = gridManager.GetMouseGridCellPosition();
		cursor.GlobalPosition = mouseGridPosition * 16;
		hoveredGridCell = mouseGridPosition;
	}

	public override void _UnhandledInput(InputEvent evt)
	{
		if (evt.IsActionPressed("left_click")
			&& hoveredGridCell == fighter.characterComponent.GetGridCellPosition())
		{
			cursor.Visible = true;
			gridManager.UpdateValidWalkableTiles(fighter.characterComponent.GetGridCellPosition());
			gridManager.HighlightWalkableTiles();
		}

		if (evt.IsActionPressed("left_click")
			&& cursor.Visible
			&& hoveredGridCell != fighter.characterComponent.GetGridCellPosition()
			&& gridManager.IsTilePositionBuildable(hoveredGridCell.Value))
		{
			fighter.GlobalPosition = hoveredGridCell.Value * 16;
			cursor.Visible = false;
			gridManager.ClearHighlightedTiles();
		}
	}
}
