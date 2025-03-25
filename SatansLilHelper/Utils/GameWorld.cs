using System.Collections.Generic;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Utils;

internal class GameWorld
{
    private List<BaseMap> maps;
    private int currentMap = 0;
    private MersenneTwister rng;
    private Point mapSize;

    public GameWorld(MersenneTwister rng, Point mapSize)
    {
        this.rng = rng;
        this.mapSize = mapSize;
        maps = [new ForestMap(this.mapSize, this.rng, new Point(1280, 720), true)];
    }

    public BaseMap CurrentMap
    {
        get { return maps[currentMap]; }
    }

    public bool IsPlayerNext
    {
        get { return maps[currentMap].IsPlayerNext; }
    }

    /// <summary>
    /// Used to get the next actor in the initiative order.
    /// </summary>
    /// <returns>The <c>Entity</c> to act next.</returns>
    public Entity GetNextActor()
    {
        return maps[currentMap].GetNextActor();
    }
}
