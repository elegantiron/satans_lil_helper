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
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class IncomingLinks<TComponent> : DrawableGameComponent
    where TComponent : struct, ILinkComponent
{
    private SpriteBatch? batch;
    private UiSystem? system;
    private UiStyle? style;
    private Panel? mainPanel,
        subPanel;
    private Vector2 buttonSize;

    public IncomingLinks(Game game)
        : base(game)
    {
        buttonSize = new(0.73f, 55);
    }

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
            subPanel?.RemoveChildren((Element element) => true);

            subPanel?.AddChild(new VerticalSpace(10));
            EntityLinks<TComponent> links = engine.World.Player.GetIncomingLinks<TComponent>();
            foreach (Entity entity in links.Entities)
            {
                subPanel?.AddChild(MakeButton(entity));
                subPanel?.AddChild(new VerticalSpace(5));
            }
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
            ScrollBarScrollerTexture = new NinePatch(
                new TextureRegion(
                    Game.Content.Load<Texture2D>(@"Images/UI/Panels/scrollbar_future_red_small")
                ),
                4,
                4,
                6,
                6,
                NinePatchMode.Tile
            ),
            ScrollBarBackground = new NinePatch(
                new TextureRegion(
                    Game.Content.Load<Texture2D>(@"Images/UI/Panels/scrollbar_future_grey")
                ),
                4,
                4,
                6,
                6,
                NinePatchMode.Tile
            ),
            PanelScrollerSize = new Vector2(16, 24),
        };
        system = new(Game, style);
        mainPanel = new(Anchor.Center, new Vector2(550, 300), new Vector2(0));
        mainPanel.AddChild(
            new Paragraph(
                Anchor.AutoCenter,
                buttonSize.X,
                text: Properties.GameStrings.Inventory_Title
            )
        );
        subPanel = new(Anchor.AutoCenter, Vector2.One, scrollOverflow: true)
        {
            Texture = null,
            PreventParentSpill = true,
        };
        mainPanel.AddChild(subPanel);
        system.Add("base panel", mainPanel);
    }

    public override void Draw(GameTime gameTime)
    {
        system?.Draw(gameTime, batch);
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is not Engine engine || system is null)
            return;
        base.Update(gameTime);
        if (engine.Handler.TryConsumePressed(Keys.Space))
            OnEnabledChanged(new object(), new EventArgs());
        system.Update(gameTime);
        if (Settings.Default.Cancel.TryConsumePressed(engine.Handler))
        {
            Enabled = false;
            Visible = false;
            engine.GameScreen.Enabled = true;
        }
    }

    private Button MakeButton(Entity entity)
    {
        string text = entity.Name.value;
        if (entity.HasComponent<Equipper>())
            text += " (equipped)";
        return new Button(Anchor.AutoCenter, buttonSize, text: text);
    }
}
