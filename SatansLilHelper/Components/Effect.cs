using Friflo.Engine.ECS;

using SatansLilHelper.Constants;

namespace SatansLilHelper.Components;

[ComponentKey("effect-power")]
internal struct Effect(ItemEffect effect, int value) : IComponent
{
    public int Value = value;
    public ItemEffect Type = effect;
}
