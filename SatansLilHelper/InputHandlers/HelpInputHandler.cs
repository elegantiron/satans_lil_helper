using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.InputHandlers;

internal class HelpInputHandler : IInputHandler
{
    private IInputHandler _parent;
    private Rectangle _shadeShape;
    private Vector2 _upArrowLocation,
        _downArrowLocation,
        _leftArrowLocation,
        _rightArrowLocation,
        _insertLocation,
        _deleteLocation,
        _pageUpLocation,
        _pageDownLocation;

    public HelpInputHandler(IInputHandler parent)
    {
        _shadeShape = Rectangle.Empty;
        _parent = parent;
        _upArrowLocation =
            _downArrowLocation =
            _leftArrowLocation =
            _rightArrowLocation =
            _insertLocation =
            _deleteLocation =
            _pageUpLocation =
            _pageDownLocation =
                Vector2.Zero;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (_shadeShape == Rectangle.Empty)
            SetVecs(spriteBatch);
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], _shadeShape, Colors.TranslucentBlack);
        spriteBatch.Draw(textureMap[TextureID.KeyboardInsert], _insertLocation, Colors.White);
        spriteBatch.Draw(textureMap[TextureID.KeyboardLeft], _leftArrowLocation, Colors.White);
        spriteBatch.Draw(textureMap[TextureID.KeyboardDelete], _deleteLocation, Colors.White);
        spriteBatch.Draw(textureMap[TextureID.KeyboardUp], _upArrowLocation, Colors.White);
        spriteBatch.Draw(textureMap[TextureID.KeyboardDown], _downArrowLocation, Colors.White);
        spriteBatch.Draw(textureMap[TextureID.KeyboardPageUp], _pageUpLocation, Colors.White);
        spriteBatch.Draw(textureMap[TextureID.KeyboardRight], _rightArrowLocation, Colors.White);
        spriteBatch.Draw(textureMap[TextureID.KeyboardPageDown], _pageDownLocation, Colors.White);
    }

    public IInputHandler HandleKey(Keys key)
    {
        return this;
    }

    private void SetVecs(SpriteBatch spriteBatch)
    {
        _shadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width / 10;
        _shadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width * 8 / 10;
        _shadeShape.Y = spriteBatch.GraphicsDevice.Viewport.Height / 10;
        _shadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height * 8 / 10;
        _insertLocation.X =
            _leftArrowLocation.X =
            _deleteLocation.X =
                spriteBatch.GraphicsDevice.Viewport.Width / 5;
        _insertLocation.Y =
            _upArrowLocation.Y =
            _pageUpLocation.Y =
                spriteBatch.GraphicsDevice.Viewport.Height / 5;
        _leftArrowLocation.Y = _rightArrowLocation.Y = _insertLocation.Y + 64;
        _deleteLocation.Y = _downArrowLocation.Y = _pageDownLocation.Y = _leftArrowLocation.Y + 64;
        _upArrowLocation.X = _downArrowLocation.X = _insertLocation.X + 64;
        _pageUpLocation.X = _rightArrowLocation.X = _pageDownLocation.X = _downArrowLocation.X + 64;
    }
}
