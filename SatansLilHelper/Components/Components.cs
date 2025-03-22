using System.Security.Cryptography;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework.Graphics;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Components;

[ComponentKey("action-delay")]
public struct ActionDelay(int value) : IComponent
{
    public int Value = value;
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
        Basis;
    public decimal Growth;

    public readonly ResourceID GetRelationKey() => Type;

    public ResourceStat(ResourceID type, int max, int cur, decimal growth)
    {
        Type = type;
        Growth = growth;
        Basis = max;
        Cur = cur;
    }

    public ResourceStat(ResourceID type, int max, decimal growth)
    {
        Growth = growth;
        Type = type;
        Basis = Cur = max;
    }

    public ResourceStat(ResourceID type, int max)
    {
        Growth = 0;
        Type = type;
        Basis = Cur = max;
    }
}

public struct AbilityStat(AbilityID type, decimal basis, decimal growth) : IRelation<AbilityID>
{
    public AbilityID Type = type;
    public decimal Basis = basis,
        Growth = growth;

    public AbilityStat(AbilityID type, decimal basis)
        : this(type, basis, 0m) { }

    public readonly AbilityID GetRelationKey() => Type;
}

public struct Skill : IRelation<SkillID>
{
    public SkillID Type;

    public readonly SkillID GetRelationKey() => Type;
}

public struct EquippedItem(ItemType type, Entity target) : ILinkComponent, IRelation<ItemType>
{
    public Entity Target = target;
    public ItemType Type = type;

    public readonly ItemType GetRelationKey() => Type;

    public readonly Entity GetIndexedValue() => Target;
}

[ComponentKey("item-slots")]
public struct ItemSlots(ItemType types) : IComponent
{
    public ItemType Types = types;
}

[ComponentKey("level")]
public struct Level(int value) : IComponent
{
    public int Value = value;
}

[ComponentKey("texture-index")]
public struct TextureIndex : IComponent
{
    public TextureID Index;

    public TextureIndex()
    {
        Index = TextureID.Missing;
    }

    public TextureIndex(TextureID index)
    {
        Index = index;
    }
}
