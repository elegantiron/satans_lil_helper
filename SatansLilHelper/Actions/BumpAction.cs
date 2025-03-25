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
        MersenneTwister rng,
        bool isPlayer
    )
        : base(entity, direction, gameMap, isPlayer)
    {
        if (_target == null)
            Action = new MoveAction(_entity, direction, gameMap, isPlayer);
        else
            Action = new MeleeAction(_entity, direction, gameMap, rng, isPlayer);
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
