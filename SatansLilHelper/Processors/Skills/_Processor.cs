using System;
using Friflo.Engine.ECS;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Processors;

internal static partial class Skills
{
    public static void Process(Entity actor, Entity skill)
    {
        SkillID skillID = SkillID.Charge;
    }

    public static void Default(Entity actor)
    {
        throw new NotImplementedException();
    }
}
