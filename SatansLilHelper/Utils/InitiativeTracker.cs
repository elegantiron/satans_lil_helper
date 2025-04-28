using System.Collections.Generic;
using System.Linq;
using Friflo.Engine.ECS;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.Utils;

internal class InitiativeTracker(ArchetypeQuery query, IRandom rng)
{
    private SortedList<decimal, Entity> _initiativeStack = [];
    private Stack<Entity> _history = new(),
        _future = new();
    private ArchetypeQuery _query = query;
    private IRandom _rng = rng;

    private void CalculateInitiative()
    {
        foreach (Entity entity in _query.ToEntityList().AsEnumerable())
        {
            decimal initiative = EntityCalcs.GetInitiative(entity, _rng) + (entity.Id / 10000m);
            _initiativeStack.Add(initiative, entity);
        }
    }

    public Entity GetNextActor()
    {
        if (_initiativeStack.Count == 0)
            CalculateInitiative();
        (decimal key, Entity actor) = _initiativeStack.Last();
        _initiativeStack.Remove(key);
        _history.Push(actor);
        return actor;
    }

    public bool IsPlayerNext
    {
        get
        {
            if (_future.Count > 0)
                return _future.Peek().Tags.Has<Player>();
            if (_initiativeStack.Count == 0)
                CalculateInitiative();
            return _initiativeStack.Last().Value.Tags.Has<Player>();
        }
    }

    public bool Rewind()
    {
        if (!_history.TryPop(out Entity entity))
            return false;
        _future.Push(entity);
        return true;
    }
}
