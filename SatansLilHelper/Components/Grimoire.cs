using Friflo.Engine.ECS;
using Friflo.Json.Fliox;

namespace SatansLilHelper.Components;

[ComponentKey("known-by")]
internal struct Grimoire(Entity target) : ILinkComponent
{
    [Serialize]
    public Entity Target = target;

    public readonly Entity GetIndexedValue() => Target;
}
