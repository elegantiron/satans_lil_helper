using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Extensions;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;

namespace SatansLilHelper.InputHandlers;

internal partial class GameInputHandler
{
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
        _gameWorld.CurrentMap.UpdatePlayerVision();

        _gameWorld.CurrentMap.Draw(spriteBatch, textureMap, effectMap, songMap, fontMap);

        if (true)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(
                textureMap[TextureID.WhitePixel],
                _statusShadeShape,
                Colors.TranslucentBlack
            );
            DrawStatus(spriteBatch, fontMap);
            DrawMessageLog(spriteBatch, fontMap);
            spriteBatch.End();
        }
    }

    private void DrawStatus(SpriteBatch spriteBatch, Dictionary<FontID, SpriteFont> fontMap)
    {
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            Properties.GameStrings.StatusTitle,
            _statusVecs.Location,
            Colors.White,
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
            Colors.White
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
            Colors.White
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
            Colors.White
        );
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            Properties.GameStrings.StatusTurn,
            _turnHeaderLocation,
            Colors.White
        );
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            String.Format(
                Properties.GameStrings.StatusMoves,
                _playerTurn.MovesUsed,
                _playerTurn.MovesMax
            ),
            _movesLeftLocation,
            Colors.White
        );
        spriteBatch.DrawString(
            fontMap[FontID.Status],
            String.Format(
                Properties.GameStrings.StatusAttacks,
                _playerTurn.AttacksUsed,
                _playerTurn.AttacksMax
            ),
            _attacksLeftLocation,
            Colors.White
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
}
