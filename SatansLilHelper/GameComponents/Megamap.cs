using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SatansLilHelper.Constants;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Extensions;

namespace SatansLilHelper.GameComponents;

internal class Megamap : DrawableGameComponent
{
    private SpriteBatch? batch;
    private Camera? camera;
    private Dictionary<TextureID, Texture2D> textures;

    public Megamap(Game game)
        : base(game)
    {
        textures = [];
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
        }
    }

    protected override void LoadContent()
    {
        batch = new(Game.GraphicsDevice);

        IVirtualViewport viewport = new DefaultViewport(Game.GraphicsDevice, Game.Window);
        camera = new(viewport);
    }

    public override void Draw(GameTime gameTime)
    {
        if (camera is null || batch is null || Game is not Engine engine)
            return;
        base.Draw(gameTime);
        camera.SetViewport();
        batch.Begin(transformMatrix: camera.View);
        DrawTiles(engine);
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
        if (engine.Handler.TryConsumePressed(Keys.Up))
            camera.Y -= 32;
        else if (engine.Handler.TryConsumePressed(Keys.Down))
            camera.Y += 32;
        if (engine.Handler.TryConsumePressed(Keys.Right))
            camera.X += 32;
        else if (engine.Handler.TryConsumePressed(Keys.Left))
            camera.X -= 32;
        if (engine.Handler.TryConsumePressed(Keys.M))
        {
            Enabled = Visible = false;
            engine.GameScreen.Enabled =
                engine.GameScreen.Visible =
                engine.StatusScreen.Enabled =
                engine.StatusScreen.Visible =
                    true;
        }
    }
}
