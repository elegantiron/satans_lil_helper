using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class SettingsInputHandler(IInputHandler parent) : IInputHandler
{
    private IInputHandler _parent = parent;
    private Menu Menu = new(Color.Blue, Color.White, FontID.Menu);

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        Menu.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                return _parent;
            case Keys.Up:
            case Keys.Down:
                Menu.HandleKey(key);
                break;
        }
        return this;
    }
}
