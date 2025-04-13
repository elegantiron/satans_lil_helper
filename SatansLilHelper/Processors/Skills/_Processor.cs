using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
