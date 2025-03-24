using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Entities;

public static class Professions
{
    public static void Warrior(Entity entity)
    {
        Bases.Actor(entity);
        entity.AddComponent(new ActionDelay(0));
        entity.AddComponent(new Location(0, 0));
        entity.AddComponent(new Level(1));
        entity.AddComponent(new ItemSlots(ItemType.Warrior));
        entity.AddComponent(new Attack(1, 6));

        #region Resource Stats
        entity.AddRelation(new ResourceStat(ResourceID.Health, 60, 10));
        entity.AddRelation(new ResourceStat(ResourceID.Mana, 5, 1));
        #endregion Resource Stats

        #region Ability Stats
        entity.AddRelation(new AbilityStat(AbilityID.Strength, 5, 0.5m));
        entity.AddRelation(new AbilityStat(AbilityID.MagicPower, 0, 0.25m));
        entity.AddRelation(new AbilityStat(AbilityID.PhysicalDefense, 7, 0.5m));
        entity.AddRelation(new AbilityStat(AbilityID.MagicDefense, 2, 0.25m));
        entity.AddRelation(new AbilityStat(AbilityID.Speed, 2, 0));
        entity.AddRelation(new AbilityStat(AbilityID.Vision, 7, 0));
        #endregion Ability Stats

        #region Tags
        entity.AddTag<IsPlayer>();
        #endregion Tags
    }
}
