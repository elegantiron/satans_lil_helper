using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;

namespace SatansLilHelper.InputHandlers;

internal class PopupInputHandler(IInputHandler parent, string message) : IInputHandler
{
    IInputHandler _parent = parent;
    string _message = message;

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        _parent.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
    }

    public IInputHandler HandleKey(Keys key)
    {
        return key switch
        {
            Keys.Escape or Keys.Enter => _parent,
            _ => this,
        };
    }
}
