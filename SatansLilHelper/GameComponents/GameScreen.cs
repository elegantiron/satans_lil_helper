using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apos.Camera;
using FontStashSharp.RichText;
using Friflo.Engine.ECS;
using Friflo.Json.Fliox.Transform;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SatansLilHelper.Constants;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
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
        if (_camera is null)
            return;
        if (Game is Engine engine)
        {
            engine.Handler.Update(gameTime);
            Location playerLoc = engine.World.CurrentMap.Player.GetComponent<Location>();
            _camera.XY = new Vector2(playerLoc.X * 32, playerLoc.Y * 32);
        }
        //Debug.WriteLine(DrawOrder);
    }

    public override void Draw(GameTime gameTime)
    {
        if (_spriteBatch is null || _camera is null)
            return;
        _camera.SetViewport();
        _spriteBatch.Begin(transformMatrix: _camera.View);
        if (Game is Engine engine)
        {
            DrawTiles(engine);
            DrawEntities(engine);
        }
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
            TextureIndex entIndex = ent.GetComponent<TextureIndex>();
            _spriteBatch.Draw(
                _textures[entIndex.Index],
                new Vector2(entLoc.X * 32, entLoc.Y * 32),
                Colors.White
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
                _spriteBatch.Draw(
                    _textures[engine.World.CurrentMap.Tiles[i, j].Texture],
                    new Vector2(i * 32, j * 32),
                    Colors.White
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
}
