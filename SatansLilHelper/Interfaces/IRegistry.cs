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
    void Update(GameTime gameTime, ActionStack actionStack)
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

                if (visibleTiles.Exists(test => test.X == playerLoc.X && test.Y == playerLoc.Y))
                {
                    Debug.WriteLine("I see the player");
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
            actionStack.AddAction(turn);
        }
    }
}
