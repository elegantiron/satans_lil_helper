using Friflo.Engine.ECS;
using SatansLilHelper.Constants;
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
    void Process()
    {
        Location playerLoc = Player.GetComponent<Location>();
        while (!IsPlayerNext)
        {
            Entity entity = GetNextActor();
            if (!entity.Tags.Has<Alive>())
                continue;
            ActionStack.AddAction(AI.Process(entity, playerLoc, CurrentMap, Generator));
        }
    }
}
