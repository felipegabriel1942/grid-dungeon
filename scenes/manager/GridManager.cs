using Godot;

namespace Game.Manager;

public partial class GridManager : Node
{

	[Export]
	private TileMapLayer baseTerrainTileMapLayer;

	[Export]
	private TileMapLayer highlightTileMapLayer;

	public override void _Ready()
	{
		GD.Print(baseTerrainTileMapLayer.GetChildren().Count);
	}

	public Vector2I GetMouseGridCellPosition()
	{
		var mousePosition = highlightTileMapLayer.GetGlobalMousePosition();
		var gridPosition = mousePosition / 16;
		gridPosition = gridPosition.Floor();
		return new Vector2I((int) gridPosition.X, (int) gridPosition.Y);
	}

	public bool TileHasCustomData(Vector2I tilePosition, string dataName)
	{


		return false;
	}

}
