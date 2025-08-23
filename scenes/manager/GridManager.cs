using Godot;
using System;

namespace Game.Manager;

public partial class GridManager : Node
{

    [Export]
    private TileMapLayer baseTerrainTileMapLayer;

    public override void _Ready()
    {
        GD.Print(baseTerrainTileMapLayer.GetChildren().Count);

        
    }


    public bool TileHasCustomData(Vector2I tilePosition, string dataName)
    {


        return false;
    }

}
