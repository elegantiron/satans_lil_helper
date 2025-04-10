using Friflo.Engine.ECS;

using Microsoft.Xna.Framework;

using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class BumpAction : ActionWithDirection
{
    private ActionWithDirection _action;

    public BumpAction(Entity entity, Point direction, BaseMap gameMap, IRandom rng, bool isPlayer)
        : base(entity, direction, gameMap, isPlayer)
    {
        _action = _target == null
            ? new MoveAction(_entity, direction, gameMap, isPlayer)
            : new MeleeAction(_entity, direction, gameMap, rng, isPlayer);
    }

    public override void Perform()
    {
        _action.Perform();
    }

    public override void Rewind()
    {
        _action.Rewind();
    }

    public IAction Action => _action;
}
