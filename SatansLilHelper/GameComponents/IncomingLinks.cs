using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    private Button? close;
    private Vector2 buttonSize;
    private Dictionary<Element, Entity> entities;

    public IncomingLinks(Game game)
        : base(game)
    {
        buttonSize = new(0.8f, 55);
        entities = [];
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
            UpdateList(engine);
        }
    }

    private void UpdateList(Engine engine)
    {
        entities.Clear();
        subPanel?.RemoveChildren((Element element) => true);

        subPanel?.AddChild(new VerticalSpace(10));
        EntityLinks<TComponent> links = engine.World.Player.GetIncomingLinks<TComponent>();
        foreach (Entity entity in links.Entities)
        {
            var element = MakeButton(entity);
            entities.Add(element, entity);
            subPanel?.AddChild(element);
            subPanel?.AddChild(new VerticalSpace(5));
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
                10,
                9,
                9,
                9,
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
        system.Controls.DownButtons.CopyFrom(Settings.Default.MoveDown);
        system.Controls.UpButtons.CopyFrom(Settings.Default.MoveUp);
        system.Controls.KeyboardButtons.CopyFrom(Settings.Default.Confirm);
        mainPanel = new(Anchor.Center, new Vector2(550, 450), new Vector2(0));

        mainPanel.AddChild(
            new Paragraph(
                Anchor.AutoCenter,
                buttonSize.X,
                text: Properties.GameStrings.Inventory_Title
            )
        );
        subPanel = new(Anchor.AutoCenter, new Vector2(0.9f, 330), scrollOverflow: true)
        {
            GetTabNextElement = (bool _, Element _) => close,
            PreventParentSpill = true,
            Texture = null,
        };
        close = new Button(Anchor.AutoCenter, new Vector2(0.25f, 55), text: "Close")
        {
            GetTabNextElement = (bool _, Element _) => subPanel,
            OnPressed = DoClose,
        };

        mainPanel.AddChild(subPanel);
        mainPanel.AddChild(new VerticalSpace(15));
        mainPanel.AddChild(close);
        system.Add("base panel", mainPanel);
    }

    public override void Draw(GameTime gameTime)
    {
        system?.Draw(gameTime, batch);
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is not Engine engine)
            return;
        base.Update(gameTime);
        if (engine.Handler.TryConsumePressed(Keys.Space))
            UpdateList(engine);
        system?.Update(gameTime);
        if (Settings.Default.Cancel.TryConsumePressed(engine.Handler))
        {
            DoClose();
        }
    }

    private void DoClose()
    {
        if (Game is not Engine engine)
            return;
        Enabled = false;
        Visible = false;
        engine.GameScreen.Enabled = true;
    }

    private void DoClose(Element _)
    {
        DoClose();
    }

    private Button MakeButton(Entity entity)
    {
        string text = entity.Name.value;
        if (entity.HasComponent<Equipper>())
            text += " (equipped)";
        return new Button(Anchor.AutoCenter, buttonSize, text: text) { OnPressed = Process };
    }

    private void Process(Element element)
    {
        Debug.WriteLine(entities[element]);
    }
}
