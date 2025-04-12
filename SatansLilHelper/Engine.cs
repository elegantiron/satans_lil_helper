using System;
using System.Collections.Generic;
using System.IO;
using Apos.Camera;
using Friflo.Engine.ECS;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.InputHandlers;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper;

public class Engine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private IInputHandler _inputHandler;
    private Dictionary<TextureID, Texture2D> _textureMap;
    private Dictionary<EffectID, SoundEffect> _effectMap;
    private Dictionary<SongID, Song> _songMap;
    private Dictionary<FontID, SpriteFont> _fontMap;
    private List<Keys> _keyList;
    private string _gamePath;
    private Camera? _camera;

    public Engine()
    {
        InitializeECS();
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _textureMap = [];
        _effectMap = [];
        _songMap = [];
        _fontMap = [];
        _keyList = [];

#if RELEASE
        _inputHandler = new TitleInputHandler();
#endif
#if DEBUG
        _inputHandler = new GameInputHandler();
#endif
        _gamePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SmuB Games",
            "Satans Lil Helper"
        );
        Directory.CreateDirectory(_gamePath);
    }

    protected override void Initialize()
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            //.AddJsonFile(_gamePath + "settings.json", true)
            .Build();
        config.Bind(Settings.Default);
        SaveSettings();

        // TODO: Add your initialization logic here
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.SynchronizeWithVerticalRetrace = true;
        _graphics.ApplyChanges();

        Window.Title = Properties.GameStrings.GameTitle;
        Window.KeyDown += new EventHandler<InputKeyEventArgs>(HandleKeyDown);
        Window.KeyUp += new EventHandler<InputKeyEventArgs>(HandleKeyUp);

        EventBus.Subscribe<EventMessage>(this, Events.QuitGame, QuitGame);

        base.Initialize();
    }

    private void SaveSettings()
    {
        string json = JsonConvert.SerializeObject(Settings.Default);
        File.WriteAllText(_gamePath + "settings.json", json);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        IVirtualViewport defaultViewport = new DefaultViewport(GraphicsDevice, Window);
        _camera = new(defaultViewport);

        // Load fonts
        _fontMap.Add(FontID.Status, Content.Load<SpriteFont>(FilePaths.StatusFont));
        _fontMap.Add(FontID.Title, Content.Load<SpriteFont>(FilePaths.TitleFont));
        _fontMap.Add(FontID.Menu, Content.Load<SpriteFont>(FilePaths.MenuFont));
        _fontMap.Add(FontID.Messages, _fontMap[FontID.Status]);

        // Load textures
        LoadTextures();

        // Load music
        _songMap.Add(SongID.HideAndSeek, Content.Load<Song>(FilePaths.HideAndSeek));
        _songMap.Add(SongID.Scavenge1, Content.Load<Song>(FilePaths.Scavenge1));
        _songMap.Add(SongID.Scavenge2, Content.Load<Song>(FilePaths.Scavenge2));
    }

    private void LoadTextures()
    {
        _textureMap.Add(TextureID.ForestFloor, Content.Load<Texture2D>(FilePaths.ForestFloor));
        _textureMap.Add(TextureID.ForestWall, Content.Load<Texture2D>(FilePaths.ForestWall));
        _textureMap.Add(TextureID.Player, Content.Load<Texture2D>(FilePaths.Player));
        _textureMap.Add(TextureID.Orc, Content.Load<Texture2D>(FilePaths.Orc));
        _textureMap.Add(TextureID.Wolf, Content.Load<Texture2D>(FilePaths.Wolf));
        _textureMap.Add(TextureID.SatanMain, Content.Load<Texture2D>(FilePaths.SatanMain));
        _textureMap.Add(TextureID.KeyboardRight, Content.Load<Texture2D>(FilePaths.KeyboardRight));
        _textureMap.Add(
            TextureID.KeyboardRightOutline,
            Content.Load<Texture2D>(FilePaths.KeyboardRightOutline)
        );
        _textureMap.Add(
            TextureID.SatanEyesClosed,
            Content.Load<Texture2D>(FilePaths.SatanEyesClosed)
        );
        _textureMap.Add(TextureID.SatanEyesOpen, Content.Load<Texture2D>(FilePaths.SatanEyesOpen));
        _textureMap.Add(
            TextureID.SatanMouthClosed,
            Content.Load<Texture2D>(FilePaths.SatanMouthClosed)
        );
        _textureMap.Add(
            TextureID.SatanMouthOpen,
            Content.Load<Texture2D>(FilePaths.SatanMouthOpen)
        );
        _textureMap.Add(TextureID.WhitePixel, new Texture2D(GraphicsDevice, 1, 1));
        _textureMap[TextureID.WhitePixel].SetData([Color.White]);
        _textureMap.Add(TextureID.KeyboardUp, Content.Load<Texture2D>(FilePaths.KeyboardUp));
        _textureMap.Add(TextureID.KeyboardDown, Content.Load<Texture2D>(FilePaths.KeyboardDown));
        _textureMap.Add(TextureID.KeyboardLeft, Content.Load<Texture2D>(FilePaths.KeyboardLeft));
        _textureMap.Add(
            TextureID.KeyboardDelete,
            Content.Load<Texture2D>(FilePaths.KeyboardDelete)
        );
        _textureMap.Add(
            TextureID.KeyboardInsert,
            Content.Load<Texture2D>(FilePaths.KeyboardInsert)
        );
        _textureMap.Add(
            TextureID.KeyboardPageUp,
            Content.Load<Texture2D>(FilePaths.KeyboardPageUp)
        );
        _textureMap.Add(
            TextureID.KeyboardPageDown,
            Content.Load<Texture2D>(FilePaths.KeyboardPageDown)
        );
        _textureMap.Add(TextureID.Keyboard1, Content.Load<Texture2D>(FilePaths.Keyboard1));
        _textureMap.Add(TextureID.Keyboard2, Content.Load<Texture2D>(FilePaths.Keyboard2));
        _textureMap.Add(TextureID.Keyboard3, Content.Load<Texture2D>(FilePaths.Keyboard3));
        _textureMap.Add(TextureID.Keyboard4, Content.Load<Texture2D>(FilePaths.Keyboard4));
        _textureMap.Add(TextureID.Keyboard6, Content.Load<Texture2D>(FilePaths.Keyboard6));
        _textureMap.Add(TextureID.Keyboard7, Content.Load<Texture2D>(FilePaths.Keyboard7));
        _textureMap.Add(TextureID.Keyboard8, Content.Load<Texture2D>(FilePaths.Keyboard8));
        _textureMap.Add(TextureID.Keyboard9, Content.Load<Texture2D>(FilePaths.Keyboard9));
        _textureMap.Add(TextureID.KeyboardF, Content.Load<Texture2D>(FilePaths.KeyboardF));
        _textureMap.Add(TextureID.KeyboardH, Content.Load<Texture2D>(FilePaths.KeyboardH));
        _textureMap.Add(TextureID.KeyboardI, Content.Load<Texture2D>(FilePaths.KeyboardI));
        _textureMap.Add(TextureID.KeyboardS, Content.Load<Texture2D>(FilePaths.KeyboardS));
        _textureMap.Add(TextureID.KeyboardT, Content.Load<Texture2D>(FilePaths.KeyboardT));
        _textureMap.Add(
            TextureID.KeyboardReturn,
            Content.Load<Texture2D>(FilePaths.KeyboardReturn)
        );
        _textureMap.Add(
            TextureID.KeyboardEscape,
            Content.Load<Texture2D>(FilePaths.KeyboardEscape)
        );
        _textureMap.Add(TextureID.KeyboardY, Content.Load<Texture2D>(FilePaths.KeyboardY));
        _textureMap.Add(TextureID.KeyboardZ, Content.Load<Texture2D>(FilePaths.KeyboardZ));
        _textureMap.Add(TextureID.ArrowSilver, Content.Load<Texture2D>(FilePaths.ArrowSilver));
        _textureMap.Add(TextureID.ArrowBlue, Content.Load<Texture2D>(FilePaths.ArrowBlue));
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        if (_inputHandler is Interfaces.IUpdateable inputHandler)
            inputHandler.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);
        if (_spriteBatch != null && _camera != null)
            _inputHandler.Draw(_spriteBatch, _camera, _textureMap, _effectMap, _songMap, _fontMap);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }

