using Godot;

namespace Game.Character;

public partial class CharacterGhost : Node2D
{

    public void SetInvalid()
    {
        Modulate = new Color(1, 0, 0, (float) 0.6);
    }

    public void SetValid()
    {
        Modulate = new Color(1, 1, 1, (float) 0.6) ;
    }
}
