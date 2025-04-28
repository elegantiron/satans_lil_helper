using Friflo.Engine.ECS;
using SatansLilHelper.Constants;

namespace SatansLilHelper.ECSComponents;

internal struct RandomEffect : IComponent
{
    public ItemEffect Effect;
    public int Dice,
        Sides,
        Bonus;

    public RandomEffect(ItemEffect effect, int dice, int sides, int bonus = 0)
    {
        Effect = effect;
        Dice = dice;
        Sides = sides;
        Bonus = bonus;
    }
}
