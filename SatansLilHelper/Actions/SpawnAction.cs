using System;

using Friflo.Engine.ECS;

using SatansLilHelper.Components;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

#nullable enable
internal class SpawnAction : IAction
{
    private Action<Entity, IRandom> _spawnAction;
    private IRandom _rng;
    private BaseMap _gameMap;
    private Entity _entity;
    private byte[] _preState;
    private (int X, int Y)? _location;

    public SpawnAction(Action<Entity, IRandom> spawnAction, BaseMap gameMap, IRandom rng)
    {
        _spawnAction = spawnAction;
        _gameMap = gameMap;
        _rng = rng;
        _preState = _rng.GetState();
    }

    public SpawnAction(
        Action<Entity, IRandom> spawnAction,
        BaseMap gameMap,
        IRandom rng,
        (int X, int Y) location
    )
        : this(spawnAction, gameMap, rng)
    {
        _location = location;
    }

    public Entity? Entity => _entity;
    public bool Successful => true;

    public void Perform()
    {
        _entity = _gameMap.Registry.CreateEntity();
        _spawnAction(_entity, _rng);
        if (_location is (int, int) place)
            _entity.AddComponent(new Location(place.X, place.Y));
    }

    public void Rewind()
    {
        _entity.DeleteEntity();
        _rng.SetState(_preState);
    }
}
