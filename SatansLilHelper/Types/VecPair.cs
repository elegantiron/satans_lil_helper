using Microsoft.Xna.Framework;

namespace SatansLilHelper.Types;

public struct VecPair
{
    public Vector2 Location,
        Origin;

    public VecPair()
    {
        Location = Origin = Vector2.Zero;
    }
}
