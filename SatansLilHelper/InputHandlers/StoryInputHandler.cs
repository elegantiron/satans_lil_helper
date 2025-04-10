using System.Collections.Generic;
using Apos.Camera;
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
        Camera camera,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        _parent.Draw(spriteBatch, camera, textureMap, effectMap, songMap, fontMap);
    }

    public IInputHandler HandleKey(Keys key)
    {
        return key == Keys.Enter ? _parent : this;
    }

    public void Update(Microsoft.Xna.Framework.GameTime gameTime) { }
}
