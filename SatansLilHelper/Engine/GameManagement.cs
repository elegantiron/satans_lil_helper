using System.IO;
using Microsoft.Xna.Framework;
using SatansLilHelper.Utils;

namespace SatansLilHelper;

internal partial class Engine
{
    private GameWorld _gameWorld;
    private bool _inCombat;
    public bool InCombat => _inCombat;
    public GameWorld World => _gameWorld;

    public void NewGame()
    {
        if (File.Exists(_gamePath + "game.dat"))
            File.Delete(_gamePath + "game.dat");

        _gameWorld = new(new MersenneTwister(), new Point(100));
    }

    public bool LoadGame()
    {
        if (!File.Exists(_gamePath + "game.dat"))
            return false;
        return true;
    }

    public void SaveGame() { }

    public void EndPlayerTurn()
    {
        _playerTurn.Finish();
        _gameWorld.ActionStack.AddAction(_playerTurn);
        _playerTurn = new(_gameWorld.Player);
    }
}
