using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Content.Text;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

public class TitleInputHandler : IInputHandler
{
    private Vector2 titlePosition = Vector2.Zero;

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<MusicID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        spriteBatch.DrawString(
            fontMap[FontID.Title],
            GameStrings.GameTitle,
            titlePosition,
            Constants.Colors.AmericanRose
        );
    }

    public IInputHandler HandleKey(Keys key)
    {
        throw new System.NotImplementedException();
    }
}
