using System.Collections.Generic;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Properties;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Actions;

internal class SmiteAction(Entity entity) : IAction, IMessageSender
{
    private List<LogMessage> _messages =
    [
        new LogMessage(GameStrings.DebugSmite, Constants.Colors.EnemyAttack),
    ];
    private Entity _entity = entity;

    public List<LogMessage> Messages
    {
        get { return _messages; }
    }
    public Entity? Entity
    {
        get { return _entity; }
    }

    public void Perform()
    {
        (this as IMessageSender).SendMessages();
        _entity.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur--;
    }

    public void Rewind()
    {
        (this as IMessageSender).RetractMessages();
        _entity.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur++;
    }
}
