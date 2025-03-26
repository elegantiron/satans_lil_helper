using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

internal struct Equipper(Entity target) : ILinkComponent
{
    public Entity Target = target;

    public readonly Entity GetIndexedValue() => Target;
}
