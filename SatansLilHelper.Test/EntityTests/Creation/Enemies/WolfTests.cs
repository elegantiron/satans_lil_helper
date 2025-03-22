using Friflo.Engine.ECS;
using SatansLilHelper.Components;
using SatansLilHelper.Test.EntityTests.Creation;
using SatansLilHelper.Utils;
using TUnit.Core;

namespace SatansLilHelper.Test.EntityTests.Creation.Enemies;

[DependsOn(typeof(BaseActorTests))]
internal class Wolf
{
    private EntityStore World;
    private Entity Entity;

    private MersenneTwister Rng;

    public Wolf()
    {
        World = new();
        Rng = new(1);
        Entity = World.CreateEntity();
        Entities.Enemies.Wolf(Rng, Entity);
    }

    [Test]
    public async Task Name()
    {
        await Assert.That(Entity.GetComponent<EntityName>().value).IsEqualTo("wolf");
    }

    [Test, Repeat(5)]
    public async Task ActionDelay()
    {
        await Assert.That(Entity.GetComponent<ActionDelay>().Value).IsBetween(1, 15);
    }

    [Test, Repeat(5)]
    public async Task Health()
    {
        await Assert
            .That(Entity.GetRelation<ResourceStat, ResourceID>(ResourceID.Health).Cur)
            .IsBetween(17, 24);
    }

    [Test]
    [Arguments(AbilityID.Strength, 2, 0)]
    [Arguments(AbilityID.PhysicalDefense, 5, 0)]
    [Arguments(AbilityID.Crit, 1, 0)]
    [Arguments(AbilityID.Speed, 3, 0)]
    [Arguments(AbilityID.Vision, 8, 0)]
    [DisplayName("$ability")]
    public async Task Abilities(AbilityID ability, int basis, decimal growth)
    {
        await Assert
            .That(Entity.GetRelation<AbilityStat, AbilityID>(ability).Basis)
            .IsEqualTo(basis);
        await Assert
            .That(Entity.GetRelation<AbilityStat, AbilityID>(ability).Growth)
            .IsEqualTo(growth);
    }

    [Test]
    public async Task Texture()
    {
        await Assert.That(Entity.GetComponent<TextureIndex>().Index).IsEqualTo(TextureID.Wolf);
    }
}
