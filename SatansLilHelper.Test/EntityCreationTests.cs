using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Entities;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Test;

public class EntityCreationTests
{
    public class ActorCreationTests
    {
        private readonly EntityStore World;
        private readonly Entity Entity;

        public ActorCreationTests()
        {
            World = new();
            Entity = World.CreateEntity();
            BaseActor.Make(Entity);
        }

        [Test]
        [MethodDataSource(typeof(EntityCreationData), nameof(EntityCreationData.ResourceIDs))]
        public async Task ResourceTest(ResourceID id)
        {
            await Assert.That(Entity.GetRelation<ResourceStat, ResourceID>(id)).IsNotNull();
        }

        [Test]
        [MethodDataSource(typeof(EntityCreationData), nameof(EntityCreationData.AbilityIDs))]
        public async Task AbilityTest(AbilityID id)
        {
            await Assert.That(Entity.GetRelation<AbilityStat, AbilityID>(id)).IsNotNull();
        }
    }

    [DependsOn(typeof(ActorCreationTests))]
    public class WarriorCreationTests
    {
        private readonly EntityStore World;
        private readonly Entity Entity;

        public WarriorCreationTests()
        {
            World = new();
            Entity = World.CreateEntity();
            Professions.Warrior(Entity);
        }

        [Test]
        public async Task StrengthTest()
        {
            await Assert.That(EntityCalcs.GetStat(Entity, AbilityID.Strength)).IsEqualTo(5);
        }
    }
}

public static class EntityCreationData
{
    public static IEnumerable<ResourceID> ResourceIDs()
    {
        foreach (ResourceID id in Enum.GetValues(typeof(ResourceID)))
        {
            yield return id;
        }
    }

    public static IEnumerable<AbilityID> AbilityIDs()
    {
        foreach (AbilityID id in Enum.GetValues(typeof(AbilityID)))
        {
            yield return id;
        }
    }
}
