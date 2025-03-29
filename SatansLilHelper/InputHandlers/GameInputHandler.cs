using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Actions;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Extensions;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal class GameInputHandler : IInputHandler, Interfaces.IUpdateable
{
    protected IRandom rng;
    protected Point mapSize;
    protected GameWorld GameWorld;
    protected ActionStack ActionStack;
    protected MessageLog MessageLog;
    protected Rectangle StatusShadeShape;
    private VecPair StatusVecs,
        MessageLogVecs,
        LocationVecs,
        HealthVecs,
        ManaVecs;

    public GameInputHandler()
    {
        rng = new MersenneTwister();
        mapSize = new Point(100, 100);
        GameWorld = new(rng, mapSize);
        GameWorld.CurrentMap.UpdatePlayerVision();
        ActionStack = new();
        MessageLog = new();
        StatusShadeShape = Rectangle.Empty;
        StatusVecs = new();
        LocationVecs = new();
        HealthVecs = new();
        ManaVecs = new();
        MessageLogVecs = new();
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                return new PauseInputHandler(this);
            case Keys when Dicts.MovementKeys.ContainsKey(key):
                HandleMovement(key);
                break;
            case Keys.H:
                Properties.Settings.Default.ShowStatus = !Properties.Settings.Default.ShowStatus;
                break;
#if DEBUG
            case Keys.X:
                ActionStack.AddAction(new SmiteAction(GameWorld.CurrentMap.Player));
                break;
            case Keys.D:
                return new DebugMenuInputHandler(this);
            case Keys.Z:
                ActionStack.Rewind();
                break;
            case Keys.Y:
                ActionStack.Replay();
                break;
#endif
        }
        Location playerPos = GameWorld.CurrentMap.Player.GetComponent<Location>();
        GameWorld.CurrentMap.Camera.SetCenter(playerPos.X, playerPos.Y);
        return this;
    }

    #region Draw methods
    public void Draw(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<EffectID, SoundEffect> effectMap,
        Dictionary<SongID, Song> songMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        if (StatusShadeShape == Rectangle.Empty)
        {
            SetVecs(spriteBatch, fontMap);
        }

        GameWorld.CurrentMap.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);

        if (Properties.Settings.Default.ShowStatus)
        {
            spriteBatch.Draw(
                textureMap[TextureID.WhitePixel],
                StatusShadeShape,
                Colors.TranslucentBlack
            );
            DrawStatus(spriteBatch, textureMap, fontMap);
            DrawMessageLog(spriteBatch, fontMap);
        }
    }

    private void SetVecs(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        StatusShadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width * 4 / 5;
        StatusShadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width / 5;
        StatusShadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height;

        Vector2 textSize = fontMap[FontID.Status].MeasureString(Properties.GameStrings.StatusTitle);
        StatusVecs.Origin.X = textSize.X / 2;
        StatusVecs.Location.X = StatusShadeShape.X + StatusShadeShape.Width / 2;
        LocationVecs.Location.Y = StatusVecs.Location.Y + textSize.Y * 1.5f;
        LocationVecs.Location.X = StatusShadeShape.X + 10;
        HealthVecs.Location.X = LocationVecs.Location.X;
        HealthVecs.Location.Y = LocationVecs.Location.Y + 2 * textSize.Y;
        ManaVecs.Location.Y = HealthVecs.Location.Y + textSize.Y;
        ManaVecs.Location.X = LocationVecs.Location.X;
        MessageLogVecs.Location = LocationVecs.Location;
    }

    private void DrawStatus(
        SpriteBatch spriteBatch,
        Dictionary<TextureID, Texture2D> textureMap,
        Dictionary<FontID, SpriteFont> fontMap
    )
    {
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            Properties.GameStrings.StatusTitle,
            StatusVecs.Location,
            Color.White,
            0f,
            StatusVecs.Origin,
            1f,
            SpriteEffects.None,
            1f
        );
        Location playerLoc = GameWorld.CurrentMap.Player.GetComponent<Location>();
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            String.Format(Properties.GameStrings.StatusLocation, playerLoc.X, playerLoc.Y),
            LocationVecs.Location,
            Color.White
        );
        AbilityStat playerResource = GameWorld.CurrentMap.Player.GetRelation<
            AbilityStat,
            AbilityID
        >(AbilityID.Health);
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            String.Format(
                Properties.GameStrings.StatusHealth,
                playerResource.Cur,
                EntityCalcs.GetStat(GameWorld.CurrentMap.Player, AbilityID.Health)
            ),
            HealthVecs.Location,
            Color.White
        );
        playerResource = GameWorld.CurrentMap.Player.GetRelation<AbilityStat, AbilityID>(
            AbilityID.Mana
        );
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            String.Format(
                Properties.GameStrings.StatusMana,
                playerResource.Cur,
                EntityCalcs.GetStat(GameWorld.CurrentMap.Player, AbilityID.Mana)
            ),
            ManaVecs.Location,
            Color.White
        );
    }

    private void DrawMessageLog(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        MessageLogVecs.Location.Y = spriteBatch.GraphicsDevice.Viewport.Height / 2 + 5;
        List<(string, Color)> messages = MessageLog.Messages;
        messages.Reverse();
        Vector2 textSize = Vector2.Zero;
        foreach ((string message, Color color) in messages)
        {
            List<string> wrappedText = message.Wrap(
                fontMap[FontID.Messages],
                StatusShadeShape.Width - 15
            );
            foreach (string textLine in wrappedText)
            {
                spriteBatch.DrawString(
                    fontMap[FontID.Messages],
                    textLine,
                    MessageLogVecs.Location,
                    color
                );
                textSize = fontMap[FontID.Messages].MeasureString(textLine);
                MessageLogVecs.Location.Y += textSize.Y;
                if (MessageLogVecs.Location.Y >= StatusShadeShape.Height - textSize.Y)
                    break;
            }
            if (MessageLogVecs.Location.Y >= StatusShadeShape.Height - textSize.Y)
                break;
        }
    }

    #endregion draw methods
    public void HandleMovement(Keys key)
    {
        if (!GameWorld.CurrentMap.IsPlayerNext)
            return;
        ActionStack.AddAction(
            new BumpAction(
                GameWorld.CurrentMap.Player,
                Dicts.MovementKeys[key],
                GameWorld.CurrentMap,
                rng,
                true
            )
        );
        GameWorld.CurrentMap.UpdatePlayerVision();
        GameWorld.GetNextActor();
    }

#if DEBUG
    public void HealPlayer()
    {
        ActionStack.AddAction(new FullHealAction(GameWorld.CurrentMap.Player));
    }

    public void SpawnNear()
    {
        Location playerPos = GameWorld.CurrentMap.Player.GetComponent<Location>();
        ActionStack.AddAction(
            new SpawnAction(
                Entities.Enemies.Wolf,
                GameWorld.CurrentMap,
                rng,
                (playerPos.X, playerPos.Y + 1)
            )
        );
    }

    public void ResetSeed()
    {
        rng.Seed(Environment.TickCount);
    }
#endif

    public void Update(GameTime gameTime)
    {
        (GameWorld as IRegistry).Update(gameTime);
    }
}
