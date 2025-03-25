using Friflo.Engine.ECS;

namespace SatansLilHelper.Types;

public struct AttackResult
{
    public byte[] OriginalState,
        NewState;
    public bool IsKill;
    public Entity Attacker,
        Target;
}
