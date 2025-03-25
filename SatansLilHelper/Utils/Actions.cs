using System;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Types;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Utils;

public static class Actions
{
    private static bool IsEntityBlocked(BaseMap gameMap, (int x, int y) destination)
    {
        ArchetypeQuery blockingQuery = gameMap.GetBlockingEntities.HasValue<
            Location,
            (int x, int y)
        >(destination);
        if (blockingQuery.Count > 0)
            return true;
        return false;
    }

    private static bool IsDestinationWalkable(BaseMap gameMap, (int x, int y) destination)
    {
        return gameMap.Tiles[destination.x, destination.y].Walkable;
    }

    public static bool BumpAction(
        Entity entity,
        BaseMap gameMap,
        (int x, int y) direction,
        MersenneTwister rng,
        out ActionResult? result
    )
    {
        result = null;
        Location entLocation = entity.GetComponent<Location>();
        (int, int) destination = (entLocation.X + direction.x, entLocation.Y + direction.y);
        if (!IsEntityBlocked(gameMap, destination))
        {
            if (IsDestinationWalkable(gameMap, destination))
            {
                result = new ActionResult(
                    new MovementResult(entity, (entLocation.X, entLocation.Y), destination)
                );
                return true;
            }
        }
        else
        {
            result = new ActionResult();
            return true;
        }
        return false;
    }
}
