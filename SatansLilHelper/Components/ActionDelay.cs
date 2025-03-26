using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

[ComponentKey("action-delay")]
public struct ActionDelay(uint value) : IComponent
{
    public uint Value = value;
}
