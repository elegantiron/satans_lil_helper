using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Test.EntityTests.Creation;

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
    public async Task Resources(ResourceID id)
    {
        await Assert.That(Entity.GetRelation<ResourceStat, ResourceID>(id)).IsNotNull();
    }

    [Test]
    [MethodDataSource(typeof(EntityTestData), nameof(EntityTestData.AbilityIDs))]
    public async Task Abilities(AbilityID id)
    {
        await Assert.That(Entity.GetRelation<AbilityStat, AbilityID>(id)).IsNotNull();
    }

    [Test]
    public async Task ActionDelay()
    {
        await Assert.That(Entity.GetComponent<ActionDelay>()).IsNotNull();
    }

    [Test]
    public async Task Location()
    {
        await Assert.That(Entity.GetComponent<Location>()).IsNotNull();
    }

    [Test]
    public async Task Level()
    {
        await Assert.That(Entity.GetComponent<Level>()).IsNotNull();
    }

    [Test]
    public async Task ItemSlots()
    {
        await Assert.That(Entity.GetComponent<ItemSlots>()).IsNotNull();
    }

    [Test]
    public async Task Attack()
    {
        await Assert.That(Entity.GetComponent<Attack>()).IsNotNull();
    }

    [Test]
    public async Task Tags()
    {
        Tags baseActorTags = new();
        baseActorTags.Add<IsBlocking>();
        baseActorTags.Add<IsActor>();
        await Assert.That(Entity.Tags.HasAll(baseActorTags)).IsTrue();
    }

    [Test]
    public async Task TextureIndex()
    {
        await Assert.That(Entity.GetComponent<TextureIndex>()).IsNotNull();
    }
}
