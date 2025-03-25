using Microsoft.Xna.Framework;

namespace SatansLilHelper.Types;

public struct TextVecs
{
    public Vector2 Location,
        Origin;

    public TextVecs()
    {
        Location = Origin = Vector2.Zero;
    }
}
