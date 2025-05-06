using System;
using System.Collections.Generic;
using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SatansLilHelper.Constants;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Extensions;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class Megamap : DrawableGameComponent
{
    private SpriteBatch? batch;
    private Camera? camera;
    private Dictionary<TextureID, Texture2D> textures;
    private Vector2 zoom;

    public Megamap(Game game)
        : base(game)
    {
        textures = [];
        zoom = Vector2.One;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void OnEnabledChanged(object _, EventArgs _1)
    {
        if (Game is not Engine engine || camera is null)
            return;
        if (Enabled)
        {
            Location playerLoc = engine.World.Player.GetComponent<Location>();
            camera.XY = new Vector2(playerLoc.X * 32, playerLoc.Y * 32);
            zoom.X = 0.9f;
            zoom.Y = 0.9f;
            camera.Scale = zoom;
        }
    }

    protected override void LoadContent()
    {
        batch = new(Game.GraphicsDevice);

        IVirtualViewport viewport = new DefaultViewport(Game.GraphicsDevice, Game.Window);
        camera = new(viewport) { Scale = zoom };

        textures.Add(TextureID.ForestFloor, Game.Content.Load<Texture2D>(FilePaths.ForestFloor));
        textures.Add(TextureID.ForestWall, Game.Content.Load<Texture2D>(FilePaths.ForestWall));
    }

    public override void Draw(GameTime gameTime)
    {
        if (camera is null || batch is null || Game is not Engine engine)
            return;
        base.Draw(gameTime);
        camera.SetViewport();
        batch.Begin(transformMatrix: camera.View, samplerState: SamplerState.PointClamp);
        DrawTiles(engine);
        batch.End();
        camera.ResetViewport();
    }

    private void DrawTiles(Engine engine)
    {
        if (batch is null)
            return;
        for (int i = 0; i < engine.World.CurrentMap.Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < engine.World.CurrentMap.Tiles.GetLength(1); j++)
            {
                if (!engine.World.CurrentMap.Tiles[i, j].Explored)
                {
                    //continue;
                }
                batch.Draw(
                    textures[engine.World.CurrentMap.Tiles[i, j].Texture],
                    new Vector2(i * 32, j * 32),
                    engine.World.CurrentMap.Tiles[i, j].Visible ? Colors.White : Colors.DeadActor,
                    new Vector2(16)
                );
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is not Engine engine || camera is null)
            return;
        if (Settings.Default.Movement.IsPressedAvailable(engine.Handler))
        {
            HandleMovement();
            return;
        }
        if (
            Settings.Default.Map.TryConsumePressed(engine.Handler)
            || Settings.Default.Cancel.TryConsumePressed(engine.Handler)
        )
        {
            Enabled = Visible = false;
            engine.GameScreen.Enabled =
                engine.GameScreen.Visible =
                engine.StatusScreen.Enabled =
                engine.StatusScreen.Visible =
                    true;
        }
        if (
            engine.Handler.TryConsumePressed(Keys.Space)
            || Settings.Default.MiniMap.TryConsumePressed(engine.Handler)
        )
            OnEnabledChanged(new object(), new EventArgs());
        if (Settings.Default.Increase.TryConsumePressed(engine.Handler))
            camera.Scale += new Vector2(0.1f);
        if (Settings.Default.Decrease.TryConsumePressed(engine.Handler))
            camera.Scale -= new Vector2(0.1f);
    }

    private void HandleMovement()
    {
        if (Game is not Engine engine || camera is null)
            return;
        if (
            Settings.Default.MoveUp.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveUpRight.IsPressedAvailable(engine.Handler)
            || Settings.Default.MoveUpLeft.IsPressedAvailable(engine.Handler)
        )
            camera.Y -= 32 / camera.Scale.X;
        if (
            Settings.Default.MoveUpLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownLeft.IsPressedAvailable(engine.Handler)
        )
            camera.X -= 32 / camera.Scale.X;
        if (
            Settings.Default.MoveUpRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownRight.IsPressedAvailable(engine.Handler)
        )
            camera.X += 32 / camera.Scale.X;
        if (
            Settings.Default.MoveDown.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownRight.TryConsumePressed(engine.Handler)
        )
            camera.Y += 32 / camera.Scale.X;
    }
}
