using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.EntityTags;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Entities;

public static class Enemies
{
    private static void Base(Entity entity)
    {
        entity.AddComponent(new ActionDelay(1500));
        entity.AddComponent(new Location(-200, -200));
        entity.AddComponent(new Level(-1));
        entity.AddComponent(new ItemSlots(ItemType.None));
        entity.AddComponent(new Attack(0, 0));
        entity.AddComponent(new TextureIndex());

        entity.AddRelation(new ResourceStat(ResourceID.Health, 0));
        entity.AddRelation(new ResourceStat(ResourceID.Mana, 0));

        entity.AddRelation(new AbilityStat(AbilityID.Strength, 0));
        entity.AddRelation(new AbilityStat(AbilityID.MagicPower, 0));
        entity.AddRelation(new AbilityStat(AbilityID.PhysicalDefense, 0));
        entity.AddRelation(new AbilityStat(AbilityID.MagicDefense, 0));
        entity.AddRelation(new AbilityStat(AbilityID.Evasion, 0));
        entity.AddRelation(new AbilityStat(AbilityID.Crit, 0));
        entity.AddRelation(new AbilityStat(AbilityID.Speed, 0));
        entity.AddRelation(new AbilityStat(AbilityID.Vision, 0));
        entity.AddRelation(new AbilityStat(AbilityID.LightRadius, 0));

        entity.AddTag<Actor>();
        entity.AddTag<Blocking>();
        entity.AddTag<Alive>();
    }

    public static void Wolf(Entity entity, MersenneTwister rng)
    {
        Base(entity);
        entity.AddComponent(new EntityName("wolf"));
        entity.AddComponent(new ActionDelay(rng.Next(1, 15)));
        entity.AddComponent(new TextureIndex(TextureID.Wolf));

        entity.AddRelation(new ResourceStat(ResourceID.Health, rng.Next(1, 8) + 16));

        entity.AddRelation(new AbilityStat(AbilityID.Strength, 2));
        entity.AddRelation(new AbilityStat(AbilityID.PhysicalDefense, 5));
        entity.AddRelation(new AbilityStat(AbilityID.Crit, 1));
        entity.AddRelation(new AbilityStat(AbilityID.Speed, 3));
        entity.AddRelation(new AbilityStat(AbilityID.Vision, 8));
    }
}
