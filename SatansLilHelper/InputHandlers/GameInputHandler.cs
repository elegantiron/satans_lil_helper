using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Constants;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils;
using System;
using System.Collections.Generic;

namespace SatansLilHelper.InputHandlers;


public class GameInputHandler : IInputHandler
{
    protected Random rng;
    protected Point mapSize;
    protected GameWorld GameWorld;

    public GameInputHandler()
    {
        rng = new Random();
        mapSize = new Point(100, 100);
        GameWorld = new(rng, mapSize);
    }

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
        GameWorld.CurrentMap.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
    }
}
