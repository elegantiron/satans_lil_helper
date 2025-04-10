using SatansLilHelper.Constants;

namespace SatansLilHelper.Types;

public struct Tile(TextureID texture, bool walkable, bool passesLight)
{
    public TextureID Texture = texture;
    public bool Walkable = walkable;
    public bool PassesLight = passesLight;
    public bool Visible = false;
    public bool Explored = false;
    public float LightDistance = 0f;
    public int MovementCost = int.MinValue;
}
