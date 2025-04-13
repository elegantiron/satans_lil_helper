using Friflo.Engine.ECS;

namespace SatansLilHelper.ECSComponents;

[ComponentKey("equipper")]
internal struct Equipper(Entity target) : ILinkComponent
{
    public Entity Target = target;

    public readonly Entity GetIndexedValue()
    {
        return Target;
    }
}
