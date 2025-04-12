using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.Entities;

internal static class Items
{
    public static void HealthPotion(Entity entity, IRandom rng)
    {
        entity.Add(
            new EntityName("Healing Potion"),
            new RandomEffect(ItemEffect.Heal, 1, 8, 2),
            Tags.Get<Item, Activatable>()
        );
    }

    public static void Torch(Entity entity)
    {
        entity.Add(
            new EntityName("Torch"),
            new ItemSlots(ItemType.Torch),
            Tags.Get<Item, Equippable>()
        );

        entity.AddRelation(new AbilityStat(AbilityID.LightRadius, 6m));
    }
}
