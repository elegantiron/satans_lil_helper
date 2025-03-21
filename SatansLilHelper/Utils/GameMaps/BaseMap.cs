using System;
using System.Collections.Generic;
using System.Diagnostics;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Components;
using SatansLilHelper.Entities;

namespace SatansLilHelper.Utils.GameMaps;

public abstract class BaseMap : ICellGrid, IDrawable
{
    protected Entity player;
    protected EntityStore registry;
    protected Types.Tile[,] tiles;
    protected Point mapSize;
    protected MersenneTwister rng;
    protected Camera camera;
    public ArchetypeQuery GetActionDelay,
        GetBlockingEntities;

    public BaseMap(Point mapSize, MersenneTwister rng, Point screenSize)
    {
        this.mapSize = mapSize;
        this.rng = rng;
        registry = new EntityStore();

        player = registry.CreateEntity();
        player.AddComponent(new EntityName("Player"));
        Professions.Warrior(player);
        tiles = new Types.Tile[this.mapSize.X, this.mapSize.Y];
#if DEBUG
        Debug.WriteLine(player);
        foreach (ResourceStat stat in player.GetRelations<ResourceStat>())
            Debug.WriteLine($"{stat.Type}: {stat.Cur}/({stat.Basis}+{stat.Growth})");
        foreach (AbilityStat stat in player.GetRelations<AbilityStat>())
            Debug.WriteLine($"{stat.Type}: {stat.Basis}+{stat.Growth}");
#endif
        GenerateMap(this.mapSize);
        camera = new(screenSize, 32);
        camera.SetCenter(0, 0);

        #region Queries
        GetActionDelay = registry.Query<ActionDelay>();
        GetBlockingEntities = registry.Query<Position>().AllTags(Tags.Get<IsBlocking>());
        #endregion Queries
    }

    public Camera Camera
    {
        get { return camera; }
    }

    #region interface implementation


    public Entity Player => player;

    public EntityStore Registry => registry;

    public Types.Tile[,] Tiles => tiles;

    public virtual void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        Point offset = camera.GetOffset();
        Vector2 spriteTarget = Vector2.Zero;
        Vector2 playerPos = new((camera.TileWidth / 2) * 32, (camera.TileHeight / 2) * 32);
        for (int i = offset.X; i < offset.X + camera.TileWidth; i++)
        {
            if (i < 0 || i >= mapSize.X)
                continue;
            spriteTarget.X = (i - offset.X) * 32;
            for (int j = offset.Y; j < offset.Y + camera.TileHeight; j++)
            {
                if (j < 0 || j >= mapSize.Y)
                    continue;
                spriteTarget.Y = (j - offset.Y) * 32;
                spriteBatch.Draw(textureMap[tiles[i, j].Texture], spriteTarget, Color.White);
            }
        }
        spriteBatch.Draw(textureMap[TextureID.Player], playerPos, Color.White);
    }

    public abstract void GenerateMap(Point size);

    #endregion interface implementation
}
