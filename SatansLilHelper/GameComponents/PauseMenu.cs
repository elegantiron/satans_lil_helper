using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        _panel.AddChild(new Paragraph(Anchor.AutoCenter, 0.65f, "Pause"));
    }

    public override void Draw(GameTime gameTime)
    {
        if (_spriteBatch is null)
            return;
    }
}
