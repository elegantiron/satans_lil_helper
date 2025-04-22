using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class ConfirmPopup : DrawableGameComponent
{
    private SpriteBatch? batch;
    private Texture2D? pixel,
        enter,
        escape;
    private Action<bool>? callback;

    public ConfirmPopup(Game game)
        : base(game) { }

    public override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        batch = new(Game.GraphicsDevice);

        enter = Game.Content.Load<Texture2D>(FilePaths.KeyboardReturn);
        escape = Game.Content.Load<Texture2D>(FilePaths.KeyboardEscape);

        base.LoadContent();
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
        if (callback is null)
            throw new Exception(
                "You can't initiate a confirmation routine without something to confirm"
            );
    }

    public void SetCallback(Action<bool> callback)
    {
        this.callback = callback;
    }
}
