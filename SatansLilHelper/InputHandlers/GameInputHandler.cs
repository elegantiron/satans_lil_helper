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
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.InputHandlers;

internal class GameInputHandler : IInputHandler, Interfaces.IUpdateable
{
    private IRandom _rng;
    private Point _mapSize;
    private GameWorld _gameWorld;
    private MessageLog _messageLog;
    private Rectangle _statusShadeShape;
    private VecPair _statusVecs,
        _messageLogVecs,
        _locationVecs,
        _healthVecs,
        _manaVecs;

    public BaseMap CurrentMap
    {
        get { return _gameWorld.CurrentMap; }
    }

    public GameInputHandler()
    {
        _rng = new MersenneTwister();
        _mapSize = new Point(100, 100);
        _gameWorld = new(_rng, _mapSize);
        _gameWorld.CurrentMap.UpdatePlayerVision();
        _messageLog = new();
        _statusShadeShape = Rectangle.Empty;
        _statusVecs = new();
        _locationVecs = new();
        _healthVecs = new();
        _manaVecs = new();
        _messageLogVecs = new();
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
            case Keys.S:
                return new SpellBookInputHandler(this);
            case Keys.I:
                return new InventoryInputHandler(this);
#if DEBUG
            case Keys.X:
                _gameWorld.ActionStack.AddAction(new SmiteAction(_gameWorld.CurrentMap.Player));
                break;
            case Keys.D:
                return new DebugMenuInputHandler(this);
            case Keys.Z:
                _gameWorld.ActionStack.Rewind();
                break;
            case Keys.Y:
                _gameWorld.ActionStack.Replay();
                break;
#endif
        }
        Location playerPos = _gameWorld.CurrentMap.Player.GetComponent<Location>();
        _gameWorld.CurrentMap.Camera.SetCenter(playerPos.X, playerPos.Y);
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
        if (_statusShadeShape == Rectangle.Empty)
        {
            SetVecs(spriteBatch, fontMap);
        }

        _gameWorld.CurrentMap.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);

        if (Properties.Settings.Default.ShowStatus)
        {
            spriteBatch.Draw(
                textureMap[TextureID.WhitePixel],
                _statusShadeShape,
                Colors.TranslucentBlack
            );
            DrawStatus(spriteBatch, fontMap);
            DrawMessageLog(spriteBatch, fontMap);
        }
    }

    private void SetVecs(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        _statusShadeShape.X = spriteBatch.GraphicsDevice.Viewport.Width * 4 / 5;
        _statusShadeShape.Width = spriteBatch.GraphicsDevice.Viewport.Width / 5;
        _statusShadeShape.Height = spriteBatch.GraphicsDevice.Viewport.Height;

        Vector2 textSize = fontMap[FontID.Status].MeasureString(Properties.GameStrings.StatusTitle);
        _statusVecs.Origin.X = textSize.X / 2;
        _statusVecs.Location.X = _statusShadeShape.X + _statusShadeShape.Width / 2;
        _locationVecs.Location.Y = _statusVecs.Location.Y + textSize.Y * 1.5f;
        _locationVecs.Location.X = _statusShadeShape.X + 10;
        _healthVecs.Location.X = _locationVecs.Location.X;
        _healthVecs.Location.Y = _locationVecs.Location.Y + 2 * textSize.Y;
        _manaVecs.Location.Y = _healthVecs.Location.Y + textSize.Y;
        _manaVecs.Location.X = _locationVecs.Location.X;
        _messageLogVecs.Location = _locationVecs.Location;
    }

    private void DrawStatus(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            Properties.GameStrings.StatusTitle,
            _statusVecs.Location,
            Color.White,
            0f,
            _statusVecs.Origin,
            1f,
            SpriteEffects.None,
            1f
        );
        Location playerLoc = _gameWorld.CurrentMap.Player.GetComponent<Location>();
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            String.Format(Properties.GameStrings.StatusLocation, playerLoc.X, playerLoc.Y),
            _locationVecs.Location,
            Color.White
        );
        AbilityStat playerResource = _gameWorld.CurrentMap.Player.GetRelation<
            AbilityStat,
            AbilityID
        >(AbilityID.Health);
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            String.Format(
                Properties.GameStrings.StatusHealth,
                playerResource.Cur,
                EntityCalcs.GetStat(_gameWorld.CurrentMap.Player, AbilityID.Health)
            ),
            _healthVecs.Location,
            Color.White
        );
        playerResource = _gameWorld.CurrentMap.Player.GetRelation<AbilityStat, AbilityID>(
            AbilityID.Mana
        );
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            String.Format(
                Properties.GameStrings.StatusMana,
                playerResource.Cur,
                EntityCalcs.GetStat(_gameWorld.CurrentMap.Player, AbilityID.Mana)
            ),
            _manaVecs.Location,
            Color.White
        );
    }

    private void DrawMessageLog(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        _messageLogVecs.Location.Y = spriteBatch.GraphicsDevice.Viewport.Height / 2 + 5;
        List<(string, Color)> messages = _messageLog.Messages;
        messages.Reverse();
        Vector2 textSize = Vector2.Zero;
        foreach ((string message, Color color) in messages)
        {
            List<string> wrappedText = message.Wrap(
                fontMap[FontID.Messages],
                _statusShadeShape.Width - 15
            );
            foreach (string textLine in wrappedText)
            {
                spriteBatch.DrawString(
                    fontMap[FontID.Messages],
                    textLine,
                    _messageLogVecs.Location,
                    color
                );
                textSize = fontMap[FontID.Messages].MeasureString(textLine);
                _messageLogVecs.Location.Y += textSize.Y;
                if (_messageLogVecs.Location.Y >= _statusShadeShape.Height - textSize.Y)
                    break;
            }
            if (_messageLogVecs.Location.Y >= _statusShadeShape.Height - textSize.Y)
                break;
        }
    }

    #endregion draw methods
    public void HandleMovement(Keys key)
    {
        if (!_gameWorld.CurrentMap.IsPlayerNext)
            return;
        _gameWorld.ActionStack.AddAction(
            new BumpAction(
                _gameWorld.CurrentMap.Player,
                Dicts.MovementKeys[key],
                _gameWorld.CurrentMap,
                _rng,
                true
            )
        );
        _gameWorld.CurrentMap.UpdatePlayerVision();
        _gameWorld.GetNextActor();
    }

#if DEBUG
    public void HealPlayer()
    {
        _gameWorld.ActionStack.AddAction(new FullHealAction(_gameWorld.CurrentMap.Player));
    }

    public void SpawnNear()
    {
        Location playerPos = _gameWorld.CurrentMap.Player.GetComponent<Location>();
        _gameWorld.ActionStack.AddAction(
            new SpawnAction(
                Entities.Enemies.Wolf,
                _gameWorld.CurrentMap,
                _rng,
                (playerPos.X, playerPos.Y + 1)
            )
        );
    }

    public void ResetSeed()
    {
        _rng.Seed(Environment.TickCount);
    }
#endif

    public void Update(GameTime gameTime)
    {
        (_gameWorld as IRegistry).Update(gameTime);
    }
}
