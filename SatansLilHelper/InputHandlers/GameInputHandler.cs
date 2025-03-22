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
using SatansLilHelper.Content.Text;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

public class GameInputHandler : IInputHandler
{
    protected MersenneTwister rng;
    protected Point mapSize;
    protected GameWorld GameWorld;
    protected ActionStack ActionStack;
    protected MessageLog MessageLog;
    protected Rectangle StatusShadeShape;
    private TextVecs StatusVecs,
        LocationVecs,
        HealthVecs,
        ManaVecs;
    private bool ShowStatus = true;

    public GameInputHandler()
    {
        rng = new();
        mapSize = new Point(100, 100);
        GameWorld = new(rng, mapSize);
        ActionStack = new();
        MessageLog = new();
        StatusShadeShape = Rectangle.Empty;
        StatusVecs = new();
        LocationVecs = new();
        HealthVecs = new();
        ManaVecs = new();
    }

    public IInputHandler HandleKey(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                return new PauseInputHandler(this);
            case Keys when Constants.MovementKeys.ContainsKey(key):
                HandleMovement(key);
                break;
            case Keys.H:
                ShowStatus = !ShowStatus;
                break;
            case Keys.I:
                // This will show the inventory
                break;
            case Keys.C:
                // This will show the character screen
                break;
            case Keys.M:
                // This will show castable magic
                break;
            case Keys.X:
                GameWorld
                    .CurrentMap.Player.GetRelation<ResourceStat, ResourceID>(ResourceID.Health)
                    .Cur--;
                break;
            case Keys.D:
                return new DebugMenuInputHandler(this);
        }
        return this;
    }

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
            StatusShadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width * 4 / 5;
            StatusShadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width / 5;
            StatusShadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height;

            Vector2 textSize = fontMap[FontID.Status].MeasureString(GameStrings.StatusTitle);
            StatusVecs.Origin.X = textSize.X / 2;
            StatusVecs.Location.X = StatusShadeShape.X + StatusShadeShape.Width / 2;
            LocationVecs.Location.Y = StatusVecs.Location.Y + textSize.Y * 1.5f;
            LocationVecs.Location.X = StatusShadeShape.X + 15;
            HealthVecs.Location.X = LocationVecs.Location.X;
            HealthVecs.Location.Y = LocationVecs.Location.Y + 2 * textSize.Y;
            ManaVecs.Location.Y = HealthVecs.Location.Y + textSize.Y;
            ManaVecs.Location.X = LocationVecs.Location.X;
        }
        GameWorld.CurrentMap.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);
        if (ShowStatus)
        {
            spriteBatch.Draw(
                textureMap[TextureID.WhitePixel],
                StatusShadeShape,
                Constants.Colors.TranslucentBlack
            );
            spriteBatch.DrawString(
                fontMap[FontID.Status],
                GameStrings.StatusTitle,
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
                String.Format(GameStrings.StatusLocation, playerLoc.X, playerLoc.Y),
                LocationVecs.Location,
                Color.White
            );
            ResourceStat playerResource = GameWorld.CurrentMap.Player.GetRelation<
                ResourceStat,
                ResourceID
            >(ResourceID.Health);
            spriteBatch.DrawString(
                fontMap[FontID.Status],
                String.Format(GameStrings.StatusHealth, playerResource.Cur, playerResource.Basis),
                HealthVecs.Location,
                Color.White
            );
            playerResource = GameWorld.CurrentMap.Player.GetRelation<ResourceStat, ResourceID>(
                ResourceID.Mana
            );
            spriteBatch.DrawString(
                fontMap[FontID.Status],
                String.Format(GameStrings.StatusMana, playerResource.Cur, playerResource.Basis),
                ManaVecs.Location,
                Color.White
            );
        }
    }

    public void HandleMovement(Keys key)
    {
        if (!GameWorld.CurrentMap.Player.TryGetComponent<ActionDelay>(out ActionDelay playerDelay))
            return;
        if (playerDelay.Value != 0)
        {
            return;
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
            catch (PathBlockedException exception)
            {
                MessageLog.AddMessage(exception.Message, Constants.Colors.Impossible);
            }
            Location playerPos = GameWorld.CurrentMap.Player.GetComponent<Location>();
            GameWorld.CurrentMap.Camera.SetCenter(playerPos.X, playerPos.Y);
        }
    }

    public void HealPlayer()
    {
        int maxHealth = EntityCalcs.GetStat(GameWorld.CurrentMap.Player, ResourceID.Health);
        GameWorld.CurrentMap.Player.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur =
            maxHealth;
    }

    public void SpawnNear() { }

    public void SpawnRandom() { }
}
