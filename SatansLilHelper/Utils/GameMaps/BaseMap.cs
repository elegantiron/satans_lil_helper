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
        GetBlockingEntities,
        GetDrawableEntities;
    private Vector2 playerPos,
        drawLocation;

    private TextureIndex index;
    private Location entityLocation;

    public BaseMap(Point mapSize, MersenneTwister rng, Point screenSize, bool makePlayer = false)
    {
        this.mapSize = mapSize;
        this.rng = rng;
        registry = new EntityStore();
        if (makePlayer)
        {
            player = registry.CreateEntity();
            player.AddComponent(new EntityName("Player"));
            Professions.Warrior(player);
        }
        tiles = new Types.Tile[this.mapSize.X, this.mapSize.Y];
#if DEBUG
        if (makePlayer)
        {
            Debug.WriteLine(player);
            foreach (ResourceStat stat in player.GetRelations<ResourceStat>())
                Debug.WriteLine($"{stat.Type}: {stat.Cur}/({stat.Basis}+{stat.Growth})");
            foreach (AbilityStat stat in player.GetRelations<AbilityStat>())
                Debug.WriteLine($"{stat.Type}: {stat.Basis}+{stat.Growth}");
        }
#endif
        GenerateMap(this.mapSize);
        if (makePlayer)
        {
            PlaceEntity(player);
        }
        camera = new(screenSize, 32);
        Location playerLoc = player.GetComponent<Location>();
        camera.SetCenter(playerLoc.X, playerLoc.Y);
        playerPos = new((camera.TileWidth / 2) * 32, (camera.TileHeight / 2) * 32);

        drawLocation = Vector2.Zero;

        #region Queries
        GetActionDelay = registry.Query<ActionDelay>();
        GetBlockingEntities = registry.Query<Location>().AllTags(Tags.Get<IsBlocking>());
        GetDrawableEntities = registry
            .Query()
            .AllComponents(ComponentTypes.Get<Location, TextureIndex>())
            .WithoutAnyTags(Tags.Get<IsInvisible, IsPlayer>());
        #endregion Queries
    }

    public static void PlaceEntity(Entity entity, int X, int Y)
    {
        entity.AddComponent(new Location(X, Y));
    }

    public void PlaceEntity(Entity entity)
    {
        bool chosen = false;
        while (!chosen)
        {
            int X = rng.Next(1, mapSize.X);
            int Y = rng.Next(1, mapSize.Y);
            if (tiles[X, Y].Walkable)
            {
                PlaceEntity(entity, X, Y);
                chosen = true;
            }
        }
    }

    public static void PlaceEntity(Entity entity, (int X, int Y) position)
    {
        PlaceEntity(entity, position.X, position.Y);
    }

    public Camera Camera
    {
        get { return camera; }
    }

    public abstract void SpawnEntities();

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
        foreach (Entity entity in GetDrawableEntities.Entities)
        {
            index = entity.GetComponent<TextureIndex>();
            entityLocation = entity.GetComponent<Location>();
            drawLocation.X = (entityLocation.X - offset.X) * 32;
            drawLocation.Y = (entityLocation.Y - offset.Y) * 32;
            spriteBatch.Draw(textureMap[index.Index], drawLocation, Color.White);
        }
    }

    public abstract void GenerateMap(Point size);

    #endregion interface implementation
}
