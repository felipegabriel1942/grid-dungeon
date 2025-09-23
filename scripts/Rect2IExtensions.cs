
using Godot;

namespace Game;

public static class Rect2IExtensions
{

    public static Rect2 ToRect2F(this Rect2I rect)
    {
        return new Rect2(rect.Position, rect.Size);
    }
}
