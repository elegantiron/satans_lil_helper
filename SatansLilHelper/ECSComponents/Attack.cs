using Friflo.Engine.ECS;

namespace SatansLilHelper.ECSComponents;

[ComponentKey("attack")]
public struct Attack(uint dice, uint sides, uint bonus = 0) : IComponent
{
    public uint Dice = dice,
        Sides = sides,
        Bonus = bonus;
}
