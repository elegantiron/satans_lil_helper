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
        if (_isBlocked && _isPlayer)
        {
            if (_isOffMap)
                _messages.Add(new(GameStrings.MapEdge, Constants.Colors.Impossible));
            else
                _messages.Add(new(GameStrings.PathBlocked, Constants.Colors.Impossible));
        }
    }

    public override void Perform()
    {
        if (!_isBlocked)
            _entity.AddComponent<Location>(Destination);
        base.Perform();
    }

    public override void Rewind()
    {
        if (!_isBlocked)
            _entity.AddComponent<Location>(Origin);
        base.Rewind();
    }
}
