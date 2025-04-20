using System;
using System.Collections.Generic;
using System.Linq;
using Friflo.Engine.ECS;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.Utils;

internal class InitiativeTracker(ArchetypeQuery query, IRandom rng)
{
    private SortedList<decimal, Entity> _initiativeSortedList = [];
    private Stack<Entity> _initiativeStack = new();
    private Stack<Entity> _history = new();
    private ArchetypeQuery _query = query;
    private IRandom _rng = rng;

    private void CalculateInitiative()
    {
        SortedList<decimal, Entity> list = [];
        foreach (Entity entity in _query.ToEntityList().AsEnumerable())
        {
            decimal initiative = EntityCalcs.GetInitiative(entity, _rng) + (entity.Id / 10000m);
            list.Add(initiative, entity);
        }
        foreach ((decimal _, Entity entity) in list.Reverse())
            _initiativeStack.Push(entity);
    }

    public Entity GetNextActor()
    {
        if (_initiativeStack.Count == 0)
            CalculateInitiative();
        Entity actor = _initiativeStack.Pop();
        Entity newActor = _initiativeSortedList.Values[0];
        _history.Push(actor);
        return actor;
    }

    public bool IsPlayerNext
    {
        get
        {
            if (_initiativeStack.Count == 0)
                CalculateInitiative();
            return _initiativeStack.Peek().Tags.Has<Player>();
        }
    }

    public bool Rewind()
    {
        if (!_history.TryPop(out Entity entity))
            return false;
        _initiativeStack.Push(entity);
        return true;
    }
}
