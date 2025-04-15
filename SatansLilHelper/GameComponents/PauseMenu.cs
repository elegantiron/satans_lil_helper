using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

internal class PauseMenu : DrawableGameComponent
{
    private SpriteBatch? _spriteBatch;
    private UiSystem? _system;
    private Panel? _panel;
    private Button? _resume,
        _bestiary,
        _mainMenu,
        _toDesktop;

    public PauseMenu(Game game)
        : base(game) { }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
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
        _system = new(Game, style);
        _panel = new(Anchor.Center, new Vector2(450, 350));
        _system.Add("root", _panel);
        _panel.AddChild(new VerticalSpace(5));
        _panel.AddChild(new Paragraph(Anchor.AutoCenter, 0.65f, "Pause"));
        _panel.AddChild(new VerticalSpace(10));
        _panel.SetHeightBasedOnChildren = true;

        Vector2 buttonSize = new(0.73f, 55);
        _resume = new(Anchor.AutoCenter, buttonSize, text: Properties.GameStrings.Resume)
        {
            OnSelected = HandleButtons,
        };
        _panel.AddChild(_resume);
        _panel.AddChild(new VerticalSpace(5));
        _bestiary = new(Anchor.AutoCenter, buttonSize, text: Properties.GameStrings.ViewBestiary)
        {
            OnSelected = HandleButtons,
        };
        _panel.AddChild(_bestiary);
        _panel.AddChild(new VerticalSpace(5));
        _mainMenu = new(Anchor.AutoCenter, buttonSize, text: Properties.GameStrings.ToMenu)
        {
            OnSelected = HandleButtons,
        };
        _panel.AddChild(_mainMenu);
        _panel.AddChild(new VerticalSpace(5));
        _toDesktop = new(Anchor.AutoCenter, buttonSize, text: Properties.GameStrings.ToDesktop)
        {
            OnSelected = HandleButtons,
        };
        _panel.AddChild(_toDesktop);
        _panel.AddChild(new VerticalSpace(15));
    }

    public override void Draw(GameTime gameTime)
    {
        if (_spriteBatch is null)
            return;
        _system?.Draw(gameTime, _spriteBatch);
    }

    public override void Update(GameTime gameTime)
    {
        if (_system is null || Game is not Engine engine)
            return;
        engine.Handler.Update(gameTime);
        _system.Update(gameTime);

        if (engine.Handler.TryConsumePressed(Keys.Escape))
        {
            Enabled = false;
            Visible = false;
            engine.GameScreen.Enabled = true;
        }
        if (DrawOrder <= engine.StatusScreen.DrawOrder)
            DrawOrder = engine.StatusScreen.DrawOrder + 1;
    }

    private void HandleButtons(Element element)
    {
        if (Game is not Engine engine || _bestiary is null || _resume is null)
            return;
        if (element == _bestiary) { }
        else if (element == _resume)
        {
            Enabled = false;
            Visible = false;
            engine.GameScreen.Enabled = true;
        }
        else if (element == _mainMenu)
        {
            engine.SaveGame();
            Enabled = false;
            Visible = false;
            engine.GameScreen.Enabled = false;
            engine.GameScreen.Visible = false;
            engine.TitleScreen.Visible = true;
            engine.TitleScreen.Enabled = true;
            engine.MainMenuScreen.Visible = true;
            engine.MainMenuScreen.Enabled = true;
        }
        else if (element == _toDesktop)
            Game.Exit();
    }
}
