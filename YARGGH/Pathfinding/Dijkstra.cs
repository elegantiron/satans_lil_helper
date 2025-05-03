using System.Collections.Generic;
using Microsoft.Xna.Framework;
using YARGGH.Mapping;

namespace YARGGH.Pathfinding;

public static class Dijkstra
{
    public static List<Point> GetPath(Point start, Point end, ICellGrid gameMap)
    {
        PriorityQueue<Point, int> queue = new();
        Dictionary<Point, int> dist = [];
        Dictionary<Point, Point> prev = [];

        dist[start] = 0;
        queue.Enqueue(start, 0);

        for (int i = 0; i < gameMap.xDim; i++)
        {
            for (int j = 0; j < gameMap.yDim; j++)
            {
                Point workingPoint = new(i, j);
                if (workingPoint != start)
                {
                    dist[workingPoint] = int.MaxValue;
                    prev[workingPoint] = new Point(-1, -1);
                    queue.Enqueue(workingPoint, int.MaxValue);
                }
            }
        }
        while (queue.Count > 0)
        {
            Point current = queue.Dequeue();
            if (current == end)
                break;
            foreach (Point neighbor in gameMap.GetNeighbors(current))
            {
                int alt = dist[current] + gameMap.GetMovementCost(neighbor);
                if (alt < dist[neighbor])
                {
                    prev[neighbor] = current;
                    dist[neighbor] = alt;
                    if (queue.Remove(neighbor, out Point _, out int _))
                    {
                        queue.Enqueue(neighbor, alt);
                    }
                }
            }
        }
        List<Point> path = [end];
        Point temp = end;
        while (prev[temp] != start)
        {
            path.Add(prev[temp]);
            temp = prev[temp];
        }
        path.Reverse();
        return path;
    }
}
