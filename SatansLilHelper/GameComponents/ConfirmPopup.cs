using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SatansLilHelper.Constants;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class ConfirmPopup : DrawableGameComponent
{
    private SpriteBatch? batch;
    private Texture2D? pixel,
        enter,
        escape;
    private SpriteFont? font;
    private Action<bool>? callback;
    private string query;

    public ConfirmPopup(Game game)
        : base(game)
    {
        query = "";
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        batch = new(Game.GraphicsDevice);

        pixel = new(Game.GraphicsDevice, 1, 1);
        pixel.SetData([Color.White]);

        enter = Game.Content.Load<Texture2D>(FilePaths.KeyboardReturn);
        escape = Game.Content.Load<Texture2D>(FilePaths.KeyboardEscape);

        font = Game.Content.Load<SpriteFont>(FilePaths.StatusFont);

        base.LoadContent();
    }

    public override void Draw(GameTime gameTime)
    {
        if (
            batch is null
            || Game is not Engine engine
            || font is null
            || enter is null
            || escape is null
        )
            return;
        batch.Begin(samplerState: SamplerState.PointClamp);
        batch.Draw(
            pixel,
            new Rectangle(
                Game.GraphicsDevice.Viewport.Width / 3,
                Game.GraphicsDevice.Viewport.Height / 4,
                Game.GraphicsDevice.Viewport.Width / 3,
                Game.GraphicsDevice.Viewport.Height / 2
            ),
            Colors.TranslucentBlack
        );
        batch.Draw(
            enter,
            new Vector2(
                Game.GraphicsDevice.Viewport.Width / 3 + 10,
                Game.GraphicsDevice.Viewport.Height * 3 / 4 - 70
            ),
            Colors.White
        );
        batch.Draw(
            escape,
            new Vector2(
                Game.GraphicsDevice.Viewport.Width * 2 / 3 - 10 - escape.Width,
                Game.GraphicsDevice.Viewport.Height * 3 / 4 - 70
            ),
            Colors.White
        );
        Vector2 size = font.MeasureString(Properties.GameStrings.Yes);
        batch.DrawString(
            font,
            Properties.GameStrings.Yes,
            new Vector2(
                Game.GraphicsDevice.Viewport.Width / 3 + 10 + enter.Width,
                Game.GraphicsDevice.Viewport.Height * 3 / 4 - 70 + enter.Height / 2 - size.Y / 2
            ),
            Colors.White
        );
        size = font.MeasureString(Properties.GameStrings.No);
        batch.DrawString(
            font,
            Properties.GameStrings.No,
            new Vector2(
                Game.GraphicsDevice.Viewport.Width * 2 / 3 - 10 - escape.Width - size.X,
                Game.GraphicsDevice.Viewport.Height * 3 / 4 - 70 + escape.Height / 2 - size.Y / 2
            ),
            Colors.White
        );
        batch.End();
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is not Engine engine || callback is null)
            return;
        base.Update(gameTime);
        if (Settings.Default.Confirm.TryConsumePressed(engine.Handler))
        {
            DoCallback(true);
        }
        else if (Settings.Default.Cancel.TryConsumePressed(engine.Handler))
        {
            DoCallback(false);
        }
    }

    private void DoCallback(bool confirmed)
    {
        if (callback is null || Game is not Engine engine)
            return;
        callback(confirmed);
        callback = null;
        Enabled = false;
        Visible = false;
    }

    protected override void OnEnabledChanged(object sender, EventArgs args)
    {
        base.OnEnabledChanged(sender, args);
        if (Enabled && callback is null)
            throw new Exception(
                "You can't initiate a confirmation routine without something to confirm"
            );
    }

    public void SetData(Action<bool> callback, string query)
    {
        this.callback = callback;
        this.query = query;
    }
}
