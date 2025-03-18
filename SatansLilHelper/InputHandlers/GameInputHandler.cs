using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.InputHandlers;

class GameInputHandler : IInputHandler
{
    protected List<BaseMap> maps;

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                throw new GameExitException();
        }
        return this;
    }

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<MusicID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            "This is some test text",
            Vector2.Zero,
            Color.White
        );
    }
}
