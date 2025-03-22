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
    private Dictionary<TextureID, Texture2D> TextureMap;
    private Dictionary<EffectID, SoundEffect> _effectMap;
    private Dictionary<SongID, Song> SongMap;
    private Dictionary<FontID, SpriteFont> FontMap;
    private List<Keys> _keyList;

    public Engine()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        TextureMap = [];
        _effectMap = [];
        SongMap = [];
        FontMap = [];
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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        // Load fonts
        FontMap.Add(FontID.Status, Content.Load<SpriteFont>(FilePaths.StatusFont));
        FontMap.Add(FontID.Title, Content.Load<SpriteFont>(FilePaths.TitleFont));
        FontMap.Add(FontID.Menu, Content.Load<SpriteFont>(FilePaths.MenuFont));

        // Load textures
        TextureMap.Add(TextureID.ForestFloor, Content.Load<Texture2D>(FilePaths.ForestFloor));
        TextureMap.Add(TextureID.ForestWall, Content.Load<Texture2D>(FilePaths.ForestWall));
        TextureMap.Add(TextureID.Player, Content.Load<Texture2D>(FilePaths.Player));
        TextureMap.Add(TextureID.Orc, Content.Load<Texture2D>(FilePaths.Orc));
        TextureMap.Add(TextureID.Wolf, Content.Load<Texture2D>(FilePaths.Wolf));
        TextureMap.Add(TextureID.WhitePixel, new Texture2D(GraphicsDevice, 1, 1));
        TextureMap[TextureID.WhitePixel].SetData([Color.White]);

        // Load music
        SongMap.Add(SongID.HideAndSeek, Content.Load<Song>(FilePaths.HideAndSeek));
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        SpriteBatch.Begin();
        InputHandler.Draw(SpriteBatch, TextureMap, _effectMap, SongMap, FontMap);
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
        try
        {
            InputHandler = InputHandler.HandleKey(eventArgs.Key);
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
