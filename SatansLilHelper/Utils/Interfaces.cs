using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SatansLilHelper.Utils;

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
    Entity Player { get; }
    EntityStore Registry { get; }
    Types.Tile[,] Tiles { get; }
}

public interface IDrawable
{
    void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    );
}

public interface IInputHandler : IDrawable
{
    IInputHandler HandleKey(Keys key);
}

public interface IMessageHandler
{
    void Write(string message);
}
