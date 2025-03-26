using System;
using System.Runtime.CompilerServices;
using Friflo.Engine.ECS;
using MonoGame.Extended;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

#nullable enable
internal class SpawnAction : IAction
{
    private byte[]? _preState = null,
        _postState = null;
    private (int X, int Y)? _location;
    private MersenneTwister? _rng = null;
    private Entity _entity;
    private Action<Entity>? _nonrandomSpawn;
    private Action<Entity, MersenneTwister>? _randomSpawn;
    private BaseMap _gameMap;

    public Entity Entity
    {
        get { return _entity; }
    }

    private SpawnAction(BaseMap gameMap)
    {
        _gameMap = gameMap;
    }

    private SpawnAction(Action<Entity, MersenneTwister> spawnFunction, BaseMap gameMap)
        : this(gameMap)
    {
        _randomSpawn = spawnFunction;
    }

    public SpawnAction(Action<Entity> spawnFunction, BaseMap gameMap)
        : this(gameMap)
    {
        _nonrandomSpawn = spawnFunction;
    }

    public SpawnAction(Action<Entity> spawnFunction, BaseMap gameMap, (int X, int Y) location)
        : this(spawnFunction, gameMap)
    {
        _location = location;
    }

    public SpawnAction(
        Action<Entity, MersenneTwister> spawnFunction,
        BaseMap gameMap,
        MersenneTwister rng,
        (int, int) location
    )
        : this(spawnFunction, gameMap)
    {
        _rng = rng;
        _preState = _rng.GetState();
        _location = location;
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
