using System;
using System.Collections.Generic;
using Apos.Camera;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Entities;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils.GameMaps;

internal abstract class BaseMap : ICellGrid, Interfaces.IDrawable
{
    //public fields
    public ArchetypeQuery GetActionDelay,
        GetBlockingEntities,
        GetActors;

    public ArchetypeQuery<Location, TextureIndex> GetDrawableEntities;

    // protected fields
    protected Entity player;
    protected EntityStore registry;
    protected Tile[,] tiles;
    protected Point mapSize;
    protected IRandom rng;
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

        drawLocation = Vector2.Zero;

        #region Queries
        GetActionDelay = registry.Query<ActionDelay>();
        GetBlockingEntities = registry.Query<Location>().AllTags(Tags.Get<Blocking>());
        GetDrawableEntities = registry
            .Query<Location, TextureIndex>()
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

    public bool IsPlayerNext => _initiativeTracker.IsPlayerNext;

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
        Camera camera,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        camera.SetViewport();
        spriteBatch.Begin(transformMatrix: camera.View);
        DrawTiles(spriteBatch, textureMap);
        DrawEntities(spriteBatch, textureMap);
        Location playerLoc = player.GetComponent<Location>();
        spriteBatch.Draw(
            textureMap[TextureID.Player],
            new Vector2(playerLoc.X * 32, playerLoc.Y * 32),
            Color.White
        );
        spriteBatch.End();
        camera.ResetViewport();
    }

    public abstract void GenerateMap(Point size);

    protected void DrawTiles(SpriteBatch spriteBatch, Dictionary<TextureID, Texture2D> textureMap)
    {
        Vector2 spriteTarget = Vector2.Zero;

        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            spriteTarget.X = i * 32;
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if (!tiles[i, j].Explored)
                    continue;
                spriteTarget.Y = j * 32;
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
        foreach (Entity entity in GetDrawableEntities.Entities)
        {
            index = entity.GetComponent<TextureIndex>();
            entityLocation = entity.GetComponent<Location>();
            drawLocation.X = entityLocation.X * 32;
            drawLocation.Y = entityLocation.Y * 32;
            spriteBatch.Draw(
                textureMap[index.Index],
                drawLocation,
                entity.Tags.Has<Alive>() ? Colors.LiveActor : Colors.DeadActor
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
                        spriteBatch.Draw(textureMap[TextureID.WhitePixel], healthRect, Colors.Red);
                        healthRect.Width = (int)(24.0m * ((decimal)entHealth.Cur / totalHealth));
                        spriteBatch.Draw(
                            textureMap[TextureID.WhitePixel],
                            healthRect,
                            Colors.Green
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
        float playerView = Math.Min(viewRadius, lightRadius);
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
