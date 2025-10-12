using Godot;
using Game.Autoload;

namespace Game.UI;

public partial class GameUi : CanvasLayer
{

    [Export]
    private Button endPlayerTurnButton;

    [Export]
    private Button showEnemyMovementRangeButton;

    [Export]
    private Button playerAttackButton;

    public override void _Ready()
    {
        InitEventEmitters();
    }

    private void InitEventEmitters()
    {
        endPlayerTurnButton.Pressed += GameEvents.EmitPlayerTurnEnded;
        showEnemyMovementRangeButton.Pressed += GameEvents.EmitShowEnemyMovementRange;
        playerAttackButton.Pressed += GameEvents.EmitPlayerAttack;
    }
}
