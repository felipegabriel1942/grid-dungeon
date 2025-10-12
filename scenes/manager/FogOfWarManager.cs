
using System.Linq;
using Game.Autoload;
using Game.Character;
using Game.Manager;
using Godot;

public partial class FogOfWarManager : Node
{

    [Export]
    private TileMapLayer fogOfWarTileMapLayer;

    [Export]
    private GridManager gridManager;

    [Export]
    private Fighter fighter;

    public override void _Ready()
    {
        fogOfWarTileMapLayer.Visible = true;
        GameEvents.Instance.PlayerMoved += DispelFogOnPlayerPosition;
        Callable.From(DispelFogOnPlayerPosition).CallDeferred();
    }

    private void DispelFogOnPlayerPosition()
    {

        gridManager.UpdateViewedTiles(fighter.characterComponent);

        for (int i = 0; i < gridManager.viewedTiles.Count; i++)
        {
            var cell = gridManager.viewedTiles.ElementAt(i);
            fogOfWarTileMapLayer.SetCell(cell, 2, Vector2I.Zero);
        }
    }
}
