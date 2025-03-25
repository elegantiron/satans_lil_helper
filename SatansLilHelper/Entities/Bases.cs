using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.EntityTags;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Entities;

public static class Bases
{
    public static void Actor(Entity entity)
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
}
