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

    private List<(string, TextureID)> _miscList;
    private List<(TextureID, int, int)> _arrowList,
        _numberList;
    private int _page = 0;

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

        spriteBatch.Draw(
            textureMap[TextureID.WhitePixel],
            new Rectangle(
                spriteBatch.GraphicsDevice.Viewport.Width / 10,
                spriteBatch.GraphicsDevice.Viewport.Height / 10,
                spriteBatch.GraphicsDevice.Viewport.Width * 8 / 10,
                spriteBatch.GraphicsDevice.Viewport.Height * 8 / 10
            ),
            Colors.AmericanRose
        );
        switch (_page)
        {
            case 0:
                DrawPage0(spriteBatch, textureMap, fontMap);
                break;
        }
        spriteBatch.End();
    }

    private void DrawPage0(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        int KEY_SPACING = 55;
        Vector2 keyTarget = new(
            spriteBatch.GraphicsDevice.Viewport.Width / 10 + 1.5f * KEY_SPACING,
            spriteBatch.GraphicsDevice.Viewport.Height / 10
                + fontMap[FontID.Messages].LineSpacing
                + KEY_SPACING
        );
        Vector2 size = fontMap[FontID.Messages].MeasureString(GameStrings.HelpMovement);
        spriteBatch.DrawString(
            fontMap[FontID.Messages],
            GameStrings.HelpMovement,
            new(
                keyTarget.X + KEY_SPACING,
                spriteBatch.GraphicsDevice.Viewport.Height / 10
                    + fontMap[FontID.Messages].LineSpacing
            ),
            Colors.White,
            new(size.X / 2, size.Y / 3)
        );
        size = fontMap[FontID.Messages].MeasureString(GameStrings.HelpOr);
        spriteBatch.DrawString(
            fontMap[FontID.Messages],
            GameStrings.HelpOr,
            new(keyTarget.X + KEY_SPACING, keyTarget.Y + 3 * KEY_SPACING),
            Colors.White,
            size / 2
        );

        Vector2 keyOffset = new(KEY_SPACING);

        DrawKeySquare(spriteBatch, textureMap, _arrowList, keyTarget, keyOffset);

        Vector2 miscLocation = new(keyTarget.X + 4 * KEY_SPACING, keyTarget.Y);
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

        keyTarget.Y += 3 * KEY_SPACING + fontMap[FontID.Messages].LineSpacing * 2;

        DrawKeySquare(spriteBatch, textureMap, _numberList, keyTarget, keyOffset);
    }

    public IInputHandler HandleKey(Keys key)
    {
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
}
