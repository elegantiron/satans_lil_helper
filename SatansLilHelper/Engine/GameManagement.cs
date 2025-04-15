using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SatansLilHelper.Utils;

namespace SatansLilHelper;

internal partial class Engine
{
    private GameWorld _gameWorld;
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
}