#nullable enable
    public void HandleKeyDown(object? sender, InputKeyEventArgs eventArgs)
    {
        if (_keyList.Contains(eventArgs.Key))
            return;
        _keyList.Add(eventArgs.Key);
        _inputHandler = _inputHandler.HandleKey(eventArgs.Key);
    }

    public void HandleKeyUp(object? sender, InputKeyEventArgs eventArgs)
    {
        _keyList.Remove(eventArgs.Key);
    }

    public void QuitGame(EventMessage _)
    {
        if (_inputHandler is ISaveable inputHandler)
        {
            inputHandler.DumpData(_gamePath);
        }
        SaveSettings();
        Exit();
    }

    private void InitializeECS()
    {
        NativeAOT aot = new();

        aot.RegisterRelation<AbilityStat, AbilityID>();
        aot.RegisterComponent<ActionDelay>();
        aot.RegisterComponent<Attack>();
        aot.RegisterComponent<Components.Effect>();
        aot.RegisterIndexedComponentEntity<Equipper>();
        aot.RegisterIndexedComponentEntity<Inventory>();
        aot.RegisterComponent<ItemSlots>();
        aot.RegisterIndexedComponentEntity<Grimoire>();
        aot.RegisterComponent<Level>();
        aot.RegisterIndexedComponentStruct<Location, (int, int)>();
        aot.RegisterComponent<RandomEffect>();
        aot.RegisterComponent<Targetable>();
        aot.RegisterComponent<TextureIndex>();

        aot.RegisterTag<Activatable>();
        aot.RegisterTag<Actor>();
        aot.RegisterTag<Alive>();
        aot.RegisterTag<Blocking>();
        aot.RegisterTag<DamagesInterruptor>();
        aot.RegisterTag<Equippable>();
        aot.RegisterTag<Hostile>();
        aot.RegisterTag<Interruptible>();
        aot.RegisterTag<Invisible>();
        aot.RegisterTag<Item>();
        aot.RegisterTag<MovesActor>();
        aot.RegisterTag<MovesTarget>();
        aot.RegisterTag<Player>();
        aot.RegisterTag<Skill>();
        aot.RegisterTag<Visible>();

        aot.CreateSchema();
    }
}
