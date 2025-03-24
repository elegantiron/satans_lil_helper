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
using MonoGame.Extended;
using MonoGame.Extended.Particles;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

public class TestInputHandler : IInputHandler
{
    private ParticleEffect _particleEffect;

    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    ) { }

    public IInputHandler HandleKey(Keys key)
    {
        return this;
    }

    public void Update(GameTime gameTime) { }
}
