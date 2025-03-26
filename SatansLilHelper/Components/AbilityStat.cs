using Friflo.Engine.ECS;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Components;

public struct AbilityStat(AbilityID type, decimal basis, decimal growth) : IRelation<AbilityID>
{
    public AbilityID Type = type;
    public decimal Basis = basis,
        Growth = growth;

    public AbilityStat(AbilityID type, decimal basis)
        : this(type, basis, 0m) { }

    public readonly AbilityID GetRelationKey() => Type;
}
