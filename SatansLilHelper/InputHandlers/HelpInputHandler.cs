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
using SatansLilHelper.Extensions;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;

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
        _pageDownLocation,
        _1location,
        _2location,
        _3location,
        _4location,
        _6location,
        _7location,
        _8location,
        _9location,
        _movementHeaderLocation,
        _movementHeaderOrigin,
        _movementOrLocation,
        _movementOrOrigin,
        _textureOrigin,
        _miscTextOrigin;
    private List<(string, TextureID)> _miscList;

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
            _1location =
            _2location =
            _3location =
            _4location =
            _6location =
            _7location =
            _8location =
            _9location =
            _movementHeaderLocation =
            _movementHeaderOrigin =
            _movementOrLocation =
            _movementOrOrigin =
            _miscTextOrigin =
                Vector2.Zero;
        _textureOrigin = new(32f, 32f);

        _miscList =
        [
            (GameStrings.HelpF, TextureID.KeyboardF),
            (GameStrings.HelpH, TextureID.KeyboardH),
            (GameStrings.HelpI, TextureID.KeyboardI),
            (GameStrings.HelpS, TextureID.KeyboardS),
            (GameStrings.HelpT, TextureID.KeyboardT),
        ];
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
            SetVecs(spriteBatch, fontMap);
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], _shadeShape, Colors.TranslucentBlack);
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardInsert],
            _insertLocation,
            Colors.White,
            _textureOrigin
        );
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardLeft],
            _leftArrowLocation,
            Colors.White,
            _textureOrigin
        );
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardDelete],
            _deleteLocation,
            Colors.White,
            _textureOrigin
        );
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardUp],
            _upArrowLocation,
            Colors.White,
            _textureOrigin
        );
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardDown],
            _downArrowLocation,
            Colors.White,
            _textureOrigin
        );
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardPageUp],
            _pageUpLocation,
            Colors.White,
            _textureOrigin
        );
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardRight],
            _rightArrowLocation,
            Colors.White,
            _textureOrigin
        );
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardPageDown],
            _pageDownLocation,
            Colors.White,
            _textureOrigin
        );
        spriteBatch.Draw(textureMap[TextureID.Keyboard1], _1location, Colors.White, _textureOrigin);
        spriteBatch.Draw(textureMap[TextureID.Keyboard2], _2location, Colors.White, _textureOrigin);
        spriteBatch.Draw(textureMap[TextureID.Keyboard3], _3location, Colors.White, _textureOrigin);
        spriteBatch.Draw(textureMap[TextureID.Keyboard4], _4location, Colors.White, _textureOrigin);
        spriteBatch.Draw(textureMap[TextureID.Keyboard6], _6location, Colors.White, _textureOrigin);
        spriteBatch.Draw(textureMap[TextureID.Keyboard7], _7location, Colors.White, _textureOrigin);
        spriteBatch.Draw(textureMap[TextureID.Keyboard8], _8location, Colors.White, _textureOrigin);
        spriteBatch.Draw(textureMap[TextureID.Keyboard9], _9location, Colors.White, _textureOrigin);

        spriteBatch.DrawString(
            fontMap[FontID.Messages],
            Properties.GameStrings.HelpMovement,
            _movementHeaderLocation,
            Colors.White,
            _movementHeaderOrigin
        );
        spriteBatch.DrawString(
            fontMap[FontID.Messages],
            Properties.GameStrings.HelpOr,
            _movementOrLocation,
            Colors.White,
            _movementOrOrigin
        );
        Vector2 miscLocation = new(_9location.X + 96, _pageUpLocation.Y);
        foreach ((string text, TextureID texture) in _miscList)
        {
            spriteBatch.Draw(textureMap[texture], miscLocation, Colors.White, _textureOrigin);
            spriteBatch.DrawString(
                fontMap[FontID.Messages],
                text,
                new(miscLocation.X + 40, miscLocation.Y),
                Color.White,
                _miscTextOrigin
            );
            miscLocation.Y += 55;
        }
    }

    public IInputHandler HandleKey(Keys key)
    {
#if DEBUG
        if (key == Keys.H)
            _shadeShape = Rectangle.Empty;
#endif
        return key switch
        {
            Keys.Escape => _parent,
            Keys.H => _parent,
            _ => this,
        };
    }

    private void SetVecs(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        Vector2 size = fontMap[FontID.Messages].MeasureString(Properties.GameStrings.HelpMovement);
        _movementHeaderOrigin.X = size.X / 2;
        _miscTextOrigin.Y = fontMap[FontID.Messages].LineSpacing / 2;
        size = fontMap[FontID.Messages].MeasureString(Properties.GameStrings.HelpOr);
        _movementOrOrigin.X = size.X / 2;
        _shadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width / 10;
        _shadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width * 8 / 10;
        _shadeShape.Y = spriteBatch.GraphicsDevice.Viewport.Height / 10;
        _shadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height * 8 / 10;
        _movementHeaderLocation.Y = _shadeShape.Y + 45;
        _movementHeaderOrigin.Y = _movementOrOrigin.Y = fontMap[FontID.Messages].LineSpacing * 0.5f;
        _insertLocation.X =
            _leftArrowLocation.X =
            _deleteLocation.X =
            _1location.X =
            _4location.X =
            _7location.X =
                spriteBatch.GraphicsDevice.Viewport.Width * 5 / 30;
        _insertLocation.Y =
            _upArrowLocation.Y =
            _pageUpLocation.Y =
                _movementHeaderLocation.Y + fontMap[FontID.Messages].LineSpacing * 1.5f;
        _leftArrowLocation.Y = _rightArrowLocation.Y = _insertLocation.Y + 55;
        _deleteLocation.Y = _downArrowLocation.Y = _pageDownLocation.Y = _leftArrowLocation.Y + 55;
        _movementOrLocation.Y =
            _deleteLocation.Y + fontMap[FontID.Messages].LineSpacing * 0.75f + 28;
        _upArrowLocation.X =
            _downArrowLocation.X =
            _2location.X =
            _8location.X =
                _insertLocation.X + 55;
        _movementHeaderLocation.X = _movementOrLocation.X = _upArrowLocation.X;
        _pageUpLocation.X =
            _rightArrowLocation.X =
            _pageDownLocation.X =
            _3location.X =
            _6location.X =
            _9location.X =
                _downArrowLocation.X + 55;
        _7location.Y =
            _8location.Y =
            _9location.Y =
                _movementOrLocation.Y + fontMap[FontID.Messages].LineSpacing * 1.75f;
        _4location.Y = _6location.Y = _7location.Y + 55;
        _1location.Y = _2location.Y = _3location.Y = _4location.Y + 55;
    }
}
