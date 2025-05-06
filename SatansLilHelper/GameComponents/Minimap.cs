using System.Collections.Generic;
using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SatansLilHelper.Constants;
using SatansLilHelper.Extensions;

namespace SatansLilHelper.GameComponents;

internal class Minimap : DrawableGameComponent
{
    private SpriteBatch? batch;
    private Camera? camera;
    private Dictionary<TextureID, Texture2D> textures;

    public Minimap(Game game)
        : base(game)
    {
        textures = [];
    }

    public override void Initialize()
    {
        batch = new(Game.GraphicsDevice);
        base.Initialize();
    }

    protected override void LoadContent()
    {
        batch = new(Game.GraphicsDevice);
        textures.Add(TextureID.ForestFloor, Game.Content.Load<Texture2D>(FilePaths.ForestFloor));
        textures.Add(TextureID.ForestWall, Game.Content.Load<Texture2D>(FilePaths.ForestWall));

        IVirtualViewport viewport = new SplitViewport(
            Game.GraphicsDevice,
            Game.Window,
            0f,
            0f,
            0.19f,
            0.32f
        );
        camera = new(viewport) { Scale = new Vector2(0.5f) };
    }

    public override void Update(GameTime gameTime)
    {
        if (camera is null || Game is not Engine engine)
            return;
        camera.X = engine.World.CurrentMap.Tiles.GetLength(0) * 16;
        camera.Y = engine.World.CurrentMap.Tiles.GetLength(1) * 16;
        if (DrawOrder <= engine.GameScreen.DrawOrder)
            DrawOrder = engine.GameScreen.DrawOrder + 1;
        camera.Scale = new Vector2(0.07f);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        if (camera is null || batch is null || Game is not Engine engine)
            return;

        Texture2D pixel = new(batch.GraphicsDevice, 1, 1);
        pixel.SetData([Colors.White]);
        camera.SetViewport();
        batch.Begin(transformMatrix: camera.View, samplerState: SamplerState.PointClamp);
        batch.Draw(pixel, new Rectangle(-500, -500, 10280, 11720), Colors.TranslucentBlack);
        for (int i = 0; i < engine.World.CurrentMap.Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < engine.World.CurrentMap.Tiles.GetLength(1); j++)
            {
                if (!engine.World.CurrentMap.Tiles[i, j].Explored)
                {
                    continue;
                }
                batch.Draw(
                    textures[engine.World.CurrentMap.Tiles[i, j].Texture],
                    new Vector2(i * 32, j * 32),
                    Colors.White,
                    new Vector2(16)
                );
            }
        }

        batch.End();
        camera.ResetViewport();
    }
}
