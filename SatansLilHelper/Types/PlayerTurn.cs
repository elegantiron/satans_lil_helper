using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using SatansLilHelper.Actions;
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
    public Entity? Entity => _entity;

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

    public bool AddAction(BumpAction action)
    {
        return AddAction(action.Action);
    }

    public bool AddAction(IAction action)
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
            if (action.Successful)
                _moves++;
            return action.Successful;
        }
        else if (action is IAttackAction && _attacks < _attacksMax)
        {
            action.Perform();
            _future.Clear();
            _history.Push(action);
            if (action.Successful)
                _attacks++;
            return action.Successful;
        }
        else if (action is ISwiftAction && !_swift)
        {
            action.Perform();
            _future.Clear();
            _history.Push(action);
            if (action.Successful)
                _swift = false;
            return action.Successful;
        }
        else if (action is IFreeAction)
        {
            action.Perform();
            _future.Clear();
            _history.Push(action);
            return action.Successful;
        }
        return false;
    }

    public bool Undo()
    {
        if (!_history.TryPop(out IAction? action))
            return false;
        action.Rewind();
        _future.Push(action);
        if (!action.Successful)
            return true;
        if (action is IMoveAction)
            _moves--;
        else if (action is IAttackAction)
            _attacks--;
        else if (action is ISwiftAction)
            _swift = true;
        return true;
    }

    public bool Redo()
    {
        if (!_future.TryPop(out IAction? action))
            return false;
        if (action.Successful)
        {
            if (action is IMoveAction)
                _moves++;
            else if (action is IAttackAction)
                _attacks++;
            else if (action is ISwiftAction)
                _swift = false;
        }
        action.Perform();
        _history.Push(action);
        return true;
    }

    public bool IsFinished => _finished;

    public int MovesLeft => _movesMax - _moves;

    public int MovesUsed => _moves;

    public int MovesMax => _movesMax;

    public int AttacksUsed => _attacks;

    public int AttacksMax => _attacksMax;
    public bool Successful => true;

    public void Finish()
    {
        _finished = true;
    }
}
