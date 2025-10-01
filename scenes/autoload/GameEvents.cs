using Game.Character;
using Godot;

namespace Game.Autoload;

public partial class GameEvents : Node
{

    public static GameEvents Instance { get; private set; }

    [Signal]
    public delegate void EnemyMovedEventHandler(Rat rat);

    [Signal]
    public delegate void EnemyTurnEndedEventHandler();

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
}
