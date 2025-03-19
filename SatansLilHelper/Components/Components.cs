using Friflo.Engine.ECS;

namespace SatansLilHelper.Components;

[ComponentKey("action-delay")]
public struct ActionDelay : IComponent
{
    public int value;
}

[ComponentKey("location")]
public struct Location(int x, int y) : IIndexedComponent<(int, int)>
{
    public int X = x,
        Y = y;

    public readonly (int, int) GetIndexedValue()
    {
        return (X, Y);
    }
}

[ComponentKey("attack")]
public struct Attack : IComponent
{
    public int dice,
        sides;
}

[ComponentKey("defense")]
public struct Defense : IComponent
{
    public int magic,
        physical;
}

[ComponentKey("health")]
public struct Health : IComponent
{
    public int Cur;
    public int Max;

    public Health(int max, int cur)
    {
        Max = max;
        Cur = cur;
    }

    public Health(int max)
    {
        Max = Cur = max;
    }
}

[ComponentKey("mana")]
public struct Mana : IComponent
{
    public int Cur;
    public int Max;

    public Mana(int max, int cur)
    {
        Max = max;
        Cur = cur;
    }

    public Mana(int max)
    {
        Max = Cur = max;
    }
}
