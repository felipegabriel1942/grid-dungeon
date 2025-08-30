using System;
using System.Collections.Generic;
using System.Linq;
using Game.Character;
using Game.Component;
using Game.Resources.Character;
using Godot;

namespace Game.Manager;

public partial class MovingManager : Node
{
    [Export]
    private GridManager gridManager;

    [Export]
    private PackedScene characterGhostScene;

    [Export]
    private Node2D ySortRoot;

    private CharacterResource fighterResource;
    private Vector2I? hoveredGridCell;
    private CharacterGhost characterGhost;
    private CharacterComponent hoveredCharacter;
    private CharacterComponent selectedCharacter;


    public override void _Ready()
    {
        // fighterResource = GD.Load<CharacterResource>("res://resources/character/fighter.tres");

        // fighter = fighterResource.CharacterScene.Instantiate<Fighter>();
        // fighter.GlobalPosition = startingPosition;

        // GD.Print(fighter);

        // AddChild(fighter);
    }
    
    public override void _Process(double delta)
	{
		var mouseGridPosition = gridManager.GetMouseGridCellPosition();
		hoveredGridCell = mouseGridPosition;

        hoveredCharacter = GetTree()
                .GetNodesInGroup(nameof(CharacterComponent))
                .Cast<CharacterComponent>()
                .FirstOrDefault((characterComponent) => characterComponent.GetGridCellPosition() == hoveredGridCell.Value);
        
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
            && selectedCharacter == null
			&& hoveredGridCell == hoveredCharacter.GetGridCellPosition())
		{
            selectedCharacter = GetTree()
                .GetNodesInGroup(nameof(CharacterComponent))
                .Cast<CharacterComponent>()
                .FirstOrDefault((characterComponent) => characterComponent.GetGridCellPosition() == hoveredGridCell.Value);

			characterGhost = characterGhostScene.Instantiate<CharacterGhost>();

            var characterSprite = hoveredCharacter.characterResource.CharacterSprite.Instantiate<Sprite2D>();

            characterGhost.AddChild(characterSprite);
            ySortRoot.AddChild(characterGhost);

            selectedCharacter = hoveredCharacter;
			gridManager.UpdateValidWalkableTiles(selectedCharacter.GetGridCellPosition());
			gridManager.HighlightWalkableTiles();
		}

		if (evt.IsActionPressed("left_click")
            && characterGhost != null
            && selectedCharacter != null
            && gridManager.IsTilePositionBuildable(hoveredGridCell.Value))
        {
            // var character = GetTree()
            //     .GetNodesInGroup(nameof(CharacterComponent))
            //     .Cast<CharacterComponent>()
            //     .FirstOrDefault((characterComponent) => characterComponent == selectedCharacter);

            selectedCharacter.GlobalPosition = characterGhost.GlobalPosition;

            ySortRoot.RemoveChild(characterGhost);
            characterGhost = null;
            selectedCharacter = null;
            gridManager.ClearHighlightedTiles();
        }
	}

}
