using System;
using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

internal struct Radius : IComponent
{
    private Func<Entity, int>? _radiusFunc;
    private int? _radiusConst;
    private Entity? _knower;

    public Radius(int range)
    {
        _radiusConst = range;
    }

    public Radius(Func<Entity, int> radiusFunc, Entity knower)
    {
        _radiusFunc = radiusFunc;
        _knower = knower;
    }

    public readonly int Value
    {
        get
        {
            if (_radiusFunc != null && _knower != null)
                return _radiusFunc((Entity)_knower);
            else if (_radiusConst != null)
                return (int)_radiusConst;
            else
                return 0;
        }
    }
}
