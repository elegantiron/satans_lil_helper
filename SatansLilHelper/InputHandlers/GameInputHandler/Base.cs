using System;
using System.Collections.Generic;
using System.Diagnostics;
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

#nullable enable
internal partial class GameInputHandler : IInputHandler, Interfaces.IUpdateable
{
    private IRandom _rng;
    private Point _mapSize;
    private GameWorld _gameWorld;
    private MessageLog _messageLog;
    private Rectangle _statusShadeShape;
    private PlayerTurn _playerTurn;
    private VecPair _statusVecs,
        _messageLogVecs,
        _locationVecs,
        _healthVecs,
        _manaVecs;
    private Vector2 _turnHeaderLocation,
        _movesLeftLocation,
        _attacksLeftLocation;

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
        _movesLeftLocation = _attacksLeftLocation = _turnHeaderLocation = Vector2.Zero;
        _playerTurn = new(_gameWorld.CurrentMap.Player);
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
        _movesLeftLocation.X =
            _attacksLeftLocation.X =
            _turnHeaderLocation.X =
                _locationVecs.Location.X;
        _turnHeaderLocation.Y = _manaVecs.Location.Y + 2 * textSize.Y;
        _movesLeftLocation.Y = _turnHeaderLocation.Y + textSize.Y;
        _attacksLeftLocation.Y = _movesLeftLocation.Y + textSize.Y;
    }

    public void Update(GameTime gameTime)
    {
        (_gameWorld as IRegistry).Update(gameTime);
    }

    public void ConfirmEndTurn(bool answer)
    {
        if (answer)
        {
            _playerTurn.Finish();
            _gameWorld.ActionStack.AddAction(_playerTurn);
            _playerTurn = new(_gameWorld.Player);
            _gameWorld.GetNextActor();
        }
    }
}
