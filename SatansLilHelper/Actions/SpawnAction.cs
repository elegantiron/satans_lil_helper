using System;
using Friflo.Engine.ECS;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

#nullable enable
internal class SpawnAction : IAction
{
    private byte[]? _preState = null,
        _postState = null;
    private MersenneTwister? _rng = null;
    private Entity _entity;

    public Entity Entity
    {
        get { return _entity; }
    }

    private SpawnAction(BaseMap gameMap)
    {
        _entity = gameMap.Registry.CreateEntity();
    }

    public SpawnAction(
        Action<Entity, MersenneTwister> spawnFunction,
        BaseMap gameMap,
        MersenneTwister rng,
        (int, int) location
    )
        : this(gameMap)
    {
        _rng = rng;
        _preState = _rng.GetState();
        spawnFunction(_entity, _rng);
        gameMap.PlaceEntity(_entity, location);
        _entity.Enabled = false;
        _postState = _rng.GetState();
    }

    public SpawnAction(Action<Entity> spawnFunction, BaseMap gameMap)
        : this(gameMap)
    {
        spawnFunction(_entity);
        _entity.Enabled = false;
    }

    public void Perform()
    {
        _rng?.SetState(_postState);
        _entity.Enabled = true;
    }

    public void Rewind()
    {
        _rng?.SetState(_preState);
        _entity.Enabled = false;
    }
}
