using System;
using System.Security.Cryptography;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Exceptions;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class MeleeAction : ActionWithDirection
{
    protected byte[] preState,
        postState;
    protected MersenneTwister rng;
    protected int Damage;

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

        this.rng = rng;
        preState = rng.GetState();

        // TODO: calculate the attack resulte

        postState = rng.GetState();
        rng.SetState(preState);
    }

    public override void Perform()
    {
        rng.SetState(postState);
    }

    public override void Rewind()
    {
        rng.SetState(preState);
    }
}
