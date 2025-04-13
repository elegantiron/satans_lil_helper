using Friflo.Engine.ECS;

namespace SatansLilHelper.ECSComponents;

[ComponentKey("known-by")]
internal struct Grimoire(Entity target) : ILinkComponent
{
    public Entity Target = target;

    public readonly Entity GetIndexedValue()
    {
        return Target;
    }
}
