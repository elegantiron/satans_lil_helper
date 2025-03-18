using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Utils;

public interface ICellGrid
{
    public bool IsPassable(Point tile);
    public bool PassesLight(Point tile);
    public void SetLight(Point tile, float distanceSquared);
    public void GenerateMap(Random rng, Point size);

    public int XDim { get; }
    public int YDim { get; }
    public Entity Player { get; }
    public EntityStore Registry { get; }
    public Tile[,] Tiles { get; }
}

public interface IDrawable
{
    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<MusicID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    );
}

public interface IInputHandler : IDrawable
{
    public IInputHandler HandleKey(Keys key);
}
