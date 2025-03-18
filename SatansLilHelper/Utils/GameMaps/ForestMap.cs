using System;
using Microsoft.Xna.Framework;

namespace SatansLilHelper.Utils.GameMaps;

public class ForestMap(Point mapSize, Random rng, Point screenSize)
    : BaseMap(mapSize, rng, screenSize)
{
    public override void GenerateMap(Random rng, Point size)
    {
        int[,] tempMap = new int[mapSize.X, mapSize.Y];
        for (int i = 0; i < mapSize.X; i++)
        {
            for (int j = 0; j < mapSize.Y; j++)
            {
                tempMap[i, j] = rng.NextDouble() < 0.35f ? 1 : 0;
            }
        }

        for (int iters = 0; iters < 5; iters++)
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
                Types.Tile tile;
                if (tempMap[i, j] == 0)
                    tile = new Types.Tile(TextureID.ForestWall, false, false);
                else
                    tile = new Types.Tile(TextureID.ForestFloor, true, true);
                tiles[i, j] = tile;
            }
        }
    }

    private static int GetNeighbors(Point target, int[,] map)
    {
        int neighbors = map[target.X, target.Y];
        Point test = new(target.X, target.Y);
        foreach ((int X, int Y) in Constants.NeighborDirections)
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
}
