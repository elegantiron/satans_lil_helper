using System.Security.Cryptography;
using Friflo.Engine.ECS;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Components;

[ComponentKey("action-delay")]
public struct ActionDelay : IComponent
{
    public int value;
}

[ComponentKey("location")]
public struct Location(int x, int y) : IIndexedComponent<(int, int)>
{
    public int X = x,
        Y = y;

    public readonly (int, int) GetIndexedValue()
    {
        return (X, Y);
    }
}

[ComponentKey("attack")]
public struct Attack(int dice, int sides, int bonus = 0) : IComponent
{
    public int Dice = dice,
        Sides = sides,
        Bonus = bonus;
}

[ComponentKey("defense")]
public struct Defense : IComponent
{
    public int magic,
        physical;
}

public struct ResourceStat : IRelation<ResourceID>
{
    public ResourceID Type;
    public int Cur,
        Max;
    public decimal Growth;

    public readonly ResourceID GetRelationKey() => Type;

    public ResourceStat(ResourceID type, int max, int cur, decimal growth)
    {
        Type = type;
        Growth = growth;
        Max = max;
        Cur = cur;
    }

    public ResourceStat(ResourceID type, int max, decimal growth)
    {
        Growth = growth;
        Type = type;
        Max = Cur = max;
    }
}

public struct AbilityStat(AbilityID type, decimal basis, decimal growth) : IRelation<AbilityID>
{
    public AbilityID Type = type;
    public decimal Basis = basis,
        Growth = growth;

    public readonly AbilityID GetRelationKey() => Type;
}

public struct Skill : IRelation<SkillID>
{
    public SkillID Type;

    public readonly SkillID GetRelationKey() => Type;
}

public struct Equipment(Entity target) : ILinkRelation
{
    public Entity Target = target;

    public readonly Entity GetRelationKey() => Target;
}

public struct ItemSlots : IComponent
{
    public ItemType Types;
}
