using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SatansLilHelper.Extensions;

public static class SpriteBatchExtensions
{
    public static void DrawString(
        this SpriteBatch spriteBatch,
        SpriteFont font,
        string text,
        Vector2 location,
        Color color,
        Vector2 origin
    )
    {
        spriteBatch.DrawString(font, text, location, color, 0f, origin, 1f, SpriteEffects.None, 1f);
    }

    public static void Draw(
        this SpriteBatch spriteBatch,
        Texture2D texture,
        Vector2 destination,
        Color color,
        Vector2 origin
    )
    {
        spriteBatch.Draw(texture, destination, null, color, 0f, origin, 1f, SpriteEffects.None, 1f);
    }
}
