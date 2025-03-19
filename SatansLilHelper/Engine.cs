using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Content;
using SatansLilHelper.Exceptions;
using SatansLilHelper.InputHandlers;
using SatansLilHelper.Utils;

namespace SatansLilHelper;

public class Engine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IInputHandler _inputHandler;
    private Dictionary<TextureID, Texture2D> _textureMap;
    private Dictionary<EffectID, SoundEffect> _effectMap;
    private Dictionary<MusicID, Song> _songMap;
    private Dictionary<FontID, SpriteFont> _fontMap;
    private List<Keys> _keyList;

    public Engine()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _textureMap = [];
        _effectMap = [];
        _songMap = [];
        _fontMap = [];
        _keyList = [];
        _inputHandler = new GameInputHandler();
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.ApplyChanges();

        Window.Title = GameStrings.title_game;
        Window.KeyDown += new EventHandler<InputKeyEventArgs>(HandleKeyDown);
        Window.KeyUp += new EventHandler<InputKeyEventArgs>(HandleKeyUp);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _fontMap.Add(FontID.Status, Content.Load<SpriteFont>(FilePaths.StatusFont));

        _textureMap.Add(TextureID.ForestFloor, Content.Load<Texture2D>(FilePaths.ForestFloor));
        _textureMap.Add(TextureID.ForestWall, Content.Load<Texture2D>(FilePaths.ForestWall));
        _textureMap.Add(TextureID.Player, Content.Load<Texture2D>(FilePaths.Player));

        // TODO: use this.Content to load your game content here
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        _spriteBatch.Begin();
        _inputHandler.Draw(_spriteBatch, _textureMap, _effectMap, _songMap, _fontMap);
        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

#nullable enable
    public void HandleKeyDown(object? sender, InputKeyEventArgs eventArgs)
    {
        if (_keyList.Contains(eventArgs.Key))
            return;
        _keyList.Add(eventArgs.Key);
        try
        {
            _inputHandler.HandleKey(eventArgs.Key);
        }
        catch (GameExitException)
        {
            Exit();
        }
    }

    public void HandleKeyUp(object? sender, InputKeyEventArgs eventArgs)
    {
        _keyList.Remove(eventArgs.Key);
    }
}
