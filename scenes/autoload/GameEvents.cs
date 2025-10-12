using Game.Character;
using Godot;

namespace Game.Autoload;

public partial class GameEvents : Node
{

    public static GameEvents Instance { get; private set; }

    [Signal]
    public delegate void PlayerTurnEndedEventHandler();

    [Signal]
    public delegate void EnemyMovedEventHandler(Rat rat);

    [Signal]
    public delegate void EnemyTurnEndedEventHandler();

    [Signal]
    public delegate void ShowEnemyMovementRangeEventHandler();

    [Signal]
    public delegate void PlayerAttackEventHandler();

    [Signal]
    public delegate void PlayerMovedEventHandler();

    public override void _Notification(int what)
    {
        if (what == NotificationSceneInstantiated)
        {
            Instance = this;
        }
    }

    public static void EmitEnemyMoved(Rat rat)
    {
        Instance.EmitSignal(SignalName.EnemyMoved, rat);
    }

    public static void EmitEnemyTurnEnded()
    {
        Instance.EmitSignal(SignalName.EnemyTurnEnded);
    }

    public static void EmitPlayerTurnEnded()
    {
        Instance.EmitSignal(SignalName.PlayerTurnEnded);
    }

    public static void EmitShowEnemyMovementRange()
    {
        Instance.EmitSignal(SignalName.ShowEnemyMovementRange);
    }

    public static void EmitPlayerAttack()
    {
        Instance.EmitSignal(SignalName.PlayerAttack);
    }

    public static void EmitPlayerMoved()
    {
        Instance.EmitSignal(SignalName.PlayerMoved);
    }
}
