using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SatansLilHelper.Utils;

public interface ICellGrid
{
    bool IsPassable(Point tile);
    bool PassesLight(Point tile);
    void SetLight(Point tile, float distanceSquared);
    void GenerateMap(Random rng, Point size);

    int XDim { get; }
    int YDim { get; }
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
        Dictionary<MusicID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    );
}

public interface IInputHandler : IDrawable
{
    IInputHandler HandleKey(Keys key);
}
