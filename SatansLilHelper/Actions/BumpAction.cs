using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class BumpAction : ActionWithDirection
{
    private ActionWithDirection _action;

    public BumpAction(Entity entity, Point direction, BaseMap gameMap, IRandom rng, bool isPlayer)
        : base(entity, direction, gameMap, isPlayer)
    {
        if (_target == null)
            _action = new MoveAction(_entity, direction, gameMap, isPlayer);
        else
            _action = new MeleeAction(_entity, direction, gameMap, rng, isPlayer);
    }

    public override void Perform()
    {
        _action.Perform();
    }

    public override void Rewind()
    {
        _action.Rewind();
    }

    public IAction Action
    {
        get { return _action; }
    }
}
