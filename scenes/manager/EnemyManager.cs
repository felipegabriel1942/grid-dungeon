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
    private Rat[] enemies = [];

    private AStarGrid2D pathfindingGrid = new();

    private Vector2[] pathToPlayer = [];

    private HashSet<Vector2I> reservedCells = new();

    private Dictionary<Rat, Line2D> visualPathDictionary = new();

    public override void _Ready()
    {
        gameUi.MovingEnemy += MoveEnemies;

        pathfindingGrid.Region = tileMapLayer.GetUsedRect();
        pathfindingGrid.CellSize = new Vector2I(TILE_SIZE, TILE_SIZE);
        pathfindingGrid.DiagonalMode = AStarGrid2D.DiagonalModeEnum.Always;
        pathfindingGrid.Update();

        foreach (var cell in tileMapLayer.GetUsedCells())
        {
            var customData = tileMapLayer.GetCellTileData(cell);
            pathfindingGrid.SetPointSolid(cell, !(bool)customData.GetCustomData("is_walkable"));
        }

        CreateVisualPaths();
        MoveEnemies();
    }

    private void CreateVisualPaths()
    {
        foreach (var enemy in enemies)
        {
            Line2D visualPath = new Line2D();
            visualPath.Width = 4.0f;
            visualPath.DefaultColor = Colors.Red;
            visualPath.GlobalPosition = new Vector2I(TILE_SIZE / 2, TILE_SIZE / 2);

            GetParent().CallDeferred("add_child", visualPath);
            visualPathDictionary.Add(enemy, visualPath);
        }
    }

    private void MoveEnemies()
    {
        reservedCells.Clear();

        foreach (var enemy in enemies)
        {
            MoveEnemy(enemy);
        }
    }


    private void MoveEnemy(Rat enemy)
    {
        // Pega o caminho entre o inimigo e o alvo
        pathToPlayer = pathfindingGrid.GetPointPath(
            (Vector2I)(enemy.GlobalPosition / TILE_SIZE),
            (Vector2I)(fighter.GlobalPosition / TILE_SIZE)
        );

        // Seta o caminho até o jogador para ser visualizado
        visualPathDictionary[enemy].Points = pathToPlayer;

        // Move o inimigo para o proximo tile de acordo com seu raio de movimento        
        if (enemy.characterComponent.characterResource != null)
        {
            var movementRadius = enemy.characterComponent.characterResource.MovementRadius;

            if ((pathToPlayer.Count() - movementRadius) == 1)
            {
                movementRadius = 1;
            }

            pathToPlayer = pathToPlayer
                .Skip(movementRadius)
                .ToArray();
        }

        // Aponta para a proxima posicao que o jogador pode se mover
        if (pathToPlayer.Count() > 1)
        {
            var goToPosition = pathToPlayer[0] + new Vector2I(TILE_SIZE / 2, TILE_SIZE / 2);

            pathfindingGrid.SetPointSolid((Vector2I)(enemy.GlobalPosition / TILE_SIZE), false);
            pathfindingGrid.SetPointSolid((Vector2I)(goToPosition / TILE_SIZE), true);

            enemy.GlobalPosition = goToPosition;

            visualPathDictionary[enemy].Points = pathToPlayer;

        }
    }
}
