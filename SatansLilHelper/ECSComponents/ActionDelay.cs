using Friflo.Engine.ECS;

namespace SatansLilHelper.ECSComponents;

[ComponentKey("action-delay")]
public struct ActionDelay(uint value) : IComponent
{
    public uint Value = value;
}
