using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SatansLilHelper.Constants;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.GameComponents;

internal class TitleScreen : DrawableGameComponent
{
    private FontSystem _fontSystem;
    private SpriteFontBase _font;
    private SpriteBatch? _spriteBatch;

    public TitleScreen(Game game)
        : base(game)
    {
        FontSystemSettings fontSettings = new()
        {
            FontResolutionFactor = 4.0f,
            KernelHeight = 4,
            KernelWidth = 4,
        };
        _fontSystem = new(fontSettings);
        _fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/FairyDustB.ttf"));
        _font = _fontSystem.GetFont(125);
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        _spriteBatch = new(Game.GraphicsDevice);
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Draw(GameTime gameTime)
    {
        Game.GraphicsDevice.Clear(Color.Black);
        base.Draw(gameTime);
        Vector2 originVec = _font.MeasureString(Properties.GameStrings.GameTitle);
        _spriteBatch?.Begin();
        _spriteBatch?.DrawString(
            _font,
            Properties.GameStrings.GameTitle,
            new Vector2((Game.GraphicsDevice.Viewport.Width - originVec.X) / 2, 25),
            Color.White
        );
        _spriteBatch?.End();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
    }
}
