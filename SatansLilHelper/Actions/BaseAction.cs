using Friflo.Engine.ECS;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Actions;

public abstract class BaseAction(Entity entity, ArchetypeQuery query, bool isPlayer)
{
    protected Entity Entity = entity;
    protected bool IsPlayer = isPlayer;
    protected ArchetypeQuery Query = query;
    protected LogMessage? _logMessage;

    public virtual void Perform()
    {
        if (_logMessage != null)
            EventBus.Send(Events.AddLogMessage, _logMessage ?? default);
    }

    public virtual void Rewind()
    {
        if (_logMessage != null)
            EventBus.Send(Events.PruneLogMessage, _logMessage ?? default);
    }
}
