using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Types;

namespace SatansLilHelper.Interfaces;

public interface ICellGrid
{
    bool IsPassable(Point tile)
    {
        return Tiles[tile.X, tile.Y].Walkable;
    }
    bool PassesLight(Point tile)
    {
        return Tiles[tile.X, tile.Y].PassesLight;
    }
    void SetLight(Point tile, float distanceSquared)
    {
        Tiles[tile.X, tile.Y].Visible = true;
        Tiles[tile.X, tile.Y].Explored = true;
        Tiles[tile.X, tile.Y].LightDistance = distanceSquared;
    }
    void GenerateMap(Point size);

    int XDim
    {
        get { return Tiles.GetLength(0); }
    }
    int YDim
    {
        get { return Tiles.GetLength(1); }
    }
    IEnumerable<(int, int)> GetNeighbors((int, int) tile);
    int GetMovementCost((int X, int Y) tile);
    Entity Player { get; }
    EntityStore Registry { get; }
    Tile[,] Tiles { get; }
}
