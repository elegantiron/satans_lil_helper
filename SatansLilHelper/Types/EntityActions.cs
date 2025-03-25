namespace SatansLilHelper.Types;

public struct EntityActions(int moves = 0, int attacks = 0)
{
    public int Moves = moves;
    public int Attacks = attacks;

    public EntityActions()
        : this(0, 0) { }
}
