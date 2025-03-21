using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Entities;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Test.EntityTests.CreationTests.ProfessionTests;

public class WarriorTests
{
    private readonly EntityStore World;
    private readonly Entity Entity;

    public WarriorTests()
    {
        World = new();
        Entity = World.CreateEntity();
        Professions.Warrior(Entity);
    }
    [Test]
    public async Task ActionDelayTest()
    {
        await Assert.That(Entity.GetComponent<ActionDelay>().Value).IsEqualTo(0);
    }
    [Test]
    public async Task LevelTest()
    {
        await Assert.That(Entity.GetComponent<Level>().Value).IsEqualTo(1);
    }
    [Test]
    public async Task ItemSlotsTest()
    {
        ItemType expectedResult = ItemType.Warrior;
        await Assert.That(Entity.GetComponent<ItemSlots>().Types).IsEqualTo(expectedResult);
    }
    [Test]
    [Arguments(ResourceID.Health, 60, 10)]
    [Arguments(ResourceID.Mana, 5, 1)]
    public async Task ResourceTest(ResourceID resource, int basis, decimal growth)
    {
        await Assert.That(Entity.GetRelation<ResourceStat, ResourceID>(resource).Basis).IsEqualTo(basis);
        await Assert.That(Entity.GetRelation<ResourceStat, ResourceID>(resource).Growth).IsEqualTo(growth);
    }

    [Test]
    [Arguments(AbilityID.Strength, 5, 0.5)]
    [Arguments(AbilityID.MagicPower, 0, 0.25)]
    [Arguments(AbilityID.PhysicalDefense, 7, 0.5)]
    [Arguments(AbilityID.MagicDefense, 2, 0.25)]
    [Arguments(AbilityID.Evasion, 0, 0)]
    [Arguments(AbilityID.Crit, 0, 0)]
    [Arguments(AbilityID.Speed, 2, 0)]
    [Arguments(AbilityID.Vision, 7, 0)]
    [Arguments(AbilityID.LightRadius, 0, 0)]
    public async Task AbilityTest(AbilityID ability, int basis, decimal growth)
    {
        await Assert.That(Entity.GetRelation<AbilityStat, AbilityID>(ability).Basis).IsEqualTo(basis);
        await Assert.That(Entity.GetRelation<AbilityStat, AbilityID>(ability).Growth).IsEqualTo(growth);
    }
       
}
