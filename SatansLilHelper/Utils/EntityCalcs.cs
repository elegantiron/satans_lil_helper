using System;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Types;

namespace SatansLilHelper.Utils;

public static class EntityCalcs
{
    public static int GetStat(Entity entity, AbilityID ability)
    {
        if (!entity.TryGetRelation(ability, out AbilityStat stat))
            throw new Exceptions.MissingComponentException();
        if (!entity.TryGetComponent<Level>(out Level level))
            throw new Exceptions.MissingComponentException();
        int total = (int)(stat.Basis + stat.Growth * (level.Value - 1));

        return total;
    }

    public static int GetStat(Entity entity, ResourceID resource)
    {
        if (!entity.TryGetRelation(resource, out ResourceStat stat))
            throw new Exceptions.MissingComponentException();
        if (!entity.TryGetComponent<Level>(out Level level))
            throw new Exceptions.MissingComponentException();

        int total = (int)(stat.Basis + stat.Growth * (level.Value - 1));

        return total;
    }

    public static decimal GetDamageFactor(MersenneTwister rng, Entity attacker, Entity target)
    {
        //throw new NotImplementedException();
        if (!attacker.TryGetComponent<Level>(out Level attackerLevel))
            throw new Exceptions.MissingComponentException();
        if (!target.TryGetComponent<Level>(out Level targetLevel))
            throw new Exceptions.MissingComponentException();
        int crit = GetStat(attacker, AbilityID.Crit);
        int roll = rng.Next(1, 100);
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

    public static uint GetDamage(MersenneTwister rng, Entity attacker, Entity target)
    {
        decimal factor = GetDamageFactor(rng, attacker, target);
        if (!attacker.TryGetComponent<Attack>(out Attack attack))
            throw new Exceptions.MissingComponentException();
        uint damage = 0;
        for (int i = 0; i < attack.Dice; i++)
            damage += rng.Next(1, attack.Sides);
        damage = (uint)(damage * factor);
        return damage;
    }

    public static decimal GetInitiative(Entity entity, MersenneTwister rng)
    {
        int speed = GetStat(entity, AbilityID.Speed);
        int evasion = GetStat(entity, AbilityID.Evasion);
        return speed * rng.NextDecimal() + evasion * rng.NextDecimal();
    }

    public static EntityActions GetActions(Entity entity)
    {
        int entitySpeed = GetStat(entity, AbilityID.Speed);
        return new EntityActions(entitySpeed);
    }
}
