using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.InputHandlers;

internal class StoryInputHandler(IInputHandler parent, FontID font, JournalID entry)
    : Interfaces.IDrawable,
        IInputHandler,
        IUpdateable
{
    private IInputHandler _parent = parent;
    private FontID _font = font;
    private JournalID _entry = entry;

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
        if (key == Keys.Enter)
            return _parent;
        return this;
    }

    public void Update(Microsoft.Xna.Framework.GameTime gameTime) { }
}
