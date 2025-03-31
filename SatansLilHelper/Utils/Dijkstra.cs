using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SatansLilHelper.Interfaces;

namespace SatansLilHelper.Utils;

internal static partial class Pathfinders
{
    public static List<(int, int)> Dijkstra(Point start, Point end, ICellGrid gameMap)
    {
        return Dijkstra((start.X, start.Y), (end.X, end.Y), gameMap);
    }

    public static List<(int, int)> Dijkstra(Vector2 start, Vector2 end, ICellGrid gameMap)
    {
        return Dijkstra(((int)start.X, (int)start.Y), ((int)end.X, (int)end.Y), gameMap);
    }

    public static List<(int, int)> Dijkstra(
        (int X, int Y) start,
        (int X, int Y) end,
        ICellGrid gameMap
    )
    {
        PriorityQueue<(int, int), int> queue = new();
        Dictionary<(int, int), int> dist = new();
        Dictionary<(int, int), (int, int)> prev = new();

        dist[start] = 0;
        queue.Enqueue(start, 0);

        for (int i = 0; i < gameMap.XDim; i++)
        {
            for (int j = 0; j < gameMap.YDim; j++)
            {
                if ((i, j) != start)
                {
                    dist[(i, j)] = int.MaxValue;
                    prev[(i, j)] = (-1, -1);
                    queue.Enqueue((i, j), int.MaxValue);
                }
            }
        }
        while (queue.Count > 0)
        {
            (int X, int Y) current = queue.Dequeue();
            foreach ((int X, int Y) neighbor in gameMap.GetNeighbors(current))
            {
                int alt = dist[current] + gameMap.GetMovementCost(neighbor);
                if ( alt < dist[neighbor])
                {
                    prev[neighbor] = current;
                    dist[neighbor] = alt;
                    if(queue.Remove(neighbor, out (int, int) element, out int priority){
                        queue.Enqueue(neighbor, alt);
                    }
                }
            }
        }
        throw new NotImplementedException();
    }
}
