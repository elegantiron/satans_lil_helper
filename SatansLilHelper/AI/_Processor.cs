using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.AI;

internal static partial class TurnProcessor
{
    public static EntityTurn Process(Entity entity, Location playerLoc, BaseMap map, IRandom rng)
    {
        EnemyType type = EnemyType.Wolf;
        return type switch
        {
            EnemyType.Wolf => Melee(entity, playerLoc, map, rng),
            _ => Default(entity),
        };
    }

    private static EntityTurn Default(Entity entity)
    {
        EntityTurn turn = new(entity);
        turn.Finish();
        return turn;
    }
}
