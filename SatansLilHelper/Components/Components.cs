using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

[ComponentKey("action-delay")]
public struct ActionDelay : IComponent
{
    public int value;
}
