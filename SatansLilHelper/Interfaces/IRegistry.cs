using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.ECSComponents;
using SatansLilHelper.Processors;
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
            ActionStack.AddAction(AI.Process(entity, playerLoc, CurrentMap, Generator));
        }
    }
}
