using System.Collections.Generic;
using Game.Character;
using Game.UI;
using Godot;

public partial class EnemyManager : Node
{

    [Export]
    private TileMapLayer tileMapLayer;

    [Export]
    private Fighter fighter;

    [Export]
    private Node2D enemy;

    [Export]
    private Line2D visualEnemyPath;

    [Export]
    private GameUi gameUi;

    private List<AStarGrid2D> pathfindingGrid = new();

    private List<Node2D> pathToPlayer = new();

    public override void _Ready()
    {
        gameUi.MovingEnemy += MoveEnemy;
    }


    private void MoveEnemy() {
        GD.Print("Recebendo o sinal, capitão!!!");
    }

}
