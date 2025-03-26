using System;
using Friflo.Engine.ECS;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class ConfusedAction : IAction
{
    private byte[] _preState;
    private BumpAction _action;
    private Entity _entity;
    private bool _isPlayer;
    private BaseMap _gameMap;

    public ConfusedAction(
        Entity entity,
        BaseMap gameMap,
        MersenneTwister rng,
        bool isPlayer = false
    )
    {
        _preState = rng.GetState();
        _entity = entity;
        _isPlayer = isPlayer;
        _gameMap = gameMap;
    }

    public Entity? Entity
    {
        get { return _entity; }
    }

    public void Perform()
    {
        throw new NotImplementedException();
    }

    public void Rewind()
    {
        throw new NotImplementedException();
    }
}
