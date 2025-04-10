using System;
using System.Collections.Generic;

using Friflo.Engine.ECS;

using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Utils;

namespace SatansLilHelper.Types;

internal class EntityTurn : IAction
{
    private Entity _entity;
    private Stack<IAction> _actions;
    private int _moves;
    private int _movesMax;
    private int _attacks;
    private int _attacksMax;
    private bool _swift = true;
    private bool _finished = false;
    public Entity? Entity => _entity;
    public bool Successful => true;

    public EntityTurn(Entity entity)
    {
        _entity = entity;
        _actions = [];
        _moves = _attacks = 0;
        _movesMax = EntityCalcs.GetStat(_entity, AbilityID.Speed);
        _attacksMax = 1;
    }

    public void Perform()
    {
        if (_actions.Count < 1)
            return;
        foreach (IAction action in _actions)
            action.Perform();
    }

    public void Rewind()
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
            _actions.Push(action);
            _moves++;
        }
        else if (action is IAttackAction && _attacks < _attacksMax)
        {
            action.Perform();
            _actions.Push(action);
            _attacks++;
        }
        else if (action is ISwiftAction && !_swift)
        {
            action.Perform();
            _actions.Push(action);
            _swift = false;
        }
        else if (action is IFreeAction)
        {
            action.Perform();
            _actions.Push(action);
        }
    }

    public void Finish()
    {
        _finished = true;
    }

    public bool HasActions => (HasMoves || HasAttacks || HasSwift) && !_finished;

    public bool HasMoves => _moves < _movesMax;

    public bool HasAttacks => _attacks < _attacksMax;

    public bool HasSwift => _swift;
}
