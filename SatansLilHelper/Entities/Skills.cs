using Friflo.Engine.ECS;

using SatansLilHelper.Components;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Entities;

internal static class Skills
{
    private static void Base(Entity entity)
    {
        entity.AddTag<Skill>();
    }

    private static void Base(Entity entity, Entity parent)
    {
        entity.AddComponent(new Grimoire(parent));
    }

    public static void ShieldUp(Entity entity)
    {
        Base(entity);
        entity.AddComponent(new EntityName("Shield Up"));
    }

    public static void ShieldUp(Entity entity, Entity parent)
    {
        Base(entity, parent);
        ShieldUp(entity);
    }

    public static void Charge(Entity entity)
    {
        Base(entity);
        entity.AddComponent(new EntityName("Charge"));
    }

    public static void Charge(Entity entity, Entity parent)
    {
        Base(entity, parent);
        Charge(entity);
    }
}
