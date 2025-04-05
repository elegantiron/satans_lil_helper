using System;
using System.Collections.Generic;
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
internal class ConfirmInputHandler : IInputHandler
{
    private string _message;
    private Rectangle _shadeShape = Rectangle.Empty;
    private Vector2 _messageLocation = Vector2.Zero;
    private FontID _fontID;
    private Action<bool> _callback;
    private IInputHandler _parent;
    private Vector2 _inputGraphicsOrigin,
        _enterGraphicLocation;

    public ConfirmInputHandler(
        IInputHandler parent,
        string message,
        FontID fontID,
        Action<bool> callback
    )
    {
        _message = message;
        _fontID = fontID;
        _callback = callback;
        _parent = parent;
        _inputGraphicsOrigin = new(0, 64);
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        List<string> messageWrapped = _message.Wrap(
            fontMap[_fontID],
            spriteBatch.GraphicsDevice.Viewport.Width / 4
        );
        float messageHeight = messageWrapped.Count * fontMap[_fontID].LineSpacing;
        if (_messageLocation == Vector2.Zero)
            SetVecs(spriteBatch, messageHeight);
        _messageLocation.Y = (spriteBatch.GraphicsDevice.Viewport.Height - messageHeight) / 2;
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], _shadeShape, Colors.TranslucentBlack);
        foreach (string text in messageWrapped)
        {
            Vector2 size = fontMap[_fontID].MeasureString(text);
            Vector2 origin = new(size.X / 2.0f, 0);
            spriteBatch.DrawString(
                fontMap[_fontID],
                text,
                _messageLocation,
                Colors.White,
                0f,
                origin,
                1f,
                SpriteEffects.None,
                1f
            );
            _messageLocation.Y += fontMap[_fontID].LineSpacing;
        }
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardReturn],
            _enterGraphicLocation,
            Colors.White,
            _inputGraphicsOrigin
        );
    }

    private void SetVecs(SpriteBatch spriteBatch, float messageHeight)
    {
        _shadeShape.X = _shadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width / 3;
        _shadeShape.Y = _shadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height / 3;
        _messageLocation.X = spriteBatch.GraphicsDevice.Viewport.Width / 2;
        _enterGraphicLocation = new(_shadeShape.X + 25, _shadeShape.Y + _shadeShape.Height - 20);
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                _callback(false);
                return _parent;
            case Keys.Enter:
                _callback(true);
                return _parent;
        }
        return this;
    }
}
