using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using SatansLilHelper.Components;
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

    public static List<(int, int)> Dijkstra(Point start, (int X, int Y) end, ICellGrid gameMap)
    {
        return Dijkstra((start.X, start.Y), end, gameMap);
    }

    public static List<(int, int)> Dijkstra((int X, int Y) start, Point end, ICellGrid gameMap)
    {
        return Dijkstra(start, (end.X, end.Y), gameMap);
    }

    public static List<(int, int)> Dijkstra(Location start, Location end, ICellGrid gameMap)
    {
        return Dijkstra((start.X, start.Y), (end.X, end.Y), gameMap);
    }

    public static List<(int, int)> Dijkstra(
        (int X, int Y) start,
        (int X, int Y) end,
        ICellGrid gameMap
    )
    {
        PriorityQueue<(int, int), int> queue = new();
        Dictionary<(int, int), int> dist = [];
        Dictionary<(int, int), (int, int)> prev = [];

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
            if (current == end)
                break;
            foreach ((int X, int Y) neighbor in gameMap.GetNeighbors(current))
            {
                int alt = dist[current] + gameMap.GetMovementCost(neighbor);
                if (alt < dist[neighbor])
                {
                    prev[neighbor] = current;
                    dist[neighbor] = alt;
                    if (queue.Remove(neighbor, out (int, int) _, out int _))
                    {
                        queue.Enqueue(neighbor, alt);
                    }
                }
            }
        }
        List<(int, int)> path = [end];
        (int, int) temp = end;
        while (prev[temp] != start)
        {
            path.Add(prev[temp]);
            temp = prev[temp];
        }
        path.Reverse();
        return path;
    }
}
