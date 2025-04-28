using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

#nullable enable
internal class MoveAction : ActionWithDirection, IMessageSender, IMoveAction
{
    public List<LogMessage> Messages => _messages;

    public MoveAction(Entity entity, Point direction, BaseMap gameMap, bool isPlayer)
        : base(entity, direction, gameMap, isPlayer)
    {
        if (_isBlocked)
        {
            if (_isPlayer)
            {
                if (_isOffMap)
                    _messages.Add(new(GameStrings.MapEdge, Constants.Colors.Impossible));
                else
                    _messages.Add(new(GameStrings.PathBlocked, Constants.Colors.Impossible));
            }
        }
        else
            _successful = true;
    }

    public override void Perform()
    {
        if (_successful)
            _entity.AddComponent<Location>(Destination);
        base.Perform();
    }

    public override void Rewind()
    {
        if (_successful)
            _entity.AddComponent<Location>(Origin);
        base.Rewind();
    }
}
