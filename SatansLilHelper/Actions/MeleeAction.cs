using System;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class MeleeAction : ActionWithDirection
{
    public MeleeAction(
        Entity entity,
        Point direction,
        BaseMap gameMap,
        ArchetypeQuery query,
        Random rng,
        bool isPlayer
    )
        : base(entity, direction, gameMap, query, isPlayer)
    {
        if (!IsBlocked || TargetEntity == null)
        {
            throw new MissingTargetException();
        }
    }

    public override void Perform()
    {
        throw new NotImplementedException();
    }

    public override void Rewind()
    {
        throw new NotImplementedException();
    }
}
