using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using SatansLilHelper.Constants;
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
    private bool _swift = true;
    private bool _finished = false;
    public readonly Entity? Entity => _entity;

    public EntityTurn(Entity entity)
    {
        _entity = entity;
        _actions = [];
        _moves = _attacks = 0;
        _movesMax = EntityCalcs.GetStat(_entity, AbilityID.Speed);
        _attacksMax = 1;
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
            action.Perform();
            _actions.Add(action);
            _moves++;
        }
        else if (action is IAttackAction && _attacks < _attacksMax)
        {
            action.Perform();
            _actions.Add(action);
            _attacks++;
        }
        else if (action is ISwiftAction && !_swift)
        {
            action.Perform();
            _actions.Add(action);
            _swift = false;
        }
        else if (action is IFreeAction)
        {
            action.Perform();
            _actions.Add(action);
        }
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
            _swift = true;
        _actions[^1].Rewind();
        _actions.RemoveAt(_actions.Count - 1);
    }

    public void Finish()
    {
        _finished = true;
    }

    public readonly bool HasActions
    {
        get { return (HasMoves || HasAttacks || HasSwift) && !_finished; }
    }

    public readonly bool HasMoves
    {
        get { return _moves < _movesMax; }
    }

    public readonly bool HasAttacks
    {
        get { return _attacks < _attacksMax; }
    }

    public readonly bool HasSwift
    {
        get { return _swift; }
    }
}
