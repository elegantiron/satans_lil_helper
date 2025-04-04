using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using SatansLilHelper.Actions;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.InputHandlers;

internal partial class GameInputHandler
{
    public IInputHandler HandleKey(Keys key)
    {
        if (_confirmPopup is not null)
            return ConfirmationMode(key);
        else
            return StandardMode(key);
    }

    private IInputHandler StandardMode(Keys key)
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
            case Keys.F:
                _confirmPopup = new ConfirmPopup(Properties.GameStrings.ConfirmTurnEnd);
                break;
#if DEBUG
            case Keys.X:
                _gameWorld.ActionStack.AddAction(new SmiteAction(_gameWorld.CurrentMap.Player));
                break;
            case Keys.D:
                return new DebugMenuInputHandler(this);
            case Keys.Z:
                Undo();
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

    private void HandleMovement(Keys key)
    {
        if (!_gameWorld.CurrentMap.IsPlayerNext)
            return;
        _playerTurn.AddAction(
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

    private GameInputHandler ConfirmationMode(Keys key)
    {
        switch (key)
        {
            case Keys.Escape:
                _confirmPopup = null;
                break;
        }
        return this;
    }
}
