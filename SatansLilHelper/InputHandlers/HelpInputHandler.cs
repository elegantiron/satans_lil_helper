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
using SatansLilHelper.Properties;

namespace SatansLilHelper.InputHandlers;

internal class HelpInputHandler : IInputHandler
{
    private IInputHandler _parent;

    private List<(string, TextureID)> _miscList;
    private List<(TextureID, int, int)> _arrowList,
        _numberList;
    private List<
        Action<SpriteBatch, Dictionary<TextureID, Texture2D>, Dictionary<FontID, SpriteFont>>
    > _pageList;
    private int _page = 0;
    private const int KEY_SPACING = 55;

    public HelpInputHandler(IInputHandler parent)
    {
        _parent = parent;

        _miscList =
        [
            (GameStrings.HelpI, TextureID.KeyboardI),
            (GameStrings.HelpS, TextureID.KeyboardS),
            (GameStrings.HelpF, TextureID.KeyboardF),
            (GameStrings.HelpT, TextureID.KeyboardT),
            (GameStrings.HelpH, TextureID.KeyboardH),
#if DEBUG
            (GameStrings.HelpZ, TextureID.KeyboardZ),
            (GameStrings.HelpY, TextureID.KeyboardY),
#endif
        ];
        _arrowList =
        [
            (TextureID.KeyboardInsert, 0, 0),
            (TextureID.KeyboardUp, 1, 0),
            (TextureID.KeyboardPageUp, 2, 0),
            (TextureID.KeyboardLeft, 0, 1),
            (TextureID.KeyboardRight, 2, 1),
            (TextureID.KeyboardDelete, 0, 2),
            (TextureID.KeyboardDown, 1, 2),
            (TextureID.KeyboardPageDown, 2, 2),
        ];
        _numberList =
        [
            (TextureID.Keyboard7, 0, 0),
            (TextureID.Keyboard8, 1, 0),
            (TextureID.Keyboard9, 2, 0),
            (TextureID.Keyboard4, 0, 1),
            (TextureID.Keyboard6, 2, 1),
            (TextureID.Keyboard1, 0, 2),
            (TextureID.Keyboard2, 1, 2),
            (TextureID.Keyboard3, 2, 2),
        ];
        _pageList = [DrawMovementPage, DrawPage1];
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
        spriteBatch.Begin();

        Rectangle shadeRectangle = new(
            spriteBatch.GraphicsDevice.Viewport.Width / 10,
            spriteBatch.GraphicsDevice.Viewport.Height / 10,
            spriteBatch.GraphicsDevice.Viewport.Width * 8 / 10,
            spriteBatch.GraphicsDevice.Viewport.Height * 8 / 10
        );
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], shadeRectangle, Colors.TranslucentBlack);
        _pageList[_page](spriteBatch, textureMap, fontMap);
        Vector2 arrowPosition = new(shadeRectangle.Left + 64, shadeRectangle.Bottom - 64);
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardLeft],
            arrowPosition,
            Color.White,
            new Vector2(32)
        );
        Vector2 size = fontMap[FontID.Messages].MeasureString(GameStrings.PrevPage);
        spriteBatch.DrawString(
            fontMap[FontID.Messages],
            Properties.GameStrings.PrevPage,
            new Vector2(arrowPosition.X + KEY_SPACING, arrowPosition.Y),
            Colors.White,
            new Vector2(0, size.Y / 2)
        );
        arrowPosition.X = shadeRectangle.Right - 64;
        spriteBatch.Draw(
            textureMap[TextureID.KeyboardRight],
            arrowPosition,
            Colors.White,
            new Vector2(32)
        );
        size = fontMap[FontID.Messages].MeasureString(GameStrings.NextPage);
        spriteBatch.DrawString(
            fontMap[FontID.Messages],
            GameStrings.NextPage,
            new Vector2(arrowPosition.X - KEY_SPACING, arrowPosition.Y),
            Colors.White,
            new Vector2(size.X, size.Y / 2)
        );
        spriteBatch.End();
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.H:
            case Keys.Escape:
                return _parent;
            case Keys.Left:
                if (--_page < 0)
                    _page = _pageList.Count - 1;
                break;
            case Keys.Right:
                _page = ++_page % _pageList.Count;
                break;
        }
        return key switch
        {
            Keys.Escape => _parent,
            Keys.H => _parent,
            _ => this,
        };
    }

    private void DrawKeySquare(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        List<(TextureID, int, int)> itemList,
        Vector2 target,
        Vector2 offset
    )
    {
        foreach ((TextureID texture, int x, int y) in itemList)
        {
            spriteBatch.Draw(
                textureMap[texture],
                new Vector2(target.X + x * offset.X, target.Y + y * offset.Y),
                Colors.White,
                new(32, 32)
            );
        }
    }

    private void DrawMovementPage(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        Vector2 keyTarget = new(
            spriteBatch.GraphicsDevice.Viewport.Width / 3,
            spriteBatch.GraphicsDevice.Viewport.Height / 2 - KEY_SPACING
        );
        Vector2 size = fontMap[FontID.Menu].MeasureString(GameStrings.HelpMovement);
        spriteBatch.DrawString(
            fontMap[FontID.Menu],
            GameStrings.HelpMovement,
            new(
                spriteBatch.GraphicsDevice.Viewport.Width / 2,
                spriteBatch.GraphicsDevice.Viewport.Height / 10 + 2 * size.Y
            ),
            Colors.White,
            new(size.X / 2, size.Y / 3)
        );

        Vector2 keyOffset = new(KEY_SPACING);

        DrawKeySquare(spriteBatch, textureMap, _arrowList, keyTarget, keyOffset);

        keyTarget.X *= 2;
        keyTarget.X -= 2 * KEY_SPACING;

        DrawKeySquare(spriteBatch, textureMap, _numberList, keyTarget, keyOffset);
        size = fontMap[FontID.Menu].MeasureString(GameStrings.HelpOr);
        spriteBatch.DrawString(
            fontMap[FontID.Menu],
            GameStrings.HelpOr,
            new Vector2(
                spriteBatch.GraphicsDevice.Viewport.Width / 2,
                spriteBatch.GraphicsDevice.Viewport.Height / 2
            ),
            Colors.White,
            size / 2
        );
    }

    private void DrawPage1(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        Vector2 miscLocation = new(
            spriteBatch.GraphicsDevice.Viewport.Width / 10 + 5.5f * KEY_SPACING,
            spriteBatch.GraphicsDevice.Viewport.Height / 10
                + fontMap[FontID.Messages].LineSpacing
                + KEY_SPACING
        );
        foreach ((string text, TextureID texture) in _miscList)
        {
            spriteBatch.Draw(textureMap[texture], miscLocation, Colors.White, new(32));
            spriteBatch.DrawString(
                fontMap[FontID.Messages],
                text,
                new(miscLocation.X + 40, miscLocation.Y),
                Color.White,
                new(0, fontMap[FontID.Messages].LineSpacing / 2)
            );
            miscLocation.Y += 55;
        }
    }

    private void DrawPage2(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<FontID, SpriteFont> fontMap
    ) { }

    private void DrawPage3(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<FontID, SpriteFont> fontMap
    ) { }
}
