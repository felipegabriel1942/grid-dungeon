using Game.Manager;
using Godot;
using System;

namespace Game;

public partial class Main : Node
{

	private GridManager gridManager;

	public override void _Ready()
	{
		gridManager = GetNode<GridManager>("GridManager");
	}

	public override void _Process(double delta)
	{
		var mouseGridPosition = gridManager.GetMouseGridCellPosition();
		GD.Print(mouseGridPosition);
	}


}
