using Friflo.Engine.ECS;
using Friflo.Json.Fliox;

namespace SatansLilHelper.Components;

[ComponentKey("equipper")]
internal struct Equipper(Entity target) : ILinkComponent
{
    [Serialize]
    public Entity Target = target;

    public readonly Entity GetIndexedValue() => Target;
}
