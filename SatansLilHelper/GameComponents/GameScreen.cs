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
    private GameWorld? _gameWorld;
    private MessageLog _messageLog;
    private PlayerTurn? _playerTurn;
    private SpriteBatch? _spriteBatch;
    private Camera? _camera;
    private Dictionary<TextureID, Texture2D> _textures;

    public GameScreen(Game game)
        : base(game)
    {
        _textures = [];
        _messageLog = new();
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is Engine engine)
        {
            engine.Handler.Update(gameTime);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        if (_gameWorld is null || _playerTurn is null || _spriteBatch is null || _camera is null)
        {
            if (_gameWorld is null)
                Debug.WriteLine("gameworld");
            if (_playerTurn is null)
                Debug.WriteLine("player turn");
            if (_spriteBatch is null)
                Debug.WriteLine("sprite batch");
            if (_camera is null)
                Debug.WriteLine("camera");
            Game.GraphicsDevice.Clear(Colors.CornflowerBlue);
            return;
        }
        Game.GraphicsDevice.Clear(Colors.Black);
        _spriteBatch.Begin(transformMatrix: _camera.View);
        for (int i = 0; i < _gameWorld.CurrentMap.Tiles.GetLength(0); i++)
        {
            for (int j = 0; j < _gameWorld.CurrentMap.Tiles.GetLength(1); j++)
            {
                _spriteBatch.Draw(
                    _textures[_gameWorld.CurrentMap.Tiles[i, j].Texture],
                    new Vector2(i * 32, j * 32),
                    Colors.White
                );
            }
        }
        var query = _gameWorld.CurrentMap.Registry.Query<Location, TextureIndex>();
        //.AllTags(Tags.Get<Visible>());
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
        _spriteBatch.End();
    }

    public override void Initialize()
    {
        _messageLog = new();
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

    public void NewGame()
    {
        _gameWorld = new(new MersenneTwister(), new Point(100, 100));
        _playerTurn = new(_gameWorld.CurrentMap.Player);
        Location playerLoc = _gameWorld.CurrentMap.Player.GetComponent<Location>();
        if (_camera != null)
        {
            _camera.XY = new Vector2(playerLoc.X * 32, playerLoc.Y * 32);
        }
    }
}
