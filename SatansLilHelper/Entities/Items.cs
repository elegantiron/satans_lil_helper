using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.Entities;

internal static class Items
{
    public static Entity HealthPotion(Entity entity)
    {
        entity.Add(
            new EntityName("Healing Potion"),
            new RandomEffect(ItemEffect.Heal, 1, 8, 2),
            Tags.Get<Item, Activatable>()
        );
        return entity;
    }

    public static Entity HealthPotion(Entity entity, Entity holder)
    {
        HealthPotion(entity);
        entity.Add(new Inventory(holder));
        return entity;
    }

    public static Entity Torch(Entity entity)
    {
        entity.Add(
            new EntityName("Torch"),
            new ItemSlots(ItemType.Torch),
            Tags.Get<Item, Equippable>()
        );

        entity.AddRelation(new AbilityStat(AbilityID.LightRadius, 6m));

        return entity;
    }

    public static Entity RustySword(Entity entity)
    {
        entity.Add(
            new EntityName("Rusty Sword"),
            new ItemSlots(ItemType.Weapon1H),
            new Attack(1, 6),
            Tags.Get<Item, Equippable>()
        );
        return entity;
    }

    public static Entity RustySword(Entity entity, Entity holder, bool equipped = false)
    {
        RustySword(entity);
        entity.Add(new Inventory(holder));
        if (equipped)
            entity.Add(new Equipper(holder));
        return entity;
    }
}
