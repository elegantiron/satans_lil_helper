using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Input;
using Newtonsoft.Json;
using SatansLilHelper.Constants;
using SatansLilHelper.GameComponents;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper;

internal partial class Engine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private string _gamePath;
    private List<Keys> _keyList;

    private InputHandler _handler;

    public InputHandler Handler => _handler;

    public Engine()
    {
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

        InitializeECS();
        SetupGameComponents();

        _handler = new(this);
        _gameWorld = new(new MersenneTwister(), new Point(100));
        _playerTurn = new(_gameWorld.Player);
        _inCombat = false;
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

    public void QuitGame(EventMessage _)
    {
        SaveSettings();
        Exit();
    }
}
