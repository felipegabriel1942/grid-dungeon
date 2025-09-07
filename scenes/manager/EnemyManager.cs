using System.Linq;
using Game.Character;
using Game.UI;
using Godot;

public partial class EnemyManager : Node
{

    private const int TILE_SIZE = 16;

    [Export]
    private TileMapLayer tileMapLayer;

    [Export]
    private Fighter fighter;

    [Export]
    private Node2D enemy;

    [Export]
    private Line2D visualEnemyPath = null;

    [Export]
    private GameUi gameUi;

    private AStarGrid2D pathfindingGrid = new();

    private Vector2[] pathToPlayer = [];

    public override void _Ready()
    {
        gameUi.MovingEnemy += MoveEnemy;

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

        MoveEnemy();
    }


    private void MoveEnemy()
    {
        pathToPlayer = pathfindingGrid.GetPointPath((Vector2I)(enemy.GlobalPosition / TILE_SIZE), (Vector2I)(fighter.GlobalPosition / TILE_SIZE));
        visualEnemyPath.Points = pathToPlayer;

        pathToPlayer = pathToPlayer.Skip(1).ToArray();

        if (!pathToPlayer.IsEmpty())
        {
            var goToPosition = pathToPlayer[0] + new Vector2I(TILE_SIZE / 2, TILE_SIZE / 2);

            enemy.GlobalPosition = goToPosition;

            visualEnemyPath.Points = pathToPlayer;

        }
    }

}
