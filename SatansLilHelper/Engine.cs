using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using SatansLilHelper.Constants;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.GameComponents;
using SatansLilHelper.InputHandlers;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper;

public class Engine : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch? _spriteBatch;
    private string _gamePath;
    private List<Keys> _keyList;
    private DrawableGameComponent titleScreen,
        mainMenuScreen,
        satanScreen;

    public DrawableGameComponent TitleScreen => titleScreen;
    public DrawableGameComponent MainMenuScreen => mainMenuScreen;

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
        titleScreen = new TitleScreen(this) { Visible = false, Enabled = false };
        mainMenuScreen = new MainMenu(this) { Visible = false, Enabled = false };
        satanScreen = new SatanFace(this) { Visible = false, Enabled = false };

        Components.Add(titleScreen);
        Components.Add(mainMenuScreen);
        Components.Add(satanScreen);
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
        //Window.KeyDown += new EventHandler<InputKeyEventArgs>(HandleKeyDown);
        //Window.KeyUp += new EventHandler<InputKeyEventArgs>(HandleKeyUp);

        EventBus.Subscribe<EventMessage>(this, Events.QuitGame, QuitGame);

        TitleScreen.Visible = true;
        TitleScreen.Enabled = true;
        satanScreen.Enabled = true;
        satanScreen.Visible = true;
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
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        // TODO: Add your update logic here
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
    }

    public void HandleKeyDown(object? sender, InputKeyEventArgs eventArgs)
    {
        if (_keyList.Contains(eventArgs.Key))
            return;
        _keyList.Add(eventArgs.Key);
    }

    public void HandleKeyUp(object? sender, InputKeyEventArgs eventArgs)
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
