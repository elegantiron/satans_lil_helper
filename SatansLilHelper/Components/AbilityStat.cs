using Friflo.Engine.ECS;

using SatansLilHelper.Constants;

namespace SatansLilHelper.Components;

[ComponentKey("ability-stat")]
public struct AbilityStat(AbilityID type, decimal basis, decimal growth) : IRelation<AbilityID>
{
    public AbilityID Type = type;

    public decimal Basis = basis;

    public decimal Growth = growth;

    public int Cur = (int)basis;

    public AbilityStat(AbilityID type, decimal basis)
        : this(type, basis, 0m) { }

    public readonly AbilityID GetRelationKey()
    {
        return Type;
    }
}
