using System;
using Friflo.Engine.ECS;
using Microsoft.Xna.Framework;
using SatansLilHelper.Constants;

namespace SatansLilHelper.Utils.GameMaps;

interface ICellGrid
{
    #region Methods
    public bool IsWall(Point tile);
    public void SetLight(Point tile, float distanceSquared);
    public void GenerateMap(Random rng, Point size);
    #endregion Methods

    #region Properties
    public int XDim { get; }
    public int YDim { get; }
    public Entity Player { get; }
    public EntityStore Registry { get; }
    public Tile[,] Tiles { get; }
    #endregion Properties
}
