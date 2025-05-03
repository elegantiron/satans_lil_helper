using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace YARGGH.Lines;

public static class Bresenham
{
    public static List<Point> GetLine(Point start, Point end)
    {
        return GetLine(start.X, start.Y, end.X, end.Y);
    }

    public static List<Point> GetLine(int x1, int y1, int x2, int y2)
    {
        List<Point> points = [];
        int w = x2 - x1;
        int h = y2 - y1;
        int dx1 = 0,
            dy1 = 0,
            dx2 = 0,
            dy2 = 0;
        if (w < 0)
            dx1 = -1;
        else if (w > 0)
            dx1 = 1;
        if (h < 0)
            dy1 = -1;
        else if (h > 0)
            dy1 = 1;
        if (w < 0)
            dx2 = -1;
        else if (w > 0)
            dx2 = 1;
        int longest = Math.Abs(w);
        int shortest = Math.Abs(h);
        if (!(longest > shortest))
        {
            longest = Math.Abs(h);
            shortest = Math.Abs(w);
            if (h < 0)
                dy2 = -1;
            else if (h > 0)
                dy2 = 1;
            dx2 = 0;
        }
        int numerator = longest >> 1;
        for (int i = 0; i <= longest; i++)
        {
            points.Add(new Point(x1, y1));
            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                x1 += dx1;
                y1 += dy1;
            }
            else
            {
                x1 += dx2;
                y1 += dy2;
            }
        }
        return points;
    }
}
