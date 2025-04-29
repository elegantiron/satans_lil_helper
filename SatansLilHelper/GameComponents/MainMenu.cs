using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using SatansLilHelper.Constants;

namespace SatansLilHelper.GameComponents;

internal class MainMenu : DrawableGameComponent
{
    private UiSystem? _system;
    private SpriteBatch? _spriteBatch;
    private Button? _newGame,
        _bestiary,
        _quit,
        _continue;
    private List<Keys> _keyList;
    private EventHandler<InputKeyEventArgs> _keyDown,
        _keyUp;

    public MainMenu(Game game)
        : base(game)
    {
        _keyList = [];
        _keyDown = new(HandleKeyDown);
        _keyUp = new(HandleKeyUp);
    }

    protected override void LoadContent()
    {
        if (Game is Engine engine)
        {
            float BUTTON_HEIGHT = 55;
            float BUTTON_WIDTH = 0.73f;
            _spriteBatch = new(Game.GraphicsDevice);
            var style = new UntexturedStyle(_spriteBatch)
            {
                Font = new GenericSpriteFont(Game.Content.Load<SpriteFont>(FilePaths.MenuFont)),
                TextColor = Colors.Black,
                TooltipTextWidth = 250,
                TextAlignment = MLEM.Formatting.TextAlignment.Center,
                PanelTexture = new NinePatch(
                    new TextureRegion(
                        Game.Content.Load<Texture2D>(@"Images/UI/Panels/panel_brown_damaged_dark")
                    ),
                    8f,
                    NinePatchMode.Tile
                ),
                TooltipDelay = new TimeSpan(0, 0, 0, 0, 350),
                ButtonTexture = new NinePatch(
                    new TextureRegion(
                        Game.Content.Load<Texture2D>(@"Images/UI/Panels/panel_brown_damaged")
                    ),
                    8f,
                    NinePatchMode.Tile
                ),
                SelectionIndicator = new NinePatch(
                    new TextureRegion(
                        Game.Content.Load<Texture2D>(@"Images/UI/Panels/panel_border_grey_detail")
                    ),
                    19f,
                    NinePatchMode.Tile
                ),
            };

            // Set up the UI System
            _system = new(Game, style, engine.Handler);
            _system.Controls.DownButtons.Add(Keys.Down);
            _system.Controls.UpButtons.Add(Keys.Up);
            _system.Controls.KeyboardButtons.Add(Keys.Enter);

            // Set up the components of the UI System
            Panel panel = new(Anchor.Center, new Vector2(550, 100), new Vector2(0))
            {
                SetHeightBasedOnChildren = true,
            };
            _system.Add("panel", panel);
            _newGame = new(
                Anchor.AutoCenter,
                new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT),
                text: Properties.GameStrings.NewGame
            )
            {
                OnPressed = HandleButtonPress,
            };
            _bestiary = new(
                Anchor.AutoCenter,
                new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT),
                Properties.GameStrings.ViewBestiary
            )
            {
                OnPressed = HandleButtonPress,
            };
            _quit = new(
                Anchor.AutoCenter,
                new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT),
                Properties.GameStrings.ToDesktop
            )
            {
                OnPressed = HandleButtonPress,
            };
            _continue = new(
                Anchor.AutoCenter,
                new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT),
                text: Properties.GameStrings.Continue
            )
            {
                OnPressed = HandleButtonPress,
            };
            panel.AddChild(new Paragraph(Anchor.AutoCenter, 0.75f, "Main Menu"));
            panel.AddChild(new VerticalSpace(15));
            panel.AddChild(_newGame);
            panel.AddChild(new VerticalSpace(5));

            if (engine.LoadGame())
            {
                panel.AddChild(_continue);
                panel.AddChild(new VerticalSpace(5));
            }

            panel.AddChild(_bestiary);
            panel.AddChild(new VerticalSpace(5));
            panel.AddChild(_quit);
            panel.AddChild(new VerticalSpace(35));
            base.LoadContent();
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is Engine engine)
        {
            _system?.Update(gameTime);
            if (engine.Handler.TryConsumePressed(Keys.Escape))
                Game.Exit();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        if (_spriteBatch is not null && _system is not null)
            _system.Draw(gameTime, _spriteBatch);

        base.Draw(gameTime);
    }

    protected override void OnEnabledChanged(object sender, EventArgs args)
    {
        base.OnEnabledChanged(sender, args);
    }

    private void HandleButtonPress(Element element)
    {
        if (Game is not Engine engine)
            return;
        if (_newGame is not null && element == _newGame)
        {
            engine.NewGame();
            engine.GameScreen.Enabled = true;
            engine.GameScreen.Visible = true;
            engine.StatusScreen.Enabled = true;
            engine.StatusScreen.Visible = true;
            engine.TitleScreen.Visible = false;
            engine.TitleScreen.Enabled = false;
            Enabled = false;
            Visible = false;
        }
        else if (_continue is not null && element == _continue) { }
        else if (_bestiary is not null && element == _bestiary) { }
        else if (_quit is not null && element == _quit)
        {
            engine.QuitGame(new Types.EventMessage());
            Game.Exit();
        }
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
}
