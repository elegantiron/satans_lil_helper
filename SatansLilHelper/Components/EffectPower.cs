using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

internal struct EffectPower(int value) : IComponent
{
    public int Value = value;
}
