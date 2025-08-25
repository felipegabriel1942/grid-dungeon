using System;
using System.Collections.Generic;
using System.Net;
using Game.Component;
using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{

	[Export]
	private TileMapLayer baseTerrainTileMapLayer;

	[Export]
	private TileMapLayer highlightTileMapLayer;

	private HashSet<Vector2I> walkableTiles = new();

	public override void _Ready()
	{
		GD.Print(baseTerrainTileMapLayer.GetChildren().Count);
	}

	public Vector2I GetMouseGridCellPosition()
	{
		var mousePosition = highlightTileMapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 16;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
	}

	public void UpdateValidWalkableTiles(CharacterComponent characterComponent)
	{
		var rootCell = characterComponent.GetGridCellPosition();
		var validTiles = GetValidTilesInRadius(rootCell, 3);
		walkableTiles.UnionWith(validTiles);
	}

	public void HighlightWalkableTiles()
	{
		foreach (var tile in walkableTiles)
		{
			highlightTileMapLayer.SetCell(tile, 0, Vector2I.Zero);
		}
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



	private List<Vector2I> GetValidTilesInRadius(Vector2I rootCell, int radius)
	{
		return GetTilesInRadius(rootCell, radius, (tilePosition) =>
			{
				return TileHasCustomData(tilePosition);
			}
		);
	}

	private List<Vector2I> GetTilesInRadius(Vector2I rootCell, int radius, Func<Vector2I, bool> filterFn)
	{
		var result = new List<Vector2I>();

		for (int x = rootCell.X - radius; x <= rootCell.X; x++)
		{
			for (int y = rootCell.Y - radius; y <= rootCell.Y + radius; y++)
			{
				var tilePosition = new Vector2I(x, y);

				if (!filterFn(tilePosition)) continue;

				result.Add(tilePosition);
			}
		}

		return result;
	}
}
