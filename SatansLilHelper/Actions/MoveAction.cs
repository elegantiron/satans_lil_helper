using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

public class MoveAction : ActionWithDirection
{
    public MoveAction(
        Entity entity,
        Point direction,
        BaseMap gameMap,
        ArchetypeQuery query,
        bool isPlayer
    )
        : base(entity, direction, gameMap, query, isPlayer)
    {
        if (IsBlocked)
            throw new PathBlockedException("The way is blocked.");
    }

    public override void Perform()
    {
        Entity.AddComponent<Location>(Destination);
    }

    public override void Rewind()
    {
        Entity.AddComponent<Location>(Origin);
    }
}
