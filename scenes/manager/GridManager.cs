using System;
using System.Collections.Generic;
using System.Linq;
using Game.Component;
using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{

	private const int TILE_SIZE = 16;

	[Export]
	private TileMapLayer baseTerrainTileMapLayer;

	[Export]
	private TileMapLayer highlightTileMapLayer;

	private HashSet<Vector2I> walkableTiles = new();

	public override void _Ready()
	{

	}

	public Vector2I GetMouseGridCellPosition()
	{
		var mousePosition = highlightTileMapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 16;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
	}

	public void UpdateValidWalkableTiles(Vector2I rootCell)
	{
		walkableTiles.Clear();
		var validTiles = GetValidTilesInRadius(rootCell, 2);
		walkableTiles.UnionWith(validTiles);
		walkableTiles.ExceptWith(GetOccupiedTiles());
	}

	public void HighlightWalkableTiles()
	{
		foreach (var tile in walkableTiles)
		{
			highlightTileMapLayer.SetCell(tile, 1, Vector2I.Zero);
		}
	}

	private List<Vector2I> GetValidTilesInRadius(Vector2I rootCell, int radius)
	{
		return GetTilesInRadius(rootCell, radius, TileHasCustomData);
	}

	private List<Vector2I> GetTilesInRadius(Vector2I rootCell, int radius, Func<Vector2I, bool> filterFn)
	{
		var result = new List<Vector2I>();

		for (var x = rootCell.X - radius; x <= rootCell.X + radius; x++)
		{
			for (var y = rootCell.Y - radius; y <= rootCell.Y + radius; y++)
			{
				var tilePosition = new Vector2I(x, y);

				if (!filterFn(tilePosition)) continue;

				result.Add(tilePosition);
			}
		}

		return result;
	}

	public bool TileHasCustomData(Vector2I tilePosition)
	{
		var layer = baseTerrainTileMapLayer;
		var customData = layer.GetCellTileData(tilePosition);

		if (customData != null)
		{
			return (bool)customData.GetCustomData("is_walkable");
		}

		return false;
	}

	private IEnumerable<Vector2I> GetOccupiedTiles()
	{
		var characterComponents = GetTree()
			.GetNodesInGroup(nameof(CharacterComponent))
			.Cast<CharacterComponent>();

		return characterComponents.Select(x => x.GetGridCellPosition());
	}

	public void ClearHighlightedTiles()
	{
		highlightTileMapLayer.Clear();
	}

	public bool IsTilePositionValid(Vector2I tilePosition)
	{
		return walkableTiles.Contains(tilePosition);
	}
}
