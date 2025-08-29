using Game.Component;
using Godot;
using System;

namespace Game.Character;

public partial class Fighter : CharacterBody2D
{
    
    [Export]
    public CharacterComponent characterComponent { get; private set; }
}
