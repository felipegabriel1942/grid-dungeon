using Game.Manager;
using Godot;
using System;
using System.Diagnostics.Tracing;

namespace Game;

public partial class Main : Node
{

	private GridManager gridManager;
	private Sprite2D cursor;
	private CharacterBody2D fighter;
	private Vector2I startingPosition = new Vector2I(2, 4) * 16;
	private Vector2I? hoveredGridCell;

	public override void _Ready()
	{
		gridManager = GetNode<GridManager>("GridManager");
		cursor = GetNode<Sprite2D>("Cursor");
		fighter = GetNode<CharacterBody2D>("Fighter");
		fighter.GlobalPosition = startingPosition;

		gridManager.HighlightWalkableTiles();
	}

	public override void _Process(double delta)
	{
		var mouseGridPosition = gridManager.GetMouseGridCellPosition();
		cursor.GlobalPosition = mouseGridPosition * 16;
		hoveredGridCell = mouseGridPosition;
	}

	public override void _UnhandledInput(InputEvent evt)
	{
		if (evt.IsActionPressed("left_click"))
		{
			GD.Print(gridManager.TileHasCustomData(hoveredGridCell.Value));
		}
	}
}
