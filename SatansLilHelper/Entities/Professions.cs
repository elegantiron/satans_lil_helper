using Friflo.Engine.ECS;
using SatansLilHelper.Components;

namespace SatansLilHelper.Entities;

public static class Professions
{
    public static void Warrior(Entity entity)
    {
        entity.AddComponent(new ActionDelay { value = 0 });
    }
}
