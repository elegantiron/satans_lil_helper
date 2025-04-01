using System;
using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Actions;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Interfaces;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.AI;

internal static partial class AI
{
    private static EntityTurn Melee(Entity entity, Location playerLoc, BaseMap map, IRandom rng)
    {
        EntityTurn turn = new();
        Location entLoc = entity.GetComponent<Location>();
        Point entPos = new(entLoc.X, entLoc.Y);
        int radius = Math.Min(
            EntityCalcs.GetStat(entity, AbilityID.Vision),
            EntityCalcs.GetStat(entity, AbilityID.LightRadius)
        );

        List<Point> visibleTiles = ShadowCast.GetVisibleTiles(map, entPos, radius);

        if (visibleTiles.Exists(test => test.X == playerLoc.X && test.Y == playerLoc.Y))
        {
            List<(int X, int Y)> path = Pathfinders.Dijkstra(entPos, playerLoc, map);
            if (turn.HasSwift && false)
            {
                // Do swift action
            }
            else if (path.Count > 1 && turn.HasMoves)
            {
                turn.AddAction(
                    new MoveAction(
                        entity,
                        new Point(path[0].X - entPos.X, path[0].Y - entPos.Y),
                        map,
                        false
                    )
                );
            }
            else if (path.Count == 1 && turn.HasAttacks)
            {
                turn.AddAction(
                    new MeleeAction(
                        entity,
                        new Point(path[0].X - entPos.X, path[0].Y - entPos.Y),
                        map,
                        rng,
                        false
                    )
                );
                turn.Finish();
            }
            else
            {
                turn.Finish();
            }
        }
        else
        {
            if (turn.HasMoves)
            {
                int x = rng.NextDouble() < 0.5 ? 1 : 0;
                x *= rng.NextDouble() < 0.5 ? -1 : 1;
                int y = rng.NextDouble() < 0.5 ? 1 : 0;
                y *= rng.NextDouble() < 0.5 ? -1 : 1;
                turn.AddAction(new MoveAction(entity, new Point(x, y), map, false));
            }
        }
        return turn;
    }
}
