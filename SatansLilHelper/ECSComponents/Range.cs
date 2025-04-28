using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;

namespace SatansLilHelper.ECSComponents;

internal struct Range : IComponent
{
    private Func<Entity, int>? _rangeFunc;
    private int? _rangeConst;
    private Entity? _knower;

    public Range(int range)
    {
        _rangeConst = range;
    }

    public Range(Func<Entity, int> rangeFunc, Entity knower)
    {
        _rangeFunc = rangeFunc;
        _knower = knower;
    }

    public readonly int Value
    {
        get
        {
            if (_rangeFunc != null && _knower != null)
                return (int)_rangeFunc((Entity)_knower);
            else if (_rangeConst != null)
                return (int)_rangeConst;
            else
                return 0;
        }
    }
}
