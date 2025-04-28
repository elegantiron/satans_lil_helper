using Friflo.Engine.ECS;

namespace SatansLilHelper.ECSComponents;

[ComponentKey("level")]
public struct Level(uint value) : IComponent
{
    public uint Value = value;
}
