using Friflo.Engine.ECS;

namespace SatansLilHelper.Types;

public struct MovementResult(Entity entity, (int, int) origin, (int, int) destination)
{
    public Entity Entity = entity;
    public (int X, int Y) Origin = origin,
        Destination = destination;
}
