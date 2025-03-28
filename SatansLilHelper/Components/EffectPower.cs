using Friflo.Engine.ECS;
using Friflo.Json.Fliox;

namespace SatansLilHelper.Components;

[ComponentKey("effect-power")]
internal struct EffectPower(int value) : IComponent
{
    [Serialize]
    public int Value = value;
}
