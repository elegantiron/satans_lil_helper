using Microsoft.Xna.Framework;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils.GameMaps;

internal class ForestMap(Point mapSize, IRandom rng, Point screenSize, bool makePlayer = false)
    : BaseMap(mapSize, rng, screenSize, makePlayer)
{
    public override void GenerateMap(Point size)
    {
        int[,] tempMap = new int[mapSize.X, mapSize.Y];
        for (int i = 0; i < mapSize.X; i++)
        {
            for (int j = 0; j < mapSize.Y; j++)
            {
                tempMap[i, j] = rng.NextDouble() < 0.33f ? 1 : 0;
            }
        }

        for (int _ = 0; _ < 3; _++)
        {
            int[,] newMap = tempMap;
            for (int i = 0; i < mapSize.X; i++)
            {
                for (int j = 0; j < mapSize.Y; j++)
                {
                    int neighbors = GetNeighbors(new Point(i, j), tempMap);
                    if (neighbors < 4)
                        newMap[i, j] = 0;
                    else if (neighbors > 4)
                        newMap[i, j] = 1;
                }
            }
            tempMap = newMap;
        }
        for (int i = 0; i < mapSize.X; i++)
        {
            for (int j = 0; j < mapSize.Y; j++)
            {
                Tile tile;
                if (tempMap[i, j] == 1)
                    tile = new Tile(TextureID.ForestWall, false, false);
                else
                    tile = new Tile(TextureID.ForestFloor, true, true);
                tiles[i, j] = tile;
            }
        }
    }

    private static int GetNeighbors(Point target, int[,] map)
    {
        int neighbors = map[target.X, target.Y];
        Point test = new(target.X, target.Y);
        foreach ((int X, int Y) in Dicts.NeighborDirections)
        {
            test.X = target.X + X;
            test.Y = target.Y + Y;
            if (
                test.X < 0
                || test.Y < 0
                || test.X >= map.GetLength(0)
                || test.Y >= map.GetLength(1)
            )
                neighbors++;
            else
                neighbors += map[test.X, test.Y];
        }
        return neighbors;
    }

    public override void SpawnEntities() { }
}
