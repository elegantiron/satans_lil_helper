using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;

namespace SatansLilHelper.InputHandlers;

internal class SpellBookInputHandler(IInputHandler parent) : IInputHandler
{
    private IInputHandler _parent = parent;
    private VecPair _titleVecs = new();
    private Rectangle _shadeShape = Rectangle.Empty;

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (_titleVecs.Location == Vector2.Zero)
        {
            CalculateVectors(spriteBatch, fontMap);
        }
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], _shadeShape, Colors.TranslucentBlack);
        spriteBatch.DrawString(
            fontMap[FontID.Menu],
            GameStrings.SpellBook,
            _titleVecs.Location,
            Colors.White,
            0f,
            _titleVecs.Origin,
            1f,
            SpriteEffects.None,
            0f
        );
    }

    public IInputHandler HandleKey(Keys key)
    {
        return key switch
        {
            Keys.S => _parent,
            Keys.Escape => _parent,
            _ => this,
        };
    }

    private void CalculateVectors(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        _titleVecs.Location.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
        _titleVecs.Origin.X = fontMap[FontID.Menu].MeasureString(GameStrings.SpellBook).X / 2;
        _shadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width / 8;
        _shadeShape.Y = spriteBatch.GraphicsDevice.Viewport.Height / 8;
        _shadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width * 6 / 8;
        _shadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height * 6 / 8;
        _titleVecs.Location.Y = _shadeShape.Y + 10;
    }
}
