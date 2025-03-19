using System;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class MeleeAction : ActionWithDirection
{
    private byte[] preState,
        postState;

    public MeleeAction(
        Entity entity,
        Point direction,
        BaseMap gameMap,
        ArchetypeQuery query,
        MersenneTwister rng,
        bool isPlayer
    )
        : base(entity, direction, gameMap, query, isPlayer)
    {
        if (!IsBlocked)
            throw new MissingTargetException();
        if (TargetEntity == null)
            throw new MissingTargetException();

        if (!Entity.TryGetComponent<Attack>(out Attack aggressorAtk))
            throw new Exceptions.MissingComponentException();
        if (!(bool)TargetEntity?.TryGetComponent<Defense>(out Defense targetDef))
            throw new Exceptions.MissingComponentException();
        preState = rng.GetState();
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
