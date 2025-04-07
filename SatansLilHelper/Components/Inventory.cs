using Friflo.Engine.ECS;
using Friflo.Json.Fliox;

namespace SatansLilHelper.Components;

[ComponentKey("holder")]
internal struct Inventory(Entity target) : ILinkComponent
{
    [Serialize]
    public Entity Target = target;

    public readonly Entity GetIndexedValue() => Target;
}
