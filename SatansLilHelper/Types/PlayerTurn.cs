using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Friflo.Engine.ECS;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Types;

internal class PlayerTurn : IAction
{
    private Entity _entity;
    private Stack<IAction> _history,
        _future;
    private int _moves,
        _movesMax,
        _attacks,
        _attacksMax;
    private bool _swift,
        _finished;
    public Entity? Entity
    {
        get { return _entity; }
    }

    public PlayerTurn(Entity entity)
    {
        _entity = entity;
        _history = new Stack<IAction>();
        _future = new Stack<IAction>();
        _attacksMax = 1;
        _movesMax = EntityCalcs.GetStat(_entity, Constants.AbilityID.Speed);
        _moves = _attacks = 0;
        _finished = false;
        _swift = true;
    }

    public void Perform()
    {
        if (_history.Count < 1)
            return;
        foreach (IAction action in _history)
            action.Perform();
    }

    public void Rewind()
    {
        if (_history.Count < 1)
            return;
        foreach (IAction action in _history)
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
            _future.Clear();
            _history.Push(action);
            _moves++;
        }
        else if (action is IAttackAction && _attacks < _attacksMax)
        {
            action.Perform();
            _future.Clear();
            _history.Push(action);
            _attacks++;
        }
        else if (action is ISwiftAction && !_swift)
        {
            action.Perform();
            _future.Clear();
            _history.Push(action);
            _swift = false;
        }
        else if (action is IFreeAction)
        {
            action.Perform();
            _future.Clear();
            _history.Push(action);
        }
    }

    public bool Undo()
    {
        if (!_history.TryPop(out IAction action))
            return false;
        if (action is IMoveAction)
            _moves--;
        else if (action is IAttackAction)
            _attacks--;
        else if (action is ISwiftAction)
            _swift = true;
        action.Rewind();
        _future.Push(action);
        return true;
    }

    public bool Redo()
    {
        if (!_future.TryPop(out IAction action))
            return false;
        if (action is IMoveAction)
            _moves++;
        else if (action is IAttackAction)
            _attacks++;
        else if (action is ISwiftAction)
            _swift = false;
        action.Perform();
        _history.Push(action);
        return true;
    }

    public bool IsFinished
    {
        get { return _finished; }
    }
}
