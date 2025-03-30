using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.EntityTags;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Entities;

internal static class Items
{
    private static void Base(Entity entity)
    {
        entity.AddTag<Item>();
    }

    public static void HealthPotion(Entity entity, IRandom rng)
    {
        Base(entity);
        entity.AddComponent(new EntityName("Healing Potion"));
        entity.AddComponent(new EffectPower((int)rng.Next(1, 8) + 2));
    }

    public static void Torch(Entity entity)
    {
        Base(entity);
        entity.AddRelation(new AbilityStat(AbilityID.LightRadius, 6m));
        entity.AddComponent(new EntityName("Torch"));
    }
}
