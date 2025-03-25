using System;
using Friflo.Engine.ECS;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

#nullable enable
internal class SpawnAction : IAction
{
    private byte[]? preState = null,
        postState = null;
    private MersenneTwister? Rng = null;
    private Entity Entity;

    private SpawnAction(BaseMap gameMap)
    {
        Entity = gameMap.Registry.CreateEntity();
    }

    public SpawnAction(
        Action<Entity, MersenneTwister> spawnFunction,
        BaseMap gameMap,
        MersenneTwister rng,
        (int, int) location
    )
        : this(gameMap)
    {
        Rng = rng;
        preState = Rng.GetState();
        spawnFunction(Entity, Rng);
        gameMap.PlaceEntity(Entity, location);
        Entity.Enabled = false;
    }

    public SpawnAction(Action<Entity> spawnFunction, BaseMap gameMap)
        : this(gameMap)
    {
        spawnFunction(Entity);
        Entity.Enabled = false;
    }

    public void Perform()
    {
        Rng?.SetState(postState);
        Entity.Enabled = true;
    }

    public void Rewind()
    {
        Rng?.SetState(preState);
        Entity.Enabled = false;
    }
}
