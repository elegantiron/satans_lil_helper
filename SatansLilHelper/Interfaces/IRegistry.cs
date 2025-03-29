using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Interfaces;

internal interface IRegistry : IUpdateable
{
    BaseMap CurrentMap { get; }
    Entity Player { get; }
    bool IsPlayerNext { get; }
    Entity GetNextActor();
    void IUpdateable.Update(GameTime gameTime)
    {
        Location playerLoc = Player.GetComponent<Location>();
        while (!IsPlayerNext)
        {
            Entity entity = GetNextActor();
            Location entLoc = entity.GetComponent<Location>();
            Point entPos = new(entLoc.X, entLoc.Y);
            int radius = Math.Min(
                EntityCalcs.GetStat(entity, AbilityID.Vision),
                EntityCalcs.GetStat(entity, AbilityID.LightRadius)
            );

            List<Point> visibleTiles = ShadowCast.GetVisibleTiles(CurrentMap, entPos, radius);

            if (visibleTiles.Contains(new Point(playerLoc.X, playerLoc.Y))) { }
            else { }
        }
    }
}
