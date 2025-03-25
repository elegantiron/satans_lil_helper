using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Content.Text;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

#nullable enable
internal class MoveAction : ActionWithDirection, IMessageSender
{
    public List<LogMessage> Messages
    {
        get { return _messages; }
    }

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
            if (IsOffMap)
                _messages.Add(new(GameStrings.MapEdge, Constants.Colors.Impossible));
            else
                _messages.Add(new(GameStrings.PathBlocked, Constants.Colors.Impossible));
    }

    public override void Perform()
    {
        if (!IsBlocked)
            _entity.AddComponent<Location>(Destination);
    }

    public override void Rewind()
    {
        if (!IsBlocked)
            _entity.AddComponent<Location>(Origin);
    }
}
