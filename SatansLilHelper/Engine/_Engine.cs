using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Apos.Camera;
using Friflo.Engine.ECS;
using MessagePack;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MLEM.Input;
using Newtonsoft.Json;
using SatansLilHelper.Constants;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.GameComponents;
using SatansLilHelper.InputHandlers;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper;

internal partial class Engine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private string _gamePath;
    private List<Keys> _keyList;
    private GameScreen _gameScreen;
    private TitleScreen _titleScreen;
    private MainMenu _mainMenuScreen;
    private SatanFace _satanScreen;
    private StatusWindow _statusScreen;
    private PlayerTurn _playerTurn;
    private PauseMenu _pauseMenu;
    private Minimap _miniMap;
    private Megamap _megaMap;

    private InputHandler _handler;

    public InputHandler Handler => _handler;
    public GameScreen GameScreen => _gameScreen;
    public TitleScreen TitleScreen => _titleScreen;
    public MainMenu MainMenuScreen => _mainMenuScreen;
    public SatanFace SatanScreen => _satanScreen;
    public StatusWindow StatusScreen => _statusScreen;
    public PlayerTurn PlayerTurn => _playerTurn;
    public PauseMenu PauseMenu => _pauseMenu;
    public Minimap MiniMap => _miniMap;
    public Megamap MegaMap => _megaMap;

    public Engine()
    {
        InitializeECS();
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _keyList = [];
        _gamePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SmuB Games",
            "Satans Lil Helper"
        );
        Directory.CreateDirectory(_gamePath);
        Debug.WriteLine("engine constructor");
        _titleScreen = new(this) { Visible = false, Enabled = false };
        _mainMenuScreen = new(this) { Visible = false, Enabled = false };
        _satanScreen = new(this) { Visible = false, Enabled = false };
        _gameScreen = new(this) { Visible = false, Enabled = false };
        _statusScreen = new(this) { Visible = false, Enabled = false };
        _pauseMenu = new(this) { Visible = false, Enabled = false };
        _miniMap = new(this) { Visible = false, Enabled = false };
        _megaMap = new(this) { Visible = false, Enabled = false };

        Components.Add(_titleScreen);
        Components.Add(_mainMenuScreen);
        Components.Add(_satanScreen);
        Components.Add(_gameScreen);
        Components.Add(_statusScreen);
        Components.Add(_pauseMenu);
        Components.Add(_miniMap);
        Components.Add(_megaMap);
        _handler = new(this);
        _gameWorld = new(new MersenneTwister(), new Point(100));
        _playerTurn = new(_gameWorld.Player);
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

        //Window.Position = new Point(4000, 1080 - 720 / 2);
        Window.Title = Properties.GameStrings.GameTitle;

        EventBus.Subscribe<EventMessage>(this, Events.QuitGame, QuitGame);
#if RELEASE
        TitleScreen.Visible = true;
        TitleScreen.Enabled = true;
        SatanScreen.Enabled = true;
        SatanScreen.Visible = true;
#elif DEBUG
        GameScreen.Enabled = true;
        GameScreen.Visible = true;
        StatusScreen.Enabled = true;
        StatusScreen.Visible = true;
#endif
        base.Initialize();
    }

    private void SaveSettings()
    {
        string json = JsonConvert.SerializeObject(Settings.Default);
        File.WriteAllText(_gamePath + "/settings.json", json);
        //Debug.WriteLine(MessagePackSerializer.Serialize(Settings.Default));
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        Handler.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Colors.Black);
        base.Draw(gameTime);
    }

    public void HandleKeyDown(object? _, InputKeyEventArgs eventArgs)
    {
        if (_keyList.Contains(eventArgs.Key))
            return;
        _keyList.Add(eventArgs.Key);
    }

    public void HandleKeyUp(object? _, InputKeyEventArgs eventArgs)
    {
        _keyList.Remove(eventArgs.Key);
    }

    public void QuitGame(EventMessage _)
    {
        SaveSettings();
        Exit();
    }

    private void InitializeECS()
    {
        NativeAOT aot = new();

        aot.RegisterRelation<AbilityStat, AbilityID>();
        aot.RegisterComponent<ActionDelay>();
        aot.RegisterComponent<Attack>();
        aot.RegisterComponent<ECSComponents.Effect>();
        aot.RegisterIndexedComponentEntity<Equipper>();
        aot.RegisterIndexedComponentEntity<Inventory>();
        aot.RegisterComponent<ItemSlots>();
        aot.RegisterIndexedComponentEntity<Grimoire>();
        aot.RegisterComponent<Level>();
        aot.RegisterIndexedComponentStruct<Location, (int, int)>();
        aot.RegisterComponent<Radius>();
        aot.RegisterComponent<RandomEffect>();
        aot.RegisterComponent<ECSComponents.Range>();
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
        aot.RegisterTag<Targetable>();
        aot.RegisterTag<Visible>();

        aot.CreateSchema();
    }
}
