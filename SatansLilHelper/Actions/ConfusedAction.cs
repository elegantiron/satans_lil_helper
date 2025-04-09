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
    private bool _successful = true;

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
        _action = new(entity, new Microsoft.Xna.Framework.Point(0, 0), gameMap, rng, isPlayer);
    }

    public Entity? Entity => _entity;
    public bool Successful => _successful;

    public void Perform()
    {
        _action.Perform();
        throw new NotImplementedException();
    }

    public void Rewind()
    {
        _action.Rewind();
        throw new NotImplementedException();
    }
}
