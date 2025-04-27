using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class TargetingOverlay : DrawableGameComponent
{
    private SpriteBatch? batch;
    private int range,
        radius;
    private Vector2 center,
        offset;
    private Camera? camera;

    public TargetingOverlay(Game game)
        : base(game) { }

    protected override void LoadContent()
    {
        base.LoadContent();
        batch = new(Game.GraphicsDevice);
    }

    public override void Draw(GameTime gameTime)
    {
        if (batch is null)
            return;
        batch.Begin();

        batch.End();
    }

    public override void Update(GameTime gameTime)
    {
        if (Game is not Engine engine)
            return;

        if (Settings.Default.Movement.IsPressedAvailable(engine.Handler))
            ProcessMovement();
    }

    private void ProcessMovement()
    {
        if (Game is not Engine engine)
            return;
        int dx = 0,
            dy = 0;
        if (
            Settings.Default.MoveUp.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveUpLeft.IsPressedAvailable(engine.Handler)
            || Settings.Default.MoveUpRight.IsPressedAvailable(engine.Handler)
        )
            dy = -1;
        if (
            Settings.Default.MoveUpRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownRight.IsPressedAvailable(engine.Handler)
        )
            dx = 1;
        if (
            Settings.Default.MoveDownRight.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDown.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveDownLeft.IsPressedAvailable(engine.Handler)
        )
            dy = 1;
        if (
            Settings.Default.MoveDownLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveLeft.TryConsumePressed(engine.Handler)
            || Settings.Default.MoveUpLeft.TryConsumePressed(engine.Handler)
        )
            dx = -1;
    }
}
