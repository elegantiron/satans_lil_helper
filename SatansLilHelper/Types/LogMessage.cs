using Microsoft.Xna.Framework;

namespace SatansLilHelper.Types;

public struct LogMessage(string message, Color color, bool stack = true)
{
    public string Message = message;
    public Color Color = color;
    public bool Stack = stack;
}
