using System;

using Friflo.Engine.ECS;

using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils;

internal static class EntityCalcs
{
    public static int GetStat(Entity entity, AbilityID ability)
    {
        int total = 0;
        // Try to get the stat, don't bother getting the level if
        // the entity doesn't have the stat.
        if (entity.TryGetRelation(ability, out AbilityStat stat))
        {
            // Add the stat's base value
            total += (int)stat.Basis;
            // Try to get the entity's level
            if (entity.TryGetComponent<Level>(out Level level))
                // Add the amount gained from levels
                total += GetTotal(stat.Growth, level.Value);
        }

        // Recursively process any incoming "Equipper" links to
        // also process any equipped items.
        foreach (Entity ent in entity.GetIncomingLinks<Equipper>().Entities)
        {
            total += GetStat(ent, ability);
        }

        return total;
    }

    public static decimal GetDamageFactor(IRandom rng, Entity attacker, Entity target)
    {
        //throw new NotImplementedException();
        if (!attacker.TryGetComponent<Level>(out Level attackerLevel))
            throw new Exceptions.MissingComponentException();
        if (!target.TryGetComponent<Level>(out Level targetLevel))
            throw new Exceptions.MissingComponentException();
        int crit = GetStat(attacker, AbilityID.Crit);
        uint roll = rng.Next(1, 100);
        decimal factor = 1m;
        if (roll > 95 - crit)
            factor = 2m;
        else if (roll <= Math.Max(Math.Min(targetLevel.Value - attackerLevel.Value, 0), 1))
            factor = 0m;
        else if (roll <= 10)
            factor = 0.5m;
        else if (roll > 61)
            factor = 1.25m;
        return factor;
    }

    public static int GetDamage(IRandom rng, Entity attacker, Entity target)
    {
        decimal factor = GetDamageFactor(rng, attacker, target);
        if (!attacker.TryGetComponent<Attack>(out Attack attack))
            throw new Exceptions.MissingComponentException();
        int damage = 0;
        for (int i = 0; i < attack.Dice; i++)
            damage += (int)rng.Next(1, attack.Sides);
        damage = (int)(damage * factor);
        return damage;
    }

    public static decimal GetInitiative(Entity entity, IRandom rng)
    {
        int speed = GetStat(entity, AbilityID.Speed);
        int evasion = GetStat(entity, AbilityID.Evasion);
        return (speed * (decimal)rng.NextDouble()) + (evasion * (decimal)rng.NextDouble());
    }

    public static EntityActions GetActions(Entity entity)
    {
        int entitySpeed = GetStat(entity, AbilityID.Speed);
        return new EntityActions(entitySpeed);
    }

    private static int GetTotal(decimal growth, int level)
    {
        return (int)(growth * (level - 1));
    }

    private static int GetTotal(decimal growth, uint level)
    {
        return (int)(growth * (level - 1));
    }
}
