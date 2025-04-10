using System.Collections.Generic;

using Friflo.Engine.ECS;

using Microsoft.Xna.Framework;

using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Utils;

internal class GameWorld : IRegistry
{
    private List<BaseMap> _maps;
    private int _currentMap = 0;
    private IRandom _rng;
    private Point _mapSize;
    private ActionStack _actionStack;

    public GameWorld(IRandom rng, Point mapSize)
    {
        _rng = rng;
        _mapSize = mapSize;
        _maps = [new ForestMap(_mapSize, _rng, new Point(1280, 720), true)];
        _actionStack = new();
    }

    public Entity Player => _maps[_currentMap].Player;

    public BaseMap CurrentMap => _maps[_currentMap];

    public bool IsPlayerNext => _maps[_currentMap].IsPlayerNext;

    public IRandom Generator => _rng;

    public ActionStack ActionStack => _actionStack;

    /// <summary>
    /// Used to get the next actor in the initiative order.
    /// </summary>
    /// <returns>The <c>Entity</c> to act next.</returns>
    public Entity GetNextActor()
    {
        return _maps[_currentMap].GetNextActor();
    }

    public void RewindInitiative()
    {
        _maps[_currentMap].RewindInitiative();
    }
}
