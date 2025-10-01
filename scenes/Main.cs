using System.Linq;
using Game.Autoload;
using Game.Character;
using Game.Manager;
using Godot;

namespace Game;

public partial class Main : Node
{

    private EnemyManager enemyManager;
    private Node2D playerParty;

    public override void _Ready()
    {
        GameEvents.Instance.EnemyTurnEnded += OnEnemyTurnEnded;

        playerParty = GetNode<Node2D>("%PlayerParty");
    }

    private void OnEnemyTurnEnded()
    {
        GD.Print("Acabou o turno dos inimigos...");

        foreach (var playerChar in playerParty.GetChildren().Cast<Fighter>())
        {
            playerChar.characterComponent.hasMoved = false;
        }
    }

}
