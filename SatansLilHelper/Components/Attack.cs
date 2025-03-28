using Friflo.Engine.ECS;
using Friflo.Json.Fliox;

namespace SatansLilHelper.Components;

[ComponentKey("attack")]
public struct Attack(uint dice, uint sides, uint bonus = 0) : IComponent
{
    [Serialize]
    public uint Dice = dice,
        Sides = sides,
        Bonus = bonus;
}
