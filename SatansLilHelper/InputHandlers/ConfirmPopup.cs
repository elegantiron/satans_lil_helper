using System.Collections.Generic;
using System.IO;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Extensions;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.InputHandlers;

#nullable enable
internal class ConfirmPopup : Interfaces.IDrawable
{
    private string _message;
    private Rectangle _shadeShape = Rectangle.Empty;
    private Vector2 _messageLocation = Vector2.Zero;
    private FontSystem _fontSystem;
    private DynamicSpriteFont _font;

    public ConfirmPopup(string message)
    {
        _message = message;
        _fontSystem = new();
        _fontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/CrayonLibre.ttf"));
        _font = _fontSystem.GetFont(20f);
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (_messageLocation == Vector2.Zero)
            SetVecs(spriteBatch);
        _messageLocation.Y = _shadeShape.Y + 10;
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], _shadeShape, Colors.TranslucentBlack);
        foreach (
            string text in Properties.GameStrings.ConfirmTurnEnd.Wrap(
                fontMap[FontID.Messages],
                spriteBatch.GraphicsDevice.Viewport.Width / 4
            )
        )
        {
            Vector2 size = fontMap[FontID.Messages].MeasureString(text);
            Vector2 origin = new Vector2(size.X / 2.0f, 0);
            spriteBatch.DrawString(
                fontMap[FontID.Messages],
                text,
                _messageLocation,
                Colors.White,
                0f,
                origin,
                1f,
                SpriteEffects.None,
                1f
            );
            _messageLocation.Y += fontMap[FontID.Messages].LineSpacing;
        }
    }

    private void SetVecs(SpriteBatch spriteBatch)
    {
        _shadeShape.X = _shadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width / 3;
        _shadeShape.Y = _shadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height / 3;
        _messageLocation.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
        _messageLocation.Y = _shadeShape.Y + 10;
    }
}
