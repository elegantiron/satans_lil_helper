using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Entities;

public static class Enemies
{
    public static void Wolf(MersenneTwister rng, Entity entity)
    {
        Bases.Actor(entity);
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
