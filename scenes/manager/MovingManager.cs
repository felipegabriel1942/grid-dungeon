using System.Linq;
using Game.Autoload;
using Game.Character;
using Game.Component;
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

    private Vector2I hoveredGridCell;
    private CharacterGhost characterGhost;
    private CharacterComponent selectedCharacter;
    private CharacterComponent hoveredCharacter;
    private Node2D playerParty;

    public override void _Ready()
    {
        playerParty = GetNode<Node2D>("%PlayerParty");
    }
    
    public override void _Process(double delta)
	{
		var mouseGridPosition = gridManager.GetMouseGridCellPosition();
		hoveredGridCell = mouseGridPosition;

        hoveredCharacter = GetTree()
                .GetNodesInGroup(nameof(CharacterComponent))
                .Cast<CharacterComponent>()
                .FirstOrDefault((characterComponent) => characterComponent.GetGridCellPosition() == hoveredGridCell);

		if (characterGhost != null)
        {
            characterGhost.GlobalPosition = hoveredGridCell * 16;

            if (gridManager.IsTilePositionValid(hoveredGridCell))
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
            && hoveredCharacter != null
            && !hoveredCharacter.hasMoved
            && playerParty.FindChild(hoveredCharacter.GetParent().Name) != null
            && hoveredGridCell == hoveredCharacter.GetGridCellPosition())
        {
            selectedCharacter = hoveredCharacter;

            characterGhost = characterGhostScene.Instantiate<CharacterGhost>();

            var characterSprite = selectedCharacter.characterResource.CharacterSprite.Instantiate<Sprite2D>();

            characterGhost.AddChild(characterSprite);
            ySortRoot.AddChild(characterGhost);

            selectedCharacter = hoveredCharacter;
            gridManager.UpdateValidWalkableTiles(selectedCharacter);
            gridManager.HighlightWalkableTiles();
        }

        if (evt.IsActionPressed("left_click")
            && characterGhost != null
            && selectedCharacter != null
            && gridManager.IsTilePositionValid(hoveredGridCell))
        {
            selectedCharacter
                .GetParent<Node2D>()
                .GlobalPosition = hoveredGridCell * 16;

            ySortRoot.RemoveChild(characterGhost);
            selectedCharacter.hasMoved = true;
            characterGhost = null;
            selectedCharacter = null;
            gridManager.ClearHighlightedTiles();
            GameEvents.EmitPlayerMoved();
        }

        if (evt.IsActionPressed("right_click")) 
        {   
            ySortRoot.RemoveChild(characterGhost);
            characterGhost = null;
            selectedCharacter = null;
            gridManager.ClearHighlightedTiles();
        }
	}
}
