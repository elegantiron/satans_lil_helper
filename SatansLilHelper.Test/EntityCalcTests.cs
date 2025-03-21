using Friflo.Engine.ECS;
using SatansLilHelper.Entities;

namespace SatansLilHelper.Test;

public record EntityTestData(Entity Attacker, Entity Defender);

[DependsOn(typeof(EntityCreationTests.ActorCreationTests))]
public class EntityCalcTests
{
    private readonly EntityStore World;
    private readonly Entity Player;

    public EntityCalcTests()
    {
        World = new EntityStore();
        Player = World.CreateEntity();
        Professions.Warrior(Player);
    }

    [Test]
    public async Task NoOpTest()
    {
        var result = 0;
        await Assert.That(result).IsEqualTo(0);
    }
}
