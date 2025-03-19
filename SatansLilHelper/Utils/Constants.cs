using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SatansLilHelper.Utils;

public static class Constants
{
    public static readonly (int x, int y)[] NeighborDirections =
    [
        (-1, -1),
        (-1, 0),
        (-1, 1),
        (0, -1),
        (0, 1),
        (1, -1),
        (1, 0),
        (1, 1),
    ];

    public static readonly Dictionary<Keys, Point> MovementKeys = new()
    {
        { Keys.Up, new Point(0, -1) },
        { Keys.Down, new Point(0, 1) },
        { Keys.Right, new Point(1, 0) },
        { Keys.Left, new Point(-1, 0) },
        { Keys.Insert, new Point(-1, -1) },
        { Keys.NumPad1, new Point(-1, 1) },
        { Keys.NumPad2, new Point(0, 1) },
        { Keys.NumPad3, new Point(1, 1) },
        { Keys.NumPad4, new Point(-1, 0) },
        { Keys.NumPad6, new Point(1, 0) },
        { Keys.NumPad7, new Point(-1, -1) },
        { Keys.NumPad8, new Point(0, -1) },
        { Keys.NumPad9, new Point(1, -1) },
        { Keys.Delete, new Point(-1, 1) },
        { Keys.PageUp, new Point(1, -1) },
        { Keys.PageDown, new Point(1, 1) },
    };

    public static class Colors
    {
        public static readonly Color Impossible = new(0x80, 0x80, 0x80);
    }
}
