using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

public class TitleInputHandler : IInputHandler
{
    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<MusicID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        throw new System.NotImplementedException();
    }

    public IInputHandler HandleKey(Keys key)
    {
        throw new System.NotImplementedException();
    }
}
