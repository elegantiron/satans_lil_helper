using Friflo.Engine.ECS;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Components;

public struct ResourceStat : IRelation<ResourceID>
{
    public ResourceID Type;
    public uint Cur,
        Basis;
    public decimal Growth;

    public readonly ResourceID GetRelationKey() => Type;

    public ResourceStat(ResourceID type, uint max, uint cur, decimal growth)
    {
        Type = type;
        Growth = growth;
        Basis = max;
        Cur = cur;
    }

    public ResourceStat(ResourceID type, uint max, decimal growth)
    {
        Growth = growth;
        Type = type;
        Basis = Cur = max;
    }

    public ResourceStat(ResourceID type, uint max)
    {
        Growth = 0;
        Type = type;
        Basis = Cur = max;
    }
}
