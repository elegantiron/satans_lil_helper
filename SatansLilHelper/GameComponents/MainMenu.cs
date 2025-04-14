using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MLEM.Font;
using MLEM.Textures;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using SatansLilHelper.Constants;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class MainMenu : DrawableGameComponent
{
    private UiSystem? _system;
    private SpriteBatch? _spriteBatch;
    private Button? _newGame,
        _bestiary,
        _quit;
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
        float BUTTON_HEIGHT = 55;
        float BUTTON_WIDTH = 0.73f;
        _spriteBatch = new(Game.GraphicsDevice);
        var style = new UntexturedStyle(_spriteBatch);
        style.Font = new GenericSpriteFont(Game.Content.Load<SpriteFont>(FilePaths.MenuFont));
        style.TextColor = Colors.Black;
        style.TooltipTextWidth = 250;
        style.TextAlignment = MLEM.Formatting.TextAlignment.Center;
        style.PanelTexture = new NinePatch(
            new TextureRegion(
                Game.Content.Load<Texture2D>(@"Images/UI/Panels/panel_brown_damaged_dark")
            ),
            8f,
            NinePatchMode.Tile
        );
        style.TooltipDelay = new TimeSpan(0, 0, 0, 0, 350);
        style.ButtonTexture = new NinePatch(
            new TextureRegion(
                Game.Content.Load<Texture2D>(@"Images/UI/Panels/panel_brown_damaged")
            ),
            8f,
            NinePatchMode.Tile
        );

        _system = new(Game, style);
        var panel = new Panel(Anchor.Center, new Vector2(550, 100), new Vector2(0));
        panel.SetHeightBasedOnChildren = true;
        _system.Add("panel", panel);
        _newGame = new(
            Anchor.AutoCenter,
            new Vector2(BUTTON_WIDTH, BUTTON_HEIGHT),
            text: Properties.GameStrings.NewGame
        );
        _newGame.OnPressed = HandleButtonPress;
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
        panel.AddChild(new Paragraph(Anchor.AutoCenter, 0.75f, "Main Menu"));
        panel.AddChild(new VerticalSpace(15));
        panel.AddChild(_newGame);
        panel.AddChild(new VerticalSpace(5));
        panel.AddChild(_bestiary);
        panel.AddChild(new VerticalSpace(5));
        panel.AddChild(_quit);
        panel.AddChild(new VerticalSpace(35));
        base.LoadContent();
    }

    public override void Update(GameTime gameTime)
    {
        _system?.Update(gameTime);
        base.Update(gameTime);
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
        if (Enabled)
        {
            Game.Window.KeyDown += _keyDown;
            Game.Window.KeyUp += _keyUp;
        }
        else
        {
            Game.Window.KeyDown -= _keyDown;
            Game.Window.KeyUp -= _keyUp;
        }
    }

    private void HandleButtonPress(Element element)
    {
        if (_newGame is not null && element == _newGame)
            Debug.WriteLine("you pressed new game!");
        else if (_bestiary is not null && element == _bestiary)
            Debug.WriteLine("you pressed bestiary!");
    }

    public void HandleKeyDown(object? sender, InputKeyEventArgs eventArgs)
    {
        if (_keyList.Contains(eventArgs.Key))
            return;
        _keyList.Add(eventArgs.Key);
        switch (eventArgs.Key)
        {
            case Keys.Escape:
                if (Game is Engine engine)
                {
                    engine.SatanScreen.Enabled = true;
                    engine.SatanScreen.Visible = true;
                    Enabled = false;
                    Visible = false;
                }
                break;
        }
    }

    public void HandleKeyUp(object? sender, InputKeyEventArgs eventArgs)
    {
        _keyList.Remove(eventArgs.Key);
    }
}
