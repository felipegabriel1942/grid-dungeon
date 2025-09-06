using Godot;

namespace Game.UI;

public partial class GameUi : CanvasLayer
{

    [Signal]
    public delegate void MovingEnemyEventHandler();

    [Export]
    private Button moveEnemyButton;

    public override void _Ready()
    {
        moveEnemyButton.Pressed += () =>
        {
            EmitSignal(SignalName.MovingEnemy);
        };
    }

}
