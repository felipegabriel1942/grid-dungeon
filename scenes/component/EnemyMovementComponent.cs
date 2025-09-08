using Godot;
using System;
using System.Linq;

public partial class EnemyMovementComponent : Node2D
{
	private const int TILE_SIZE = 16;

	private TileMapLayer tileMapLayer;

	[Export]
	private Line2D visualEnemyPath = null;

	private AStarGrid2D pathfindingGrid = new();

	private Vector2[] pathToTarget = [];

	private Node2D target;

	public override void _Ready()
	{
		
	}

	public void Initialize(Node2D target, TileMapLayer tileMapLayer)
	{
		this.target = target;
		this.tileMapLayer = tileMapLayer;

		visualEnemyPath.GlobalPosition = new Vector2I(TILE_SIZE / 2, TILE_SIZE / 2);

		pathfindingGrid.Region = tileMapLayer.GetUsedRect();
		pathfindingGrid.CellSize = new Vector2I(TILE_SIZE, TILE_SIZE);
		pathfindingGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Never;
		pathfindingGrid.Update();

		foreach (var cell in tileMapLayer.GetUsedCells())
		{
			var customData = tileMapLayer.GetCellTileData(cell);
			pathfindingGrid.SetPointSolid(cell, !(bool)customData.GetCustomData("is_walkable"));
		}

		Move();
	}


	public void Move()
	{
		pathToTarget = pathfindingGrid.GetPointPath((Vector2I)(GlobalPosition / TILE_SIZE), (Vector2I)(target.GlobalPosition / TILE_SIZE));
		visualEnemyPath.Points = pathToTarget;

		pathToTarget = pathToTarget.Skip(1).ToArray();

		if (pathToTarget.Count() > 1)
		{
			var goToPosition = pathToTarget[0] + new Vector2I(TILE_SIZE / 2, TILE_SIZE / 2);

			GlobalPosition = goToPosition;

			visualEnemyPath.Points = pathToTarget;

		}
	}
}
