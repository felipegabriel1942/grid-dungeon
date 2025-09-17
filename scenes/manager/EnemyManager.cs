using System.Collections.Generic;
using System.Linq;
using Game.Character;
using Game.UI;
using Godot;

namespace Game.Manager;

public partial class EnemyManager : Node
{

    private const int TILE_SIZE = 16;
    private static readonly Vector2I CELL_CENTER_OFFSET = new(TILE_SIZE / 2, TILE_SIZE / 2);

    [Export]
    private TileMapLayer tileMapLayer;

    [Export]
    private Fighter fighter;

    [Export]
    private GameUi gameUi;

    [Export]
    private bool developMode = false;

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

        StartEnemyTurn();
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
                GlobalPosition = CELL_CENTER_OFFSET
            };

            GetParent().CallDeferred("add_child", visualPath);
            visualPathDictionary.Add(enemy, visualPath);
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
        if (path.Length > 1)
        {
            var goTo = path[0] + CELL_CENTER_OFFSET;
            UpdateGridOccupancy(enemy, goTo);
            enemy.MoveTo(goTo);
            UpdateDebugPath(enemy, path);
        }
        else
        {
            MoveNextEnemy();
        }
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
