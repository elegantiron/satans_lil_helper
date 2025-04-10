using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

internal abstract class ActionWithDirection : IAction
{
    protected Location Destination,
        Origin;
    protected bool _isBlocked = false,
        _isOffMap = false,
        _isPlayer;
    protected Entity? _target = null;
    protected Entity _entity;
    protected List<LogMessage> _messages = [];
    protected bool _successful = false;

    public ActionWithDirection(Entity entity, Point direction, BaseMap gameMap, bool isPlayer)
    {
        _isPlayer = isPlayer;
        _entity = entity;
        Origin = entity.GetComponent<Location>();
        Destination = new Location(Origin.X + direction.X, Origin.Y + direction.Y);
        ArchetypeQuery Query = entity
            .Store.Query()
            .HasValue<Location, (int X, int Y)>((Destination.X, Destination.Y))
            .AllTags(Tags.Get<Alive>());
        if (Query.Count > 0)
        {
            _isBlocked = true;
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
            _isOffMap = true;
            _isBlocked = true;
        }
        if (!_isOffMap && !gameMap.Tiles[Destination.X, Destination.Y].Walkable)
            _isBlocked = true;
    }

    public bool Successful => _successful;
    public Entity? Entity => _entity;

    public virtual void Perform()
    {
        ((IMessageSender)this).SendMessages();
    }

    public virtual void Rewind()
    {
        ((IMessageSender)this).RetractMessages();
    }
}
