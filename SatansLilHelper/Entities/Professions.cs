using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.EntityTags;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Entities;

public static class Professions
{
    private static void Base(Entity entity)
    {
        entity.AddComponent(new ActionDelay(1500));
        entity.AddComponent(new Location(-200, -200));
        entity.AddComponent(new Level(0));
        entity.AddComponent(new ItemSlots(ItemType.None));
        entity.AddComponent(new Attack(0, 0));
        entity.AddComponent(new TextureIndex());

        entity.AddRelation(new AbilityStat(AbilityID.Health, 0));
        entity.AddRelation(new AbilityStat(AbilityID.Mana, 0));

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
        entity.AddTag<Player>();
    }

    public static void Warrior(Entity entity)
    {
        Base(entity);
        entity.AddComponent(new ActionDelay(0));
        entity.AddComponent(new Location(0, 0));
        entity.AddComponent(new Level(1));
        entity.AddComponent(new ItemSlots(ItemType.Warrior));
        entity.AddComponent(new Attack(1, 6));

        #region Resource Stats
        entity.AddRelation(new AbilityStat(AbilityID.Health, 60, 10));
        entity.AddRelation(new AbilityStat(AbilityID.Mana, 5, 1));
        #endregion Resource Stats

        #region Ability Stats
        entity.AddRelation(new AbilityStat(AbilityID.Strength, 5, 0.5m));
        entity.AddRelation(new AbilityStat(AbilityID.MagicPower, 0, 0.25m));
        entity.AddRelation(new AbilityStat(AbilityID.PhysicalDefense, 7, 0.5m));
        entity.AddRelation(new AbilityStat(AbilityID.MagicDefense, 2, 0.25m));
        entity.AddRelation(new AbilityStat(AbilityID.Speed, 2, 0));
        entity.AddRelation(new AbilityStat(AbilityID.Vision, 7, 0));
        #endregion Ability Stats

        #region Starting gear
        // Torch
        Entity torch = entity.Store.CreateEntity();
        Items.Torch(torch);
        torch.AddComponent(new Equipper(entity));

        #endregion starting gear
    }
}
