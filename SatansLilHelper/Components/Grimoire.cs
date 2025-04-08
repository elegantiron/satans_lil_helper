using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

[ComponentKey("known-by")]
internal struct Grimoire(Entity target) : ILinkComponent
{
    public Entity Target = target;

    public readonly Entity GetIndexedValue() => Target;
}
