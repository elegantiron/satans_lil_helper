using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

internal struct OldTargetable(int range, int radius) : IComponent
{
    public int Range = range,
        Radius = radius;
}
