using Game.Character;
using Game.Manager;
using Game.Resources.Character;
using Godot;

namespace Game;

public partial class Main : Node
{

	[Export]
	private PackedScene characterGhostScene;

	private GridManager gridManager;
	private CharacterResource fighterResource;
	private Vector2I startingPosition = new Vector2I(2, 4) * 16;
	private Vector2I? hoveredGridCell;
	private Fighter fighter;
	private CharacterGhost characterGhost;

	public override void _Ready()
	{
		gridManager = GetNode<GridManager>("GridManager");
		fighterResource = GD.Load<CharacterResource>("res://resources/character/fighter.tres");
		fighter = fighterResource.CharacterScene.Instantiate<Fighter>();
		fighter.GlobalPosition = startingPosition;

		AddChild(fighter);
	}

	public override void _Process(double delta)
	{
		var mouseGridPosition = gridManager.GetMouseGridCellPosition();
		hoveredGridCell = mouseGridPosition;

		if (characterGhost != null)
		{
			characterGhost.GlobalPosition = hoveredGridCell.Value * 16;

			if (gridManager.IsTilePositionBuildable(hoveredGridCell.Value))
			{
				characterGhost.SetValid();
			}
			else
			{
				characterGhost.SetInvalid();
			}
		}
	}

	public override void _UnhandledInput(InputEvent evt)
	{
		if (evt.IsActionPressed("left_click")
			&& characterGhost == null
			&& hoveredGridCell == fighter.characterComponent.GetGridCellPosition())
		{
			characterGhost = characterGhostScene.Instantiate<CharacterGhost>();
			var fighterSprite = fighterResource.CharacterSprite.Instantiate<Sprite2D>();
			characterGhost.AddChild(fighterSprite);
			AddChild(characterGhost);

			gridManager.UpdateValidWalkableTiles(fighter.characterComponent.GetGridCellPosition());
			gridManager.HighlightWalkableTiles();
		}

		if (evt.IsActionPressed("left_click")
			&& hoveredGridCell != fighter.characterComponent.GetGridCellPosition()
			&& characterGhost != null
			&& gridManager.IsTilePositionBuildable(hoveredGridCell.Value))
		{
			fighter.GlobalPosition = hoveredGridCell.Value * 16;
			RemoveChild(characterGhost);
			characterGhost = null;
			gridManager.ClearHighlightedTiles();
		}
	}
}
