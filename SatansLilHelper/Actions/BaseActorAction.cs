using System.Collections.Generic;
using System.Linq;
using Friflo.Engine.ECS;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Actions;

internal abstract class BaseActorAction(Entity entity, ArchetypeQuery query, bool isPlayer)
    : IAction
{
    protected Entity Entity = entity;
    protected bool IsPlayer = isPlayer;
    protected ArchetypeQuery Query = query;
    protected List<LogMessage> _logMessages = [];
    protected List<BaseActorAction> _subActions = [];

    public virtual void Perform()
    {
        if (_logMessages != null)
            foreach (LogMessage logMessage in _logMessages)
                EventBus.Send(Events.AddLogMessage, logMessage);
    }

    public virtual void Rewind()
    {
        if (_logMessages != null)
            foreach (LogMessage logMessage in _logMessages.AsReadOnly().Reverse())
                EventBus.Send(Events.PruneLogMessage, logMessage);
    }
}
