using System;
using Game.Component;
using Godot;

namespace Game.Character;

public partial class Rat : Node2D
{
	[Export]
	public CharacterComponent characterComponent { get; private set; }

	private Label label;

	public override void _Ready()
	{
		label = GetNode<Label>("Label");
		GD.Print(label);
	}

	public void SetLabelValue(string value)
	{
		GD.Print(label);
		label.Text = value;
	}

}
