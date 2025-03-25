using System.Collections.Generic;
using System.Linq;
using Friflo.Engine.ECS;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Actions;

public abstract class BaseAction(Entity entity, ArchetypeQuery query, bool isPlayer)
{
    protected Entity Entity = entity;
    protected bool IsPlayer = isPlayer;
    protected ArchetypeQuery Query = query;
    protected List<LogMessage> _logMessages = [];

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
