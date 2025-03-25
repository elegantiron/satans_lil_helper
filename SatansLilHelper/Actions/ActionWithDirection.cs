using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal abstract class ActionWithDirection : IAction
{
    protected Location Destination,
        Origin;
    protected bool IsBlocked = false,
        IsOffMap = false;
    protected Entity? _target = null;
    protected Entity _entity;
    protected List<LogMessage> _messages;

    public ActionWithDirection(
        Entity entity,
        Point direction,
        BaseMap gameMap,
        ArchetypeQuery query,
        bool isPlayer
    )
    {
        _entity = entity;
        Origin = entity.GetComponent<Location>();
        Destination = new Location(Origin.X + direction.X, Origin.Y + direction.Y);
        ArchetypeQuery Query = entity
            .Store.Query()
            .HasValue<Location, (int X, int Y)>((Destination.X, Destination.Y))
            .AllTags(Tags.Get<IsAlive>());
        if (Query.Count > 0)
        {
            IsBlocked = true;
            foreach (Entity ent in Query.Entities)
            {
                _target = ent;
                break;
            }
        }
        if (
            Destination.X < 0
            || Destination.Y < 0
            || Destination.X >= gameMap.Tiles.GetLength(0)
            || Destination.Y >= gameMap.Tiles.GetLength(1)
        )
        {
            IsOffMap = true;
            IsBlocked = true;
        }
        if (!IsOffMap && !gameMap.Tiles[Destination.X, Destination.Y].Walkable)
            IsBlocked = true;
    }

    public Entity Entity => throw new System.NotImplementedException();

    public abstract void Perform();

    public abstract void Rewind();
}
