using Friflo.Engine.ECS;

namespace SatansLilHelper.ECSComponents;

internal struct OldTargetable(int range, int radius) : IComponent
{
    public int Range = range,
        Radius = radius;
}
