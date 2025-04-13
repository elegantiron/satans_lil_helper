using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Processors;

internal static partial class AI
{
    public static EntityTurn Process(Entity entity, Location playerLoc, BaseMap map, IRandom rng)
    {
        ActorID type = ActorID.Wolf;
        return type switch
        {
            ActorID.Wolf => Melee(entity, playerLoc, map, rng),
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
