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
        switch (key)
        {
            case Keys.Escape:
                return new PauseInputHandler(this);
            case Keys when Dicts.MovementKeys.ContainsKey(key):
                HandleMovement(key);
                break;
            case Keys.H:
                return new HelpInputHandler(this);
            case Keys.T:
                return new TargetingInputHandler(this, 5, 2);
            case Keys.S:
                return new IncomingLinkInputHandler<Grimoire>(
                    this,
                    Properties.GameStrings.SpellBookTitle
                );
            case Keys.I:
                return new IncomingLinkInputHandler<Inventory>(
                    this,
                    Properties.GameStrings.Inventory_Title
                );
            case Keys.F:
                return new ConfirmInputHandler(
                    this,
                    Properties.GameStrings.ConfirmTurnEnd,
                    FontID.Messages,
                    ConfirmEndTurn
                );
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
        if (_playerTurn.IsFinished)
        {
            _gameWorld.ActionStack.AddAction(_playerTurn);
            _playerTurn = new(_gameWorld.Player);
            _gameWorld.GetNextActor();
        }
    }
}
