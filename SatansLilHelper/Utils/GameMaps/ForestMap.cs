using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Utils.GameMaps;

public class ForestMap(Point mapSize, Random rng) : BaseMap(mapSize, rng)
{
    public override void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<MusicID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    ) { }

    public override void GenerateMap(Random rng, Point size)
    {
        bool[,] tempMap = new bool[mapSize.X, mapSize.Y];
        for (int i = 0; i < mapSize.X; i++)
        {
            for (int j = 0; j < mapSize.Y; j++)
            {
                tempMap[i, j] = rng.NextDouble() < 0.35f;
            }
        }

        for (int iters = 0; iters < 5; iters++)
        {
            bool[,] newMap = tempMap;
            for (int i = 0; i < mapSize.X; i++)
            {
                for (int j = 0; j < mapSize.Y; j++)
                {
                    int neighbors = GetNeighbors(new Point(i, j));
                    if (neighbors < 4)
                        newMap[i, j] = false;
                    else if (neighbors > 4)
                        newMap[i, j] = true;
                }
            }
            tempMap = newMap;
        }
        for (int i = 0; i < mapSize.X; i++)
        {
            for (int j = 0; j < mapSize.Y; j++)
            {
                Tile tile;
                if (tempMap[i, j])
                    tile = new Tile(TextureID.ForestWall, false, false);
                else
                    tile = new Tile(TextureID.ForestFloor, true, true);
                tiles[i, j] = tile;
            }
        }
    }

    private int GetNeighbors(Point target)
    {
        return 0;
    }
}
