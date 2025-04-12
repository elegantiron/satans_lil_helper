using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Entities;

internal static class Skills
{
    private static void Base(Entity entity, Entity parent)
    {
        entity.AddComponent(new Grimoire(parent));
    }

    public static void ShieldUp(Entity entity)
    {
        entity.Add(new EntityName("Shield Up"), Tags.Get<Activatable, Skill>());
    }

    public static void Charge(Entity entity)
    {
        entity.Add(
            new EntityName("Charge"),
            new Targetable(3, 0),
            Tags.Get<Activatable, DamagesInterruptor, Interruptible, MovesActor, Skill>()
        );
    }
}
