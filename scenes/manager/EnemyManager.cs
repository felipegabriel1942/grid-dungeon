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
    private Line2D visualEnemyPath = null;

    [Export]
    private GameUi gameUi;

    private AStarGrid2D pathfindingGrid = new();

    private Vector2[] pathToPlayer = [];

    public override void _Ready()
    {
        gameUi.MovingEnemy += MoveEnemy;
    }

    private void MoveEnemy()
    {
        
    }

}
