using Friflo.Engine.ECS;
using Friflo.Json.Fliox;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Components;

[ComponentKey("ability-stat")]
public struct AbilityStat(AbilityID type, decimal basis, decimal growth) : IRelation<AbilityID>
{
    [Serialize]
    public AbilityID Type = type;

    [Serialize]
    public decimal Basis = basis;

    [Serialize]
    public decimal Growth = growth;

    [Serialize]
    public uint Cur = (uint)basis;

    public AbilityStat(AbilityID type, decimal basis)
        : this(type, basis, 0m) { }

    public readonly AbilityID GetRelationKey() => Type;
}
