using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SatansLilHelper.Actions;
using SatansLilHelper.Components;

namespace SatansLilHelper.InputHandlers;

#if DEBUG
internal partial class GameInputHandler
{
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

    private void Undo()
    {
        if (_playerTurn.Undo())
            return;
        while (_gameWorld.ActionStack.HasActions && !_gameWorld.ActionStack.IsPlayerTurn)
        {
            _gameWorld.ActionStack.Rewind();
            _gameWorld.RewindInitiative();
        }
        if (_gameWorld.ActionStack.GetPlayerTurn(_gameWorld.Player, out _playerTurn))
        {
            _playerTurn.Undo();
            return;
        }
    }
}
#endif
