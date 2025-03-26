using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Friflo.Engine.ECS;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal class ConfusedAction : IAction
{
    private byte[] _preState,
        _postState;
    private BumpAction _action;
    private Entity _entity;
    private bool _isPlayer;

    public ConfusedAction(
        Entity entity,
        BaseMap gameMap,
        MersenneTwister rng,
        bool isPlayer = false
    )
    {
        _preState = rng.GetState();
        _entity = entity;
        _isPlayer = isPlayer;
        int X = rng.Next() < 0.5 ? 1 : 0;
        X *= rng.Next() < 0.5 ? -1 : 1;
        int Y = rng.Next() < 0.5 ? 1 : 0;
        Y *= rng.Next() < 0.5 ? -1 : 1;
    }

    public Entity Entity
    {
        get { return _entity; }
    }

    public void Perform()
    {
        throw new NotImplementedException();
    }

    public void Rewind()
    {
        throw new NotImplementedException();
    }
}
