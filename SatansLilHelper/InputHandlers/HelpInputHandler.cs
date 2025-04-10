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
    private LinkedList<
        Action<SpriteBatch, Dictionary<TextureID, Texture2D>, Dictionary<FontID, SpriteFont>>
    > _pageList;
    private LinkedListNode<
        Action<SpriteBatch, Dictionary<TextureID, Texture2D>, Dictionary<FontID, SpriteFont>>
    > _currentPage;
    private const int KEY_SPACING = 55;

    public HelpInputHandler(IInputHandler parent)
    {
        _parent = parent;

        _miscList =
        [
            (GameStrings.HelpI, TextureID.KeyboardI),
            (GameStrings.HelpS, TextureID.KeyboardS),
            (GameStrings.HelpF, TextureID.KeyboardF),
            //(GameStrings.HelpT, TextureID.KeyboardT),
#if DEBUG
            (GameStrings.HelpZ, TextureID.KeyboardZ),
            (GameStrings.HelpY, TextureID.KeyboardY),
#endif
            (GameStrings.HelpH, TextureID.KeyboardH),
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
        _pageList = new();
        _pageList.AddLast(DrawMovementPage);
        _pageList.AddLast(DrawMapPage);
        _currentPage = _pageList.First;
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
        spriteBatch.Draw(textureMap[TextureID.WhitePixel], shadeRectangle, Colors.AmericanRose);
        _currentPage.Value(spriteBatch, textureMap, fontMap);
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
                _currentPage = _currentPage.Previous
                    is LinkedListNode<
                        Action<
                            SpriteBatch,
                            Dictionary<TextureID, Texture2D>,
                            Dictionary<FontID, SpriteFont>
                        >
                    > prevNode
                    ? prevNode
                    : _currentPage.List.Last;
                break;
            case Keys.Right:
                _currentPage = _currentPage.Next
                    is LinkedListNode<
                        Action<
                            SpriteBatch,
                            Dictionary<TextureID, Texture2D>,
                            Dictionary<FontID, SpriteFont>
                        >
                    > nextNode
                    ? nextNode
                    : _currentPage.List.First;
                break;
        }
        return this;
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
                new Vector2(target.X + (x * offset.X), target.Y + (y * offset.Y)),
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
            (spriteBatch.GraphicsDevice.Viewport.Height / 2) - KEY_SPACING
        );
        Vector2 size = fontMap[FontID.Menu].MeasureString(GameStrings.HelpMovement);
        spriteBatch.DrawString(
            fontMap[FontID.Menu],
            GameStrings.HelpMovement,
            new(
                spriteBatch.GraphicsDevice.Viewport.Width / 2,
                (spriteBatch.GraphicsDevice.Viewport.Height / 10) + (2 * size.Y)
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

    private void DrawMapPage(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        Vector2 size = fontMap[FontID.Menu].MeasureString(GameStrings.HelpGameplay);
        spriteBatch.DrawString(
            fontMap[FontID.Menu],
            GameStrings.HelpGameplay,
            new(
                spriteBatch.GraphicsDevice.Viewport.Width / 2,
                (spriteBatch.GraphicsDevice.Viewport.Height / 10) + (2 * size.Y)
            ),
            Colors.White,
            new(size.X / 2, size.Y / 3)
        );
        Vector2 miscLocation = new(
            (spriteBatch.GraphicsDevice.Viewport.Width / 10) + KEY_SPACING,
            (spriteBatch.GraphicsDevice.Viewport.Height / 10)
                + fontMap[FontID.Messages].LineSpacing
                + (size.Y * 3.5f)
        );
        float SCALE = 0.75f;
        foreach ((string text, TextureID texture) in _miscList)
        {
            spriteBatch.Draw(
                textureMap[texture],
                miscLocation,
                null,
                Colors.White,
                0f,
                new Vector2(32),
                SCALE,
                SpriteEffects.None,
                0f
            );
            spriteBatch.DrawString(
                fontMap[FontID.Messages],
                text,
                new(miscLocation.X + 40, miscLocation.Y),
                Color.White,
                new(0, fontMap[FontID.Messages].LineSpacing / 2)
            );
            miscLocation.Y += KEY_SPACING * SCALE;
        }
    }
}
