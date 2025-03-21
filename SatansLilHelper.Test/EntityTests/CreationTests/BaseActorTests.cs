using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Utils;


namespace SatansLilHelper.Test.EntityTests.CreationTests;

public class BaseActorTests
{
    private readonly EntityStore World;
    private readonly Entity Entity;

    public BaseActorTests()
    {
        World = new();
        Entity = World.CreateEntity();
        Entities.BaseActor.Make(Entity);
    }

    [Test]
    [MethodDataSource(typeof(EntityTestData), nameof(EntityTestData.ResourceIDs))]
    public async Task ResourceTest(ResourceID id)
    {
        await Assert.That(Entity.GetRelation<ResourceStat, ResourceID>(id)).IsNotNull();
    }

    [Test]
    [MethodDataSource(typeof(EntityTestData), nameof(EntityTestData.AbilityIDs))]
    public async Task AbilityTest(AbilityID id)
    {
        await Assert.That(Entity.GetRelation<AbilityStat, AbilityID>(id)).IsNotNull();
    }

    [Test]
    public async Task ActionDelayTest()
    {
        await Assert.That(Entity.GetComponent<ActionDelay>()).IsNotNull();
    }

    [Test]
    public async Task LocationTest()
    {
        await Assert.That(Entity.GetComponent<Location>()).IsNotNull();
    }

    [Test]
    public async Task LevelTest()
    {
        await Assert.That(Entity.GetComponent<Level>()).IsNotNull();
    }

    [Test]
    public async Task ItemSlotsTest()
    {
        await Assert.That(Entity.GetComponent<ItemSlots>()).IsNotNull();
    }

    [Test]
    public async Task AttackTest()
    {
        await Assert.That(Entity.GetComponent<Attack>()).IsNotNull();
    }

    [Test]
    public async Task TagTest()
    {
        Tags baseActorTags = new();
        baseActorTags.Add<IsBlocking>();
        baseActorTags.Add<IsActor>();
        await Assert.That(Entity.Tags.HasAll(baseActorTags)).IsTrue();
        
    }
}
