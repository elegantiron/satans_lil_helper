using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Entities;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils.GameMaps;

internal abstract class BaseMap : ICellGrid, Interfaces.IDrawable
{
    //public fields
    public ArchetypeQuery GetActionDelay,
        GetBlockingEntities,
        GetDrawableEntities,
        GetActors;

    // protected fields
    protected Entity player;
    protected EntityStore registry;
    protected Tile[,] tiles;
    protected Point mapSize;
    protected IRandom rng;
    protected Camera camera;
    protected InitiativeTracker _initiativeTracker;

    // private fields
    private Vector2 playerPos,
        drawLocation;
    private TextureIndex index;
    private Location entityLocation;

    public BaseMap(Point mapSize, IRandom rng, Point screenSize, bool makePlayer = false)
    {
        this.mapSize = mapSize;
        this.rng = rng;
        registry = new EntityStore();
        if (makePlayer)
        {
            player = registry.CreateEntity();
            player.AddComponent(new EntityName("Player"));
            Professions.Warrior(player, rng);
        }
        tiles = new Tile[this.mapSize.X, this.mapSize.Y];
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
        GetBlockingEntities = registry.Query<Location>().AllTags(Tags.Get<Blocking>());
        GetDrawableEntities = registry
            .Query()
            .AllComponents(ComponentTypes.Get<Location, TextureIndex>())
            .WithoutAnyTags(Tags.Get<Invisible, Player>());
        GetActors = registry.Query<Location>().AllTags(Tags.Get<Actor>());
        #endregion Queries

        _initiativeTracker = new(GetActors, rng);
    }

    public void PlaceEntity(Entity entity, int X, int Y)
    {
        entity.AddComponent(new Location(X, Y));
    }

    public void PlaceEntity(Entity entity)
    {
        bool chosen = false;
        while (!chosen)
        {
            int X = (int)rng.Next(1, (uint)mapSize.X);
            int Y = (int)rng.Next(1, (uint)mapSize.Y);
            if (tiles[X, Y].Walkable)
            {
                PlaceEntity(entity, X, Y);
                chosen = true;
            }
        }
    }

    public void PlaceEntity(Entity entity, (int X, int Y) position)
    {
        PlaceEntity(entity, position.X, position.Y);
    }

    public void PlaceEntity(Entity entity, Point position)
    {
        PlaceEntity(entity, position.X, position.Y);
    }

    public void RewindInitiative()
    {
        _initiativeTracker.Rewind();
    }

    public Camera Camera
    {
        get { return camera; }
    }

    public bool IsPlayerNext
    {
        get { return _initiativeTracker.IsPlayerNext; }
    }

    public Entity GetNextActor()
    {
        return _initiativeTracker.GetNextActor();
    }

    public abstract void SpawnEntities();

    public Entity Player => player;

    public EntityStore Registry => registry;

    public Tile[,] Tiles => tiles;

    public virtual void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        DrawTiles(spriteBatch, textureMap);
        spriteBatch.Draw(textureMap[TextureID.Player], playerPos, Color.White);
        DrawEntities(spriteBatch, textureMap);
    }

    public abstract void GenerateMap(Point size);

    protected void DrawTiles(SpriteBatch spriteBatch, Dictionary<TextureID, Texture2D> textureMap)
    {
        Point offset = camera.GetOffset();
        Vector2 spriteTarget = Vector2.Zero;

        for (int i = offset.X; i < offset.X + camera.TileWidth + 1; i++)
        {
            if (i < 0 || i >= mapSize.X)
                continue;
            spriteTarget.X = (i - offset.X) * 32;
            for (int j = offset.Y; j < offset.Y + camera.TileHeight + 1; j++)
            {
                if (j < 0 || j >= mapSize.Y)
                    continue;
                if (!tiles[i, j].Explored)
                    continue;
                spriteTarget.Y = (j - offset.Y) * 32;
                spriteBatch.Draw(
                    textureMap[tiles[i, j].Texture],
                    spriteTarget,
                    tiles[i, j].Visible ? Colors.White : Colors.HiddenTile
                );
            }
        }
    }

    protected void DrawEntities(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap
    )
    {
        Point offset = camera.GetOffset();
        foreach (Entity entity in GetDrawableEntities.Entities)
        {
            index = entity.GetComponent<TextureIndex>();
            entityLocation = entity.GetComponent<Location>();
            drawLocation.X = (entityLocation.X - offset.X) * 32;
            drawLocation.Y = (entityLocation.Y - offset.Y) * 32;
            spriteBatch.Draw(
                textureMap[index.Index],
                drawLocation,
                entity.Tags.Has<Alive>() ? Constants.Colors.LiveActor : Constants.Colors.DeadActor
            );
            if (Settings.Default.ShowHealthBars)
                if (entity.TryGetRelation(AbilityID.Health, out AbilityStat entHealth))
                {
                    int totalHealth = EntityCalcs.GetStat(entity, AbilityID.Health);
                    Rectangle healthRect = new(
                        (int)drawLocation.X + 4,
                        (int)drawLocation.Y + 4,
                        24,
                        4
                    );
                    if (entHealth.Cur < totalHealth)
                    {
                        spriteBatch.Draw(
                            textureMap[TextureID.WhitePixel],
                            healthRect,
                            Constants.Colors.Red
                        );
                        healthRect.Width = (int)(24.0m * ((decimal)entHealth.Cur / totalHealth));
                        spriteBatch.Draw(
                            textureMap[TextureID.WhitePixel],
                            healthRect,
                            Constants.Colors.Green
                        );
                    }
                }
        }
    }

    public void UpdatePlayerVision()
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i, j].Visible = false;
            }
        }
        int viewRadius = EntityCalcs.GetStat(Player, AbilityID.Vision);
        int lightRadius = EntityCalcs.GetStat(Player, AbilityID.LightRadius);
        float playerView = (float)Math.Min(viewRadius, lightRadius);
        Location playerLoc = Player.GetComponent<Location>();
        ShadowCast.ComputeVisibility(this, new Point(playerLoc.X, playerLoc.Y), playerView);
    }

    public IEnumerable<(int, int)> GetNeighbors((int, int) tile)
    {
        List<(int, int)> neighbors = [];
        foreach ((int X, int Y) in Dicts.NeighborDirections)
        {
            int nX = tile.Item1 + X;
            int nY = tile.Item2 + Y;
            if (
                nX >= 0
                && nX < (this as ICellGrid).XDim
                && nY >= 0
                && nY < (this as ICellGrid).YDim
            )
                neighbors.Add((nX, nY));
        }
        return neighbors;
    }

    public int GetMovementCost((int X, int Y) tile)
    {
        return tiles[tile.X, tile.Y].MovementCost;
    }
}
