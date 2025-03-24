using Microsoft.Xna.Framework;

namespace SatansLilHelper.Utils;

public static class Types
{
    public struct Tile(TextureID texture, bool walkable, bool passesLight)
    {
        public TextureID Texture = texture;
        public bool Walkable = walkable;
        public bool PassesLight = passesLight;
        public bool Visible = false;
        public bool Explored = false;
        public float LightDistance = 0f;
    }
}

public struct TextVecs
{
    public Vector2 Location,
        Origin;

    public TextVecs()
    {
        Location = Origin = Vector2.Zero;
    }
}

public struct LogMessage(string message, Color color, bool stack = true)
{
    public string Message = message;
    public Color Color = color;
    public bool Stack = stack;
}

public struct EventMessage { }
