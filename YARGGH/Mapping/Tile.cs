namespace YARGGH.Mapping;

public struct Tile(bool walkable, bool passesLight)
{
    public bool Walkable = walkable;
    public bool PassesLight = passesLight;
    public bool Visible = false;
    public bool Explored = false;
    public float LightDistance = 0f;
    public int MovementCost = int.MinValue;
}
