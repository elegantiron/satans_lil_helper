using Friflo.Engine.ECS;
using Friflo.Json.Fliox;

namespace SatansLilHelper.Components;

[ComponentKey("level")]
public struct Level(uint value) : IComponent
{
    [Serialize]
    public uint Value = value;
}
