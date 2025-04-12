using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Entities;

internal static class Skills
{
    public static Entity ShieldUp(Entity entity)
    {
        entity.Add(new EntityName("Shield Up"), Tags.Get<Activatable, Skill>());
        return entity;
    }

    public static Entity ShieldUp(Entity entity, Entity knower)
    {
        ShieldUp(entity);
        entity.Add(new Grimoire(knower));
        return entity;
    }

    public static Entity Charge(Entity entity)
    {
        entity.Add(
            new EntityName("Charge"),
            Tags.Get<Activatable, DamagesInterruptor, Interruptible, MovesActor, Skill>()
        );
        entity.Add(new Radius(0), Tags.Get<Targetable>());
        return entity;
    }

    public static Entity Charge(Entity entity, Entity knower)
    {
        Charge(entity);
        entity.Add(
            new Range(
                (Entity ent) =>
                {
                    return EntityCalcs.GetStat(ent, AbilityID.Speed) * 2;
                },
                knower
            ),
            new Grimoire(knower)
        );
        return entity;
    }
}
