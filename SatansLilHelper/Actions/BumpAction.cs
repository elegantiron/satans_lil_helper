using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class BumpAction : ActionWithDirection
{
    private ActionWithDirection Action;

    public BumpAction(
        Entity entity,
        Point direction,
        BaseMap gameMap,
        ArchetypeQuery query,
        MersenneTwister rng,
        bool isPlayer
    )
        : base(entity, direction, gameMap, query, isPlayer)
    {
        if (TargetEntity != null)
            Action = new MoveAction(Entity, direction, gameMap, query, isPlayer);
        else
            Action = new MeleeAction(Entity, direction, gameMap, query, rng, isPlayer);
    }

    public override void Perform()
    {
        Action.Perform();
    }

    public override void Rewind()
    {
        Action.Rewind();
    }
}
