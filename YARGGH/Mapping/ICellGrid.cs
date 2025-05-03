using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace YARGGH.Mapping;

public interface ICellGrid
{
    bool IsPassable(Point tile);
    bool PassesLight(Point tile);

    void SetLight(Point tile, float distanceSquared);

    IEnumerable<Point> GetNeighbors(Point tile);
    int GetMovementCost(Point tile);
    int xDim { get; }
    int yDim { get; }
    Dictionary<Point, Tile> Tiles { get; }
}
