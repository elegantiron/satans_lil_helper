using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Utils.GameMaps;

public class ForestMap(Point mapSize, Random rng) : BaseMap(mapSize, rng)
{
    public override void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<MusicID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    ) { }

    public override void GenerateMap(Random rng, Point size) { }
}
