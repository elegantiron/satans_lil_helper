using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
}
