using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Content.Text;
using SatansLilHelper.Exceptions;
using SatansLilHelper.InputHandlers;
using SatansLilHelper.Utils;

namespace SatansLilHelper;

public class Engine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch SpriteBatch;
    private IInputHandler InputHandler;
    private Dictionary<TextureID, Texture2D> _textureMap;
    private Dictionary<EffectID, SoundEffect> _effectMap;
    private Dictionary<SongID, Song> _songMap;
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
#if RELEASE
        InputHandler = new TitleInputHandler();
#endif
#if DEBUG
        InputHandler = new GameInputHandler();
#endif
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.ApplyChanges();

        Window.Title = GameStrings.GameTitle;
        Window.KeyDown += new EventHandler<InputKeyEventArgs>(HandleKeyDown);
        Window.KeyUp += new EventHandler<InputKeyEventArgs>(HandleKeyUp);

        EventBus.Subscribe(this, Events.QuitGame, QuitGame);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // Load fonts
        _fontMap.Add(FontID.Status, Content.Load<SpriteFont>(FilePaths.StatusFont));
        _fontMap.Add(FontID.Title, Content.Load<SpriteFont>(FilePaths.TitleFont));
        _fontMap.Add(FontID.Menu, Content.Load<SpriteFont>(FilePaths.MenuFont));
        _fontMap.Add(FontID.Messages, _fontMap[FontID.Status]);

        // Load textures
        _textureMap.Add(TextureID.ForestFloor, Content.Load<Texture2D>(FilePaths.ForestFloor));
        _textureMap.Add(TextureID.ForestWall, Content.Load<Texture2D>(FilePaths.ForestWall));
        _textureMap.Add(TextureID.Player, Content.Load<Texture2D>(FilePaths.Player));
        _textureMap.Add(TextureID.Orc, Content.Load<Texture2D>(FilePaths.Orc));
        _textureMap.Add(TextureID.Wolf, Content.Load<Texture2D>(FilePaths.Wolf));
        _textureMap.Add(TextureID.WhitePixel, new Texture2D(GraphicsDevice, 1, 1));
        _textureMap[TextureID.WhitePixel].SetData([Color.White]);

        // Load music
        _songMap.Add(SongID.HideAndSeek, Content.Load<Song>(FilePaths.HideAndSeek));
        _songMap.Add(SongID.Scavenge1, Content.Load<Song>(FilePaths.Scavenge1));
        _songMap.Add(SongID.Scavenge2, Content.Load<Song>(FilePaths.Scavenge2));
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        if (InputHandler is Utils.IUpdateable inputHandler)
            inputHandler.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin();
        InputHandler.Draw(SpriteBatch, _textureMap, _effectMap, _songMap, _fontMap);
        SpriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

#nullable enable
    public void HandleKeyDown(object? sender, InputKeyEventArgs eventArgs)
    {
        if (_keyList.Contains(eventArgs.Key))
            return;
        _keyList.Add(eventArgs.Key);
        InputHandler = InputHandler.HandleKey(eventArgs.Key);
    }

    public void HandleKeyUp(object? sender, InputKeyEventArgs eventArgs)
    {
        _keyList.Remove(eventArgs.Key);
    }

    public void QuitGame()
    {
        Exit();
    }
}
