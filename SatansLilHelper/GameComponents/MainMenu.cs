using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Font;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class MainMenu : DrawableGameComponent
{
    private UiSystem? _system;
    private SpriteBatch? _spriteBatch;

    public MainMenu(Game game)
        : base(game) { }

    protected override void LoadContent()
    {
        _spriteBatch = new(Game.GraphicsDevice);
        var style = new UntexturedStyle(_spriteBatch);
        style.Font = new GenericSpriteFont(Game.Content.Load<SpriteFont>(FilePaths.MenuFont));
        style.TooltipTextWidth = 250;
        style.TextAlignment = MLEM.Formatting.TextAlignment.Center;
        style.TooltipDelay = new TimeSpan(0, 0, 0, 0, 350);

        _system = new(Game, style);
        var panel = new Panel(Anchor.Center, new Vector2(500, 100), new Vector2(0));
        _system.Add("panel", panel);
        var newGame = new Button(
            Anchor.AutoCenter,
            new Vector2(0.85f),
            text: "Start a new Game",
            tooltipText: "This is some help text"
        );
        panel.AddChild(newGame);
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
        Debug.WriteLine("main menu activated");
        base.OnEnabledChanged(sender, args);
    }
}
