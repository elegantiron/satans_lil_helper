using System.Collections.Generic;
using Apos.Camera;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Input;
using SatansLilHelper.Constants;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Extensions;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class GameScreen : DrawableGameComponent
{
    private SpriteBatch? _spriteBatch;
    private Camera? _camera;
    private Dictionary<TextureID, Texture2D> _textures;

    public GameScreen(Game game)
        : base(game)
    {
        _textures = [];
    }

    public override void Update(GameTime gameTime)
    {
        if (_camera is null || Game is not Engine engine)
            return;

        Location playerLoc = engine.World.CurrentMap.Player.GetComponent<Location>();
        _camera.XY = new Vector2(playerLoc.X * 32, playerLoc.Y * 32);
        engine.World.CurrentMap.UpdatePlayerVision();
        if (Settings.Default.Cancel.TryConsumePressed(engine.Handler))
        {
            Enabled = false;
            engine.PauseMenu.Enabled = true;
            engine.PauseMenu.Visible = true;
        }

        if (engine.World.CurrentMap.IsPlayerNext)
            ProcessPlayerTurn();
        else
            ProcessNPCTurns();
    }

    public override void Draw(GameTime gameTime)
    {
        if (_spriteBatch is null || _camera is null || Game is not Engine engine)
            return;
        _camera.SetViewport();
        _spriteBatch.Begin(transformMatrix: _camera.View);
        DrawTiles(engine);
        DrawEntities(engine);
        _spriteBatch.End();
        _camera.ResetViewport();
    }

    private void DrawEntities(Engine engine)
    {
        if (_spriteBatch is null)
            return;

        var query = engine
            .World.CurrentMap.Registry.Query<Location, TextureIndex>()
            .AllTags(Tags.Get<Visible>());
        foreach (Entity ent in query.Entities)
        {
            Location entLoc = ent.GetComponent<Location>();
            if (!engine.World.CurrentMap.Tiles[entLoc.X, entLoc.Y].Visible)
                continue;
            TextureIndex entIndex = ent.GetComponent<TextureIndex>();
            _spriteBatch.Draw(
                _textures[entIndex.Index],
                new Vector2(entLoc.X * 32, entLoc.Y * 32),
                Colors.White,
                new Vector2(16)
            );
        }
    }

    private void DrawTiles(Engine engine)
    {
        if (_spriteBatch is null)
            return;
        for (int i = 0; i < engine.World.CurrentMap.Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < engine.World.CurrentMap.Tiles.GetLength(1); j++)
            {
                if (!engine.World.CurrentMap.Tiles[i, j].Explored)
                {
                    //continue;
                }
                _spriteBatch.Draw(
                    _textures[engine.World.CurrentMap.Tiles[i, j].Texture],
                    new Vector2(i * 32, j * 32),
                    engine.World.CurrentMap.Tiles[i, j].Visible ? Colors.White : Colors.DeadActor,
                    new Vector2(16)
                );
            }
        }
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new(Game.GraphicsDevice);

        _textures.Add(TextureID.ForestFloor, Game.Content.Load<Texture2D>(FilePaths.ForestFloor));
        _textures.Add(TextureID.ForestWall, Game.Content.Load<Texture2D>(FilePaths.ForestWall));
        _textures.Add(TextureID.Player, Game.Content.Load<Texture2D>(FilePaths.Player));
        IVirtualViewport defaultViewport = new DefaultViewport(Game.GraphicsDevice, Game.Window);
        _camera = new(defaultViewport);
        base.LoadContent();
    }

    private void ProcessPlayerTurn()
    {
        if (Game is not Engine engine)
            return;
        if (Settings.Default.Movement.IsPressedAvailable(engine.Handler))
            HandleMovement();
        else if (Settings.Default.MiniMap.TryConsumePressed(engine.Handler))
            engine.MiniMap.Visible = engine.MiniMap.Enabled = !engine.MiniMap.Enabled;
        else if (Settings.Default.Map.TryConsumePressed(engine.Handler))
        {
            Enabled = Visible = engine.StatusScreen.Enabled = engine.StatusScreen.Visible = false;
            engine.MegaMap.Enabled = engine.MegaMap.Visible = true;
        }
        else if (Settings.Default.Skills.TryConsumePressed(engine.Handler))
        {
            // Show skills selector here
            Enabled = false;
        }
        else if (Settings.Default.Inventory.TryConsumePressed(engine.Handler))
        {
            Enabled = false;
            engine.Inventory.Enabled = true;
            engine.Inventory.Visible = true;
        }
    }

    private void ProcessNPCTurns() { }

    private void HandleMovement()
    {
        if (Game is not Engine engine)
            return;
        int dx = 0,
            dy = 0;
        if (
            Settings.Default.MoveUp.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveUpLeft.IsPressedAvailable(engine.Handler)
            || Settings.Default.MoveUpRight.IsPressedAvailable(engine.Handler)
        )
            dy = -1;
        if (
            Settings.Default.MoveUpRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownRight.IsPressedAvailable(engine.Handler)
        )
            dx = 1;
        if (
            Settings.Default.MoveDown.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownLeft.IsPressedAvailable(engine.Handler)
        )
            dy = 1;
        if (
            Settings.Default.MoveLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveUpLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownLeft.TryConsumePressed(engine.Handler)
        )
            dx = -1;

        engine.PlayerTurn.AddAction(
            new Actions.BumpAction(
                engine.World.Player,
                new Point(dx, dy),
                engine.World.CurrentMap,
                engine.World.Generator,
                true
            )
        );
    }
}
