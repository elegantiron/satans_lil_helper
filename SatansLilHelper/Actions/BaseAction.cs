using Friflo.Engine.ECS;

namespace SatansLilHelper.Actions;

public abstract class BaseAction(Entity entity, ArchetypeQuery query, bool isPlayer)
{
    protected Entity Entity = entity;
    protected bool IsPlayer = isPlayer;
    protected ArchetypeQuery Query = query;

    public abstract void Perform();

    public abstract void Rewind();
}
