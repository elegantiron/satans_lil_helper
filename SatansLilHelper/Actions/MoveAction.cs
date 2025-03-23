using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Content.Text;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

#nullable enable
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
            _logMessages.Add(new(GameStrings.PathBlocked, Constants.Colors.Impossible));
    }

    public override void Perform()
    {
        if (!IsBlocked)
            Entity.AddComponent<Location>(Destination);
        base.Perform();
    }

    public override void Rewind()
    {
        if (!IsBlocked)
            Entity.AddComponent<Location>(Origin);
        base.Rewind();
    }
}
