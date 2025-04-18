using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
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

internal class IncomingLinks<TComponent> : DrawableGameComponent
    where TComponent : struct, ILinkComponent
{
    private SpriteBatch? batch;
    private UiSystem? system;
    private UiStyle? style;
    private Panel? panel;

    public IncomingLinks(Game game)
        : base(game) { }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void OnEnabledChanged(object sender, EventArgs args)
    {
        base.OnEnabledChanged(sender, args);
        if (Game is not Engine engine)
            return;
        if (Enabled)
        {
            panel?.RemoveChildren((Element _) => true);
            EntityLinks<TComponent> entityLinks =
                engine.World.Player.GetIncomingLinks<TComponent>();
        }
    }

    protected override void LoadContent()
    {
        batch = new(Game.GraphicsDevice);
        style = new UntexturedStyle(batch)
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
        system = new(Game, style);
        panel = new(Anchor.Center, new Vector2(0.5f));
    }

    public override void Draw(GameTime gameTime) { }

    public override void Update(GameTime gameTime)
    {
        if (Game is not Engine engine || system is null)
            return;
        base.Update(gameTime);
        if (engine.Handler.TryConsumePressed(Keys.Space))
            OnEnabledChanged(new object(), new EventArgs());
    }

    private Button MakeButton(Anchor anchor, Vector2 size, string text)
    {
        Button newButton = new(anchor, size, text);
        throw new NotImplementedException();
    }
}
