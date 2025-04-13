using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MLEM.Font;
using MLEM.Ui;
using MLEM.Ui.Elements;
using MLEM.Ui.Style;

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
        var style = new UiStyle()
        {
            Font = new GenericSpriteFont(Game.Content.Load<SpriteFont>(FilePaths.MenuFont)),
        };
        _system = new(
            Game,
            new UntexturedStyle(_spriteBatch)
            {
                Font = new GenericSpriteFont(Game.Content.Load<SpriteFont>(FilePaths.MenuFont)),
            }
        );
        var panel = new Panel(Anchor.Center, new Vector2(1280, 100), true, false, true);
        _system.Add("panel", panel);
        var checkbox = new Checkbox(Anchor.AutoCenter, new Vector2(100, 100), "A checkbox");
        panel.AddChild(checkbox);
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
