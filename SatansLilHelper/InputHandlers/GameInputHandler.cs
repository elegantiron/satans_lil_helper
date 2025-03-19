using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Actions;
using SatansLilHelper.Components;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

public class GameInputHandler : IInputHandler
{
    protected Random rng;
    protected Point mapSize;
    protected GameWorld GameWorld;
    protected ActionStack ActionStack;

    public GameInputHandler()
    {
        rng = new Random();
        mapSize = new Point(100, 100);
        GameWorld = new(rng, mapSize);
        ActionStack = new();
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
        if (!GameWorld.CurrentMap.Player.TryGetComponent<ActionDelay>(out ActionDelay playerDelay))
            return;
        if (playerDelay.value != 0)
        {
            // TODO tick entities' ActionDelays
        }
        else
        {
            try
            {
                ActionStack.AddAction(
                    new MoveAction(
                        GameWorld.CurrentMap.Player,
                        Constants.MovementKeys[key],
                        GameWorld.CurrentMap,
                        GameWorld.CurrentMap.GetBlockingEntities,
                        true
                    )
                );
            }
            catch (PathBlockedException exception) { }
            Location playerPos = GameWorld.CurrentMap.Player.GetComponent<Location>();
            GameWorld.CurrentMap.Camera.SetCenter(playerPos.X, playerPos.Y);
        }
    }
}
