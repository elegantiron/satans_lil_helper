using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Types;

internal struct EntityTurn : IAction
{
    private Entity _entity;
    private List<IAction> _actions;
    private int _moves;
    private int _movesMax;
    private int _attacks;
    private int _attacksMax;
    private bool _swift = false;
    public readonly Entity? Entity => _entity;

    public EntityTurn(Entity entity)
    {
        _entity = entity;
        _actions = [];
        _moves = _attacks = 0;
        _attacksMax = EntityCalcs.GetStat(_entity, AbilityID.Speed);
    }

    public readonly void Perform()
    {
        if (_actions.Count < 1)
            return;
        foreach (IAction action in _actions)
            action.Perform();
    }

    public readonly void Rewind()
    {
        if (_actions.Count < 1)
            return;
        foreach (IAction action in _actions)
            action.Rewind();
    }

    public void AddAction(IAction action)
    {
        if (action.Entity != Entity)
            throw new ArgumentException(
                "You cannot add an Entity's action to another Entity's turn."
            );
        if (action is IMoveAction && _moves < _movesMax)
        {
            _actions.Add(action);
            _moves++;
        }
        else if (action is IAttackAction && _attacks < _attacksMax)
        {
            _actions.Add(action);
            _attacks++;
        }
        else if (action is ISwiftAction && !_swift)
        {
            _actions.Add(action);
            _swift = true;
        }
        else if (action is IFreeAction)
            _actions.Add(action);
    }

    public void RemoveLastAction()
    {
        if (_actions.Count < 1)
            return;
        if (_actions[^1] is IMoveAction)
            _moves--;
        else if (_actions[^1] is IAttackAction)
            _attacks--;
        else if (_actions[^1] is ISwiftAction)
            _swift = false;
        _actions.RemoveAt(_actions.Count - 1);
    }
}
