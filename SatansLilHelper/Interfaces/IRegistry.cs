using System;
using System.Collections.Generic;
using System.Diagnostics;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Actions;
using SatansLilHelper.Components;
using SatansLilHelper.Constants;
using SatansLilHelper.Types;
using SatansLilHelper.Utils;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Interfaces;

internal interface IRegistry
{
    BaseMap CurrentMap { get; }
    Entity Player { get; }
    IRandom Generator { get; }
    bool IsPlayerNext { get; }
    Entity GetNextActor();
    ActionStack ActionStack { get; }
    void Update(GameTime gameTime)
    {
        Location playerLoc = Player.GetComponent<Location>();
        while (!IsPlayerNext)
        {
            Entity entity = GetNextActor();
            EntityTurn turn = new(entity);
            while (turn.HasActions)
            {
                Location entLoc = entity.GetComponent<Location>();
                Point entPos = new(entLoc.X, entLoc.Y);
                int radius = Math.Min(
                    EntityCalcs.GetStat(entity, AbilityID.Vision),
                    EntityCalcs.GetStat(entity, AbilityID.LightRadius)
                );

                List<Point> visibleTiles = ShadowCast.GetVisibleTiles(CurrentMap, entPos, radius);
                List<(int X, int Y)> path;

                if (visibleTiles.Exists(test => test.X == playerLoc.X && test.Y == playerLoc.Y))
                {
                    path = Pathfinders.Dijkstra(entPos, (playerLoc.X, playerLoc.Y), CurrentMap);
                    Debug.WriteLine($"{path[0].X} {path[0].Y}; {entPos.X} {entPos.Y}");
                    if (turn.HasSwift && false)
                    {
                        // Do swift action
                    }
                    else if (path.Count > 1 && turn.HasMoves)
                    {
                        turn.AddAction(
                            new MoveAction(
                                entity,
                                new(path[0].X - entPos.X, path[0].Y - entPos.Y),
                                CurrentMap,
                                false
                            )
                        );
                    }
                    else if (path.Count == 1 && turn.HasAttacks)
                    {
                        turn.AddAction(
                            new MoveAction(
                                entity,
                                new(path[0].X - entPos.X, path[0].Y - entPos.Y),
                                CurrentMap,
                                false
                            )
                        );
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
                        int x = Generator.NextDouble() < 0.5 ? 1 : 0;
                        x *= Generator.NextDouble() < 0.5 ? -1 : 1;
                        int y = Generator.NextDouble() < 0.5 ? 1 : 0;
                        y *= Generator.NextDouble() < 0.5 ? -1 : 1;
                        turn.AddAction(new MoveAction(entity, new Point(x, y), CurrentMap, false));
                    }
                    else
                        break;
                }
            }
            ActionStack.AddAction(turn);
        }
    }
}
