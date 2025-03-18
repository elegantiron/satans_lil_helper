using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Components;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils;

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
            case Keys when Constants.MovementKeys.ContainsKey(key):
                HandleMovement(key);
                break;
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

    public void HandleMovement(Keys key)
    {
        ActionDelay playerDelay;
        if (!GameWorld.CurrentMap.Player.TryGetComponent<ActionDelay>(out playerDelay))
            return;
        if (playerDelay.value != 0) { }
    }
}
