using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;

namespace SatansLilHelper.Utils;

public static class EntityCalcs
{
    public static int GetStat(Entity entity, AbilityID ability)
    {
        if (!entity.TryGetRelation(ability, out AbilityStat stat))
            throw new Exceptions.MissingComponentException();
        if (!entity.TryGetComponent<Level>(out Level level))
            throw new Exceptions.MissingComponentException();
        int total = (int)(stat.Basis + stat.Growth * level.Value);

        return total;
    }

    public static int GetStat(Entity entity, ResourceID resource)
    {
        if (!entity.TryGetRelation(resource, out ResourceStat stat))
            throw new Exceptions.MissingComponentException();
        if (!entity.TryGetComponent<Level>(out Level level))
            throw new Exceptions.MissingComponentException();

        int total = (int)(stat.Basis + stat.Growth * level.Value);

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
        else if (roll <= Math.Max(targetLevel.Value - attackerLevel.Value, 1))
            factor = 0m;
        else if (roll <= 10)
            factor = 0.5m;
        else if (roll > 61)
            factor = 1.25m;

        return factor;
    }

    public static int GetDamage(MersenneTwister rng, Entity attacker, Entity target)
    {
        decimal factor = GetDamageFactor(rng, attacker, target);
        if (!attacker.TryGetComponent<Attack>(out Attack attack))
            throw new Exceptions.MissingComponentException();
        int damage = 0;
        for (int i = 0; i < attack.Dice; i++)
            damage += rng.Next(1, attack.Sides);
        damage = (int)(damage * factor);
        return damage;
    }

    public static int DoNothing()
    {
        return 0;
    }
}
