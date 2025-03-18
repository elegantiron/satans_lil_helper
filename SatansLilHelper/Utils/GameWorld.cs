using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SatansLilHelper.Utils.GameMaps;

namespace SatansLilHelper.Utils;

public class GameWorld
{
    private List<BaseMap> maps;
    private int currentMap = 0;
    private Random rng;
    private Point mapSize;

    public GameWorld(Random rng, Point mapSize)
    {
        this.rng = rng;
        this.mapSize = mapSize;
        maps = [new ForestMap(this.mapSize, this.rng, new Point(1280,720))];
    }

    public BaseMap CurrentMap
    {
        get { return maps[currentMap]; }
    }
}
