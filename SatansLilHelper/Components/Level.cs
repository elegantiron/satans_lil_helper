using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

[ComponentKey("level")]
public struct Level(uint value) : IComponent
{
    public uint Value = value;
}
