using System;
using System.Collections.Generic;
using System.Linq;
using Game.Component;
using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{

	[Export]
	private TileMapLayer baseTerrainTileMapLayer;

	[Export]
	private TileMapLayer enemyHighlightTileMapLayer;

	[Export]
	private TileMapLayer playerHighlightTileMapLayer;

	private HashSet<Vector2I> walkableTiles = new();
	public HashSet<Vector2I> viewedTiles = new();

	public Vector2I GetMouseGridCellPosition()
	{
		var mousePosition = enemyHighlightTileMapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 16;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int)gridPosition.X, (int)gridPosition.Y);
	}

	public void UpdateValidWalkableTiles(CharacterComponent characterComponent)
	{
		walkableTiles.Clear();
		var rootCell = characterComponent.GetGridCellPosition();
		var tileArea = new Rect2I(rootCell, characterComponent.characterResource.Dimensions);
		var validTiles = GetValidTilesInRadius(tileArea, characterComponent.characterResource.MovementRadius);
		walkableTiles.UnionWith(validTiles);
		walkableTiles.ExceptWith(GetOccupiedTiles());
	}

	public void UpdateViewedTiles(CharacterComponent characterComponent)
    {
		viewedTiles.Clear();
		var rootCell = characterComponent.GetGridCellPosition();
		var tileArea = new Rect2I(rootCell, characterComponent.characterResource.Dimensions);
		var validTiles = GetTilesInRadius(tileArea, characterComponent.characterResource.viewRadius, (_) => true);
		viewedTiles.UnionWith(validTiles);
    }

	public void HighlightWalkableTiles()
	{
		foreach (var tile in walkableTiles)
		{
			enemyHighlightTileMapLayer.SetCell(tile, 1, Vector2I.Zero);
		}
	}

	private List<Vector2I> GetValidTilesInRadius(Rect2I tileArea, int radius)
	{
		return GetTilesInRadius(tileArea, radius, TileHasCustomData);
	}

	private List<Vector2I> GetTilesInRadius(Rect2I tileArea, int radius, Func<Vector2I, bool> filterFn)
	{
		var result = new List<Vector2I>();
		var tileAreaF = tileArea.ToRect2F();
		var tileAreaCenter = tileAreaF.GetCenter();
		var radiusMod = Mathf.Max(tileAreaF.Size.X, tileAreaF.Size.Y) / 2;

		for (var x = tileArea.Position.X - radius; x <= tileArea.Position.X + radius; x++)
		{
			for (var y = tileArea.Position.Y - radius; y <= tileArea.Position.Y + radius; y++)
			{
				var tilePosition = new Vector2I(x, y);

				if (!IsTileInsideCircle(tileAreaCenter, tilePosition, radius + radiusMod) || !filterFn(tilePosition)) continue;

				result.Add(tilePosition);
			}
		}

		return result;
	}

	private bool IsTileInsideCircle(Vector2 centerPosition, Vector2 tilePosition, float radius)
	{
		var distanceX = centerPosition.X - (tilePosition.X + .5);
		var distanceY = centerPosition.Y - (tilePosition.Y + .5);
		var distanceSquared = (distanceX * distanceX) + (distanceY * distanceY);
		return distanceSquared <= radius * radius;
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
		enemyHighlightTileMapLayer.Clear();
	}

	public bool IsTilePositionValid(Vector2I tilePosition)
	{
		return walkableTiles.Contains(tilePosition);
	}

	public bool EnemyHighlightTileMapLayerIsActive() {
		return enemyHighlightTileMapLayer.GetUsedCells().Count() > 0;
	}
}
