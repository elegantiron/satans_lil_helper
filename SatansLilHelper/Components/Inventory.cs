using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

[ComponentKey("holder")]
internal struct Inventory(Entity target) : ILinkComponent
{
    public Entity Target = target;

    public readonly Entity GetIndexedValue()
    {
        return Target;
    }
}
