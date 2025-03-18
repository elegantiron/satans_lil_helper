using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Actions;

public abstract class ActionWithDirection : BaseAction
{
    protected Location Destination,
        Origin;
    protected bool IsBlocked = false;
    protected Entity? TargetEntity = null;

    public ActionWithDirection(
        Entity entity,
        Point direction,
        BaseMap gameMap,
        ArchetypeQuery query,
        bool isPlayer
    )
        : base(entity, query, isPlayer)
    {
        Origin = entity.GetComponent<Location>();
        Destination = new Location(Origin.X + direction.X, Origin.Y + direction.Y);
        ArchetypeQuery Query = query.HasValue<Location, (int X, int Y)>(
            (Destination.X, Destination.Y)
        );
        if (Query.Count > 0)
        {
            IsBlocked = true;
            foreach (Entity ent in Query.Entities)
            {
                TargetEntity = ent;
                break;
            }
        }
        if (!gameMap.Tiles[Destination.X, Destination.Y].Walkable)
            IsBlocked = true;
    }
}
