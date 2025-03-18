using System;
using System.Collections.Generic;
using System.Diagnostics;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Entities;

namespace SatansLilHelper.Utils.GameMaps;

public abstract class BaseMap : ICellGrid, IDrawable
{
    protected Entity player;
    protected EntityStore registry;
    protected Tile[,] tiles;
    protected Point mapSize;
    protected Random rng;

    public BaseMap(Point mapSize, Random rng)
    {
        this.mapSize = mapSize;
        this.rng = rng;
        registry = new EntityStore();
        player = registry.CreateEntity();
        player.AddComponent(new EntityName("player"));
        Professions.Warrior(player);
#if DEBUG
        Debug.WriteLine(player);
#endif
        GenerateMap(this.rng, this.mapSize);
    }

    #region interface implementation
    public int XDim => mapSize.X;

    public int YDim => mapSize.Y;

    public Entity Player => player;

    public EntityStore Registry => registry;

    public Tile[,] Tiles => tiles;

    public abstract void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<MusicID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    );
    public abstract void GenerateMap(Random rng, Point size);

    public bool IsPassable(Point tile)
    {
        return tiles[tile.X, tile.Y].Walkable;
    }

    public bool PassesLight(Point tile)
    {
        return tiles[tile.X, tile.Y].PassesLight;
    }

    public void SetLight(Point tile, float distanceSquared)
    {
        tiles[tile.X, tile.Y].Visible = true;
        tiles[tile.X, tile.Y].Explored = true;
        tiles[tile.X, tile.Y].LightDistance = distanceSquared;
    }
    #endregion interface implementation
}
