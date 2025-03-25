using System;
using System.Collections.Generic;
using System.Linq;
using Friflo.Engine.ECS;
using SatansLilHelper.Components;

namespace SatansLilHelper.Utils;

public class InitiativeTracker(ArchetypeQuery query, MersenneTwister rng)
{
    private Stack<Entity> _initiativeStack = new();
    private ArchetypeQuery _query = query;
    private MersenneTwister _rng = rng;

    private void CalculateInitiative()
    {
        Entity[] entities = new Entity[_query.Count];
        _query.ToEntityList().CopyTo(entities, 0);
        SortedList<decimal, Entity> list = [];
        foreach (Entity entity in entities)
        {
            decimal initiative = EntityCalcs.GetInitiative(entity, _rng);
            list.Add(initiative, entity);
        }
        foreach ((decimal _, Entity entity) in list.Reverse())
            _initiativeStack.Push(entity);
    }

    public Entity GetNextActor()
    {
        if (_initiativeStack.Count == 0)
            CalculateInitiative();
        while (true)
        {
            if (!_initiativeStack.Peek().IsNull)
                return _initiativeStack.Pop();
            _initiativeStack.Pop();
        }
    }

    public bool IsPlayerNext
    {
        get
        {
            if (_initiativeStack.Count == 0)
                CalculateInitiative();
            return _initiativeStack.Peek().Tags.Has<IsPlayer>();
        }
    }
}
