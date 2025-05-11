using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using YARGGH;
using YARGGH.Scenes;

namespace SatansLilHelper.Scenes;

public class TitleScene : Scene
{
    private Texture2D _satanBase,
        _satanEyesOpen,
        _satanMouthClosed,
        _satanEyesClosed;

    private Vector2 _satanPosition,
        _satanCenter;

    public TitleScene()
    {
        _satanBase =
            _satanEyesOpen =
            _satanEyesClosed =
            _satanMouthClosed =
                new(Core.GraphicsDevice, 0, 0);
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void LoadContent()
    {
        _satanBase = Content.Load<Texture2D>(FilePaths.SatanMain);
        _satanEyesOpen = Content.Load<Texture2D>(FilePaths.SatanEyesOpen);
        _satanEyesClosed = Content.Load<Texture2D>(FilePaths.SatanEyesClosed);
        _satanMouthClosed = Content.Load<Texture2D>(FilePaths.SatanMouthClosed);

        _satanCenter = new(_satanBase.Width, _satanBase.Height);
        _satanPosition =
            new Vector2(Core.GraphicsDevice.Viewport.Width, Core.GraphicsDevice.Viewport.Height)
            / 2;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Core.SpriteBatch.Begin();

        Core.SpriteBatch.End();
    }
}
