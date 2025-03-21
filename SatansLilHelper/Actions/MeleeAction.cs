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
    protected MersenneTwister Twister;
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
        if (!IsBlocked || TargetEntity == null)
            throw new MissingTargetException();


        Twister = rng;
        preState = rng.GetState();

        Damage = EntityCalcs.GetDamage(Twister, Entity, (Entity)TargetEntity);


        postState = rng.GetState();
        rng.SetState(preState);
    }

    public override void Perform()
    {
        Twister.SetState(postState);
    }

    public override void Rewind()
    {
        Twister.SetState(preState);
    }
}
