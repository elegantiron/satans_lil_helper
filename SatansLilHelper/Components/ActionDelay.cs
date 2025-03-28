using Friflo.Engine.ECS;
using Friflo.Json.Fliox;

namespace SatansLilHelper.Components;

[ComponentKey("action-delay")]
public struct ActionDelay(uint value) : IComponent
{
    [Serialize]
    public uint Value = value;
}
