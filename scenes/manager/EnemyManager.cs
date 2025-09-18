using System.Collections.Generic;
using System.Linq;
using Game.Character;
using Game.UI;
using Godot;

namespace Game.Manager;

public partial class EnemyManager : Node
{

    private const int TILE_SIZE = 16;

    [Export]
    private TileMapLayer tileMapLayer;

    [Export]
    private Fighter fighter;

    [Export]
    private GameUi gameUi;

    [Export]
    private bool developMode = false;

    [Export]
    private GridManager gridManager;

    private readonly AStarGrid2D pathfindingGrid = new();
    private readonly Vector2[] pathToPlayer = [];
    private Dictionary<Rat, Line2D> visualPathDictionary = new();
    private HashSet<Rat> enemies = new();
    private Queue<Rat> moveQueue = new();

    public override void _Ready()
    {
        InitPathfinding();
        InitEnemies();
        InitDebugPaths();

        gameUi.MovingEnemy += StartEnemyTurn;

        HighlightEnemiesWalkableTiles();
    }

    private void InitPathfinding()
    {
        pathfindingGrid.Region = tileMapLayer.GetUsedRect();
        pathfindingGrid.CellSize = new Vector2I(TILE_SIZE, TILE_SIZE);
        pathfindingGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Always;
        pathfindingGrid.Update();

        foreach (var cell in tileMapLayer.GetUsedCells())
        {
            var data = tileMapLayer.GetCellTileData(cell);
            bool isWalkable = (bool)data.GetCustomData("is_walkable");
            pathfindingGrid.SetPointSolid(cell, !isWalkable);
        }
    }

    private void InitEnemies()
    {
        enemies = GetNode("%EnemyParty").GetChildren().Cast<Rat>().ToHashSet();
    }

    private void InitDebugPaths()
    {
        if (!developMode) return;

        foreach (var enemy in enemies)
        {
            var visualPath = new Line2D
            {
                Width = 4.0f,
                DefaultColor = Colors.Red,
                GlobalPosition = Vector2I.Zero
            };

            GetParent().CallDeferred("add_child", visualPath);
            visualPathDictionary.Add(enemy, visualPath);
        }
    }

    private void HighlightEnemiesWalkableTiles()
    {
        foreach (var enemy in enemies)
        {
            gridManager.UpdateValidWalkableTiles(enemy.characterComponent.GetGridCellPosition());
            gridManager.HighlightWalkableTiles();
        }
    }

    private void StartEnemyTurn()
    {
        moveQueue = new Queue<Rat>(enemies);
        MoveNextEnemy();
    }

    private void MoveNextEnemy()
    {
        if (moveQueue.Count == 0) return;

        var enemy = moveQueue.Dequeue();
        enemy.MoveCompleted += OnEnemyMoveCompleted;

        var path = CalculatePath(enemy);

        Vector2I enemyCell = tileMapLayer.LocalToMap(enemy.GlobalPosition);
        Vector2I fighterCell = tileMapLayer.LocalToMap(fighter.GlobalPosition);

        if (path.Length > 1 && CanDetectPlayer(enemyCell, fighterCell, enemy.detecctionRadius))
        {
            UpdateGridOccupancy(enemy, path[0]);
            enemy.MoveTo(path[0]);
            UpdateDebugPath(enemy, path);
        }
        else
        {
            MoveNextEnemy();
        }
    }

    private bool CanDetectPlayer(Vector2I enemyPos, Vector2I playerPos, int detecctionRadius) {
        return enemyPos.DistanceTo(playerPos) <= detecctionRadius;
    }

    private void OnEnemyMoveCompleted(Rat enemy)
    {
        enemy.MoveCompleted -= OnEnemyMoveCompleted;

        MoveNextEnemy();
    }

    private Vector2[] CalculatePath(Rat enemy)
    {
        var path = pathfindingGrid.GetPointPath(
            (Vector2I)(enemy.GlobalPosition / TILE_SIZE),
            (Vector2I)(fighter.GlobalPosition / TILE_SIZE)
        );

        var radius = enemy.characterComponent.characterResource?.MovementRadius ?? 1;

        if (path.Length - radius == 1) radius = 1;

        return path.Skip(radius).ToArray();
    }

    private void UpdateGridOccupancy(Rat enemy, Vector2 target)
    {
        pathfindingGrid.SetPointSolid((Vector2I)(enemy.GlobalPosition / TILE_SIZE), false);
        pathfindingGrid.SetPointSolid((Vector2I)(target / TILE_SIZE), true);
    }

    private void UpdateDebugPath(Rat enemy, Vector2[] path)
    {
        if (developMode && visualPathDictionary.TryGetValue(enemy, out var line))
        {
            line.Points = path;
        }
    }
}
